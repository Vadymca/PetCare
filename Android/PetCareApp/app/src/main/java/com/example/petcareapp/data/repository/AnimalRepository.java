package com.example.petcareapp.data.repository;

import android.util.Log;
import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import com.example.petcareapp.data.api.ApiService;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.room.daos.AnimalDao;
import com.example.petcareapp.data.room.daos.ShelterDao;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import com.example.petcareapp.data.room.entities.ShelterEntity;
import java.util.List;
import java.util.stream.Collectors;
import javax.inject.Inject;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AnimalRepository {
    private final ApiService apiService;
    private final AnimalDao animalDao;
    private final ShelterDao shelterDao;

    @Inject
    public AnimalRepository(ApiService apiService, AnimalDao animalDao, ShelterDao shelterDao) {
        this.apiService = apiService;
        this.animalDao = animalDao;
        this.shelterDao = shelterDao;
    }

    public LiveData<List<Animal>> getAnimals(int page, int size) {
        MutableLiveData<List<Animal>> data = new MutableLiveData<>();
        List<AnimalEntity> cachedAnimals = animalDao.getAll();
        if (!cachedAnimals.isEmpty()) {
            data.setValue(convertEntitiesToModels(cachedAnimals));
            // Виклик API лише якщо потрібно оновити (наприклад, page не кешовано)
            if (shouldUpdateCache(page, size)) {
                fetchAndCacheAnimals(page, size, data);
            }
        } else {
            fetchAndCacheAnimals(page, size, data);
        }
        return data;
    }

    private void fetchAndCacheAnimals(int page, int size, MutableLiveData<List<Animal>> data) {
        apiService.getAnimals(page, size).enqueue(new Callback<List<Animal>>() {
            @Override
            public void onResponse(Call<List<Animal>> call, Response<List<Animal>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    List<Animal> animals = response.body();
                    for (Animal animal : animals) {
                        animalDao.insert(new AnimalEntity(animal.id, animal.slug, animal.name, animal.breedId, animal.status, animal.photos));
                    }
                    data.setValue(animals);
                }
            }
            @Override
            public void onFailure(Call<List<Animal>> call, Throwable t) {
                data.setValue(convertEntitiesToModels(animalDao.getAll()));
                Log.e("AnimalRepository", "API Failure", t);
            }
        });
    }

    private boolean shouldUpdateCache(int page, int size) {
        // Проста логіка: оновлювати, якщо page > 1 або кеш застарілий
        // Для повноти можна додати перевірку часу або версії кешу
        return page > 1 || animalDao.getAll().isEmpty();
    }

    public LiveData<Animal> getAnimalBySlug(String slug) {
        MutableLiveData<Animal> data = new MutableLiveData<>();
        AnimalEntity entity = animalDao.getBySlug(slug);
        if (entity != null) {
            data.setValue(new Animal(entity.id, entity.slug, entity.name, entity.breedId, entity.status, entity.photos, null));
        } else {
            apiService.getAnimalBySlug(slug).enqueue(new Callback<Animal>() {
                @Override
                public void onResponse(Call<Animal> call, Response<Animal> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        Animal animal = response.body();
                        animalDao.insert(new AnimalEntity(animal.id, animal.slug, animal.name, animal.breedId, animal.status, animal.photos));
                        data.setValue(animal);
                    }
                }
                @Override
                public void onFailure(Call<Animal> call, Throwable t) {
                    data.setValue(entity != null ? new Animal(entity.id, entity.slug, entity.name, entity.breedId, entity.status, entity.photos, null) : null);
                    Log.e("AnimalRepository", "API Failure", t);
                }
            });
        }
        return data;
    }

    public LiveData<List<Shelter>> getShelters(int page, int size) {
        MutableLiveData<List<Shelter>> data = new MutableLiveData<>();
        List<ShelterEntity> cachedShelters = shelterDao.getAll();
        if (!cachedShelters.isEmpty()) {
            data.setValue(convertShelterEntitiesToModels(cachedShelters));
            if (shouldUpdateCache(page, size)) {
                fetchAndCacheShelters(page, size, data);
            }
        } else {
            fetchAndCacheShelters(page, size, data);
        }
        return data;
    }

    private void fetchAndCacheShelters(int page, int size, MutableLiveData<List<Shelter>> data) {
        apiService.getShelters(page, size).enqueue(new Callback<List<Shelter>>() {
            @Override
            public void onResponse(Call<List<Shelter>> call, Response<List<Shelter>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    List<Shelter> shelters = response.body();
                    for (Shelter shelter : shelters) {
                        shelterDao.insert(new ShelterEntity(shelter.id, shelter.slug, shelter.name, shelter.address, shelter.coordinates, shelter.photos));
                    }
                    data.setValue(shelters);
                }
            }
            @Override
            public void onFailure(Call<List<Shelter>> call, Throwable t) {
                data.setValue(convertShelterEntitiesToModels(shelterDao.getAll()));
                Log.e("AnimalRepository", "API Failure", t);
            }
        });
    }

    public LiveData<Shelter> getShelterBySlug(String slug) {
        MutableLiveData<Shelter> data = new MutableLiveData<>();
        ShelterEntity entity = shelterDao.getBySlug(slug);
        if (entity != null) {
            data.setValue(new Shelter(entity.id, entity.slug, entity.name, entity.address, entity.coordinates, entity.photos));
        } else {
            apiService.getShelterBySlug(slug).enqueue(new Callback<Shelter>() {
                @Override
                public void onResponse(Call<Shelter> call, Response<Shelter> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        Shelter shelter = response.body();
                        shelterDao.insert(new ShelterEntity(shelter.id, shelter.slug, shelter.name, shelter.address, shelter.coordinates, shelter.photos));
                        data.setValue(shelter);
                    }
                }
                @Override
                public void onFailure(Call<Shelter> call, Throwable t) {
                    data.setValue(entity != null ? new Shelter(entity.id, entity.slug, entity.name, entity.address, entity.coordinates, entity.photos) : null);
                    Log.e("AnimalRepository", "API Failure", t);
                }
            });
        }
        return data;
    }

    private List<Animal> convertEntitiesToModels(List<AnimalEntity> entities) {
        return entities.stream().map(e -> new Animal(e.id, e.slug, e.name, e.breedId, e.status, e.photos, null)).collect(Collectors.toList());
    }

    private List<Shelter> convertShelterEntitiesToModels(List<ShelterEntity> entities) {
        return entities.stream().map(e -> new Shelter(e.id, e.slug, e.name, e.address, e.coordinates, e.photos)).collect(Collectors.toList());
    }
}