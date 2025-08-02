package com.example.petcareapp.data.repository;

import android.util.Log;
import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import com.example.petcareapp.data.api.ApiClient;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.room.daos.AnimalDao;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import java.util.List;
import javax.inject.Inject;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AnimalRepository {
    private final ApiClient apiClient;
    private final AnimalDao animalDao;

    @Inject
    public AnimalRepository(ApiClient apiClient, AnimalDao animalDao) {
        this.apiClient = apiClient;
        this.animalDao = animalDao;
    }

    public LiveData<List<AnimalEntity>> getAnimals() {
        MutableLiveData<List<AnimalEntity>> data = new MutableLiveData<>();
        apiClient.getApiService().getAnimals().enqueue(new Callback<List<Animal>>() {
            @Override
            public void onResponse(Call<List<Animal>> call, Response<List<Animal>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    List<Animal> animals = response.body();
                    for (Animal animal : animals) {
                        animalDao.insert(new AnimalEntity(
                                animal.id,
                                animal.slug,
                                animal.name,
                                animal.breedId,
                                animal.status,
                                animal.photos
                        ));
                    }
                    data.setValue(animalDao.getAll());
                }
            }

            @Override
            public void onFailure(Call<List<Animal>> call, Throwable t) {
                data.setValue(animalDao.getAll()); // Повертаємо кеш при помилці
                Log.e("AnimalRepository", "API Failure", t);
            }
        });
        return data;
    }

    public LiveData<AnimalEntity> getAnimalBySlug(String slug) {
        MutableLiveData<AnimalEntity> data = new MutableLiveData<>();
        data.setValue(animalDao.getBySlug(slug));
        return data;
    }
}