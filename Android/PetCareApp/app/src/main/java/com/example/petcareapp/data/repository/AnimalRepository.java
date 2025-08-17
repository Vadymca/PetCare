package com.example.petcareapp.data.repository;

import android.util.Log;
import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import com.example.petcareapp.data.api.ApiService;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Breed;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.models.Species;
import com.example.petcareapp.data.models.User;
import com.example.petcareapp.data.room.daos.AnimalDao;
import com.example.petcareapp.data.room.daos.BreedDao;
import com.example.petcareapp.data.room.daos.ShelterDao;
import com.example.petcareapp.data.room.daos.SpeciesDao;
import com.example.petcareapp.data.room.daos.UserDao;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import com.example.petcareapp.data.room.entities.BreedEntity;
import com.example.petcareapp.data.room.entities.ShelterEntity;
import com.example.petcareapp.data.room.entities.SpeciesEntity;
import com.example.petcareapp.data.room.entities.UserEntity;

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
    private final SpeciesDao speciesDao;
    private final BreedDao breedDao;
    private final UserDao userDao;

    @Inject
    public AnimalRepository(ApiService apiService, AnimalDao animalDao, ShelterDao shelterDao,
                            SpeciesDao speciesDao, BreedDao breedDao, UserDao userDao) {
        this.apiService = apiService;
        this.animalDao = animalDao;
        this.shelterDao = shelterDao;
        this.speciesDao = speciesDao;
        this.breedDao = breedDao;
        this.userDao = userDao;
    }


    public LiveData<List<Animal>> getAnimals(int page, int size) {
        MutableLiveData<List<Animal>> data = new MutableLiveData<>();
        List<AnimalEntity> cachedAnimals = animalDao.getAll();
        if (!cachedAnimals.isEmpty()) {
            data.setValue(convertAnimalEntitiesToModels(cachedAnimals));
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
                    List<AnimalEntity> entities = animals.stream()
                            .map(animal -> new AnimalEntity(
                                    animal.getId(),
                                    animal.getSlug(),
                                    animal.getName(),
                                    animal.getBreedId(),
                                    animal.getStatus(),
                                    animal.getPhotos(),
                                    animal.getShelterId(),
                                    animal.getUserId(),
                                    animal.getBirthday(),
                                    animal.getGender(),
                                    animal.getDescription(),
                                    animal.getHealthStatus(),
                                    animal.getVideos(),
                                    animal.getAdoptionRequirements(),
                                    animal.getMicrochipId(),
                                    animal.getIdNumber(),
                                    animal.getWeight(),
                                    animal.getHeight(),
                                    animal.getColor(),
                                    animal.isSterilized(),
                                    animal.isHaveDocuments(),
                                    animal.getCreatedAt(),
                                    animal.getUpdatedAt()
                            ))
                            .collect(Collectors.toList());
                    animalDao.insertAll(entities);
                    data.setValue(animals);
                }
            }

            @Override
            public void onFailure(Call<List<Animal>> call, Throwable t) {
                data.setValue(convertAnimalEntitiesToModels(animalDao.getAll()));
                Log.e("AnimalRepository", "API Failure", t);
            }
        });
    }

    public LiveData<Animal> getAnimalBySlug(String slug) {
        MutableLiveData<Animal> data = new MutableLiveData<>();
        AnimalEntity entity = animalDao.getBySlug(slug);
        if (entity != null) {
            data.setValue(convertAnimalEntityToModel(entity));
        } else {
            apiService.getAnimalBySlug(slug).enqueue(new Callback<Animal>() {
                @Override
                public void onResponse(Call<Animal> call, Response<Animal> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        Animal animal = response.body();
                        AnimalEntity entity = new AnimalEntity(
                                animal.getId(),
                                animal.getSlug(),
                                animal.getName(),
                                animal.getBreedId(),
                                animal.getStatus(),
                                animal.getPhotos(),
                                animal.getShelterId(),
                                animal.getUserId(),
                                animal.getBirthday(),
                                animal.getGender(),
                                animal.getDescription(),
                                animal.getHealthStatus(),
                                animal.getVideos(),
                                animal.getAdoptionRequirements(),
                                animal.getMicrochipId(),
                                animal.getIdNumber(),
                                animal.getWeight(),
                                animal.getHeight(),
                                animal.getColor(),
                                animal.isSterilized(),
                                animal.isHaveDocuments(),
                                animal.getCreatedAt(),
                                animal.getUpdatedAt()
                        );
                        animalDao.insert(entity);
                        data.setValue(animal);
                    }
                }

                @Override
                public void onFailure(Call<Animal> call, Throwable t) {
                    AnimalEntity cachedEntity = animalDao.getBySlug(slug);
                    data.setValue(cachedEntity != null ? convertAnimalEntityToModel(cachedEntity) : null);
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
                    List<ShelterEntity> entities = shelters.stream()
                            .map(shelter -> new ShelterEntity(
                                    shelter.getId(),
                                    shelter.getSlug(),
                                    shelter.getName(),
                                    shelter.getAddress(),
                                    shelter.getCoordinates(),
                                    shelter.getContactPhone(),
                                    shelter.getContactEmail(),
                                    shelter.getDescription(),
                                    shelter.getCapacity(),
                                    shelter.getCurrentOccupancy(),
                                    shelter.getPhotos(),
                                    shelter.getVirtualTourUrl(),
                                    shelter.getWorkingHours(),
                                    shelter.getSocialMedia(),
                                    shelter.getManagerId(),
                                    shelter.getCreatedAt(),
                                    shelter.getUpdatedAt()
                            ))
                            .collect(Collectors.toList());
                    shelterDao.insertAll(entities);
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
            data.setValue(convertShelterEntityToModel(entity));
        } else {
            apiService.getShelterBySlug(slug).enqueue(new Callback<Shelter>() {
                @Override
                public void onResponse(Call<Shelter> call, Response<Shelter> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        Shelter shelter = response.body();
                        ShelterEntity entity = new ShelterEntity(
                                shelter.getId(),
                                shelter.getSlug(),
                                shelter.getName(),
                                shelter.getAddress(),
                                shelter.getCoordinates(),
                                shelter.getContactPhone(),
                                shelter.getContactEmail(),
                                shelter.getDescription(),
                                shelter.getCapacity(),
                                shelter.getCurrentOccupancy(),
                                shelter.getPhotos(),
                                shelter.getVirtualTourUrl(),
                                shelter.getWorkingHours(),
                                shelter.getSocialMedia(),
                                shelter.getManagerId(),
                                shelter.getCreatedAt(),
                                shelter.getUpdatedAt()
                        );
                        shelterDao.insert(entity);
                        data.setValue(shelter);
                    }
                }

                @Override
                public void onFailure(Call<Shelter> call, Throwable t) {
                    ShelterEntity cachedEntity = shelterDao.getBySlug(slug);
                    data.setValue(cachedEntity != null ? convertShelterEntityToModel(cachedEntity) : null);
                    Log.e("AnimalRepository", "API Failure", t);
                }
            });
        }
        return data;
    }

    public LiveData<List<Species>> getSpecies() {
        MutableLiveData<List<Species>> data = new MutableLiveData<>();
        List<SpeciesEntity> cachedSpecies = speciesDao.getAllSpecies();
        if (!cachedSpecies.isEmpty()) {
            data.setValue(convertSpeciesEntitiesToModels(cachedSpecies));
        } else {
            apiService.getSpecies().enqueue(new Callback<List<Species>>() {
                @Override
                public void onResponse(Call<List<Species>> call, Response<List<Species>> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        List<Species> species = response.body();
                        List<SpeciesEntity> entities = species.stream()
                                .map(s -> new SpeciesEntity(s.getId(), s.getName()))
                                .collect(Collectors.toList());
                        speciesDao.insertAll(entities);
                        data.setValue(species);
                    }
                }

                @Override
                public void onFailure(Call<List<Species>> call, Throwable t) {
                    data.setValue(convertSpeciesEntitiesToModels(speciesDao.getAllSpecies()));
                    Log.e("AnimalRepository", "API Failure", t);
                }
            });
        }
        return data;
    }

    public LiveData<List<Breed>> getBreeds() {
        MutableLiveData<List<Breed>> data = new MutableLiveData<>();
        List<BreedEntity> cachedBreeds = breedDao.getAllBreeds();
        if (!cachedBreeds.isEmpty()) {
            data.setValue(convertBreedEntitiesToModels(cachedBreeds));
        } else {
            apiService.getBreeds().enqueue(new Callback<List<Breed>>() {
                @Override
                public void onResponse(Call<List<Breed>> call, Response<List<Breed>> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        List<Breed> breeds = response.body();
                        List<BreedEntity> entities = breeds.stream()
                                .map(b -> new BreedEntity(b.getId(), b.getSpeciesId(), b.getName(), b.getDescription()))
                                .collect(Collectors.toList());
                        breedDao.insertAll(entities);
                        data.setValue(breeds);
                    }
                }

                @Override
                public void onFailure(Call<List<Breed>> call, Throwable t) {
                    data.setValue(convertBreedEntitiesToModels(breedDao.getAllBreeds()));
                    Log.e("AnimalRepository", "API Failure", t);
                }
            });
        }
        return data;
    }

    public LiveData<User> getUserById(String id) {
        MutableLiveData<User> data = new MutableLiveData<>();
        UserEntity entity = userDao.getUserById(id);
        if (entity != null) {
            data.setValue(convertUserEntityToModel(entity));
        } else {
            apiService.getUserById(id).enqueue(new Callback<User>() {
                @Override
                public void onResponse(Call<User> call, Response<User> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        User user = response.body();
                        UserEntity userEntity = new UserEntity(
                                user.getId(),
                                user.getEmail(),
                                user.getFirstName(),
                                user.getLastName(),
                                user.getPhone(),
                                user.getRole(),
                                user.getLanguage()
                        );
                        userDao.insert(userEntity);
                        data.setValue(user);
                    }
                }

                @Override
                public void onFailure(Call<User> call, Throwable t) {
                    UserEntity cachedEntity = userDao.getUserById(id);
                    data.setValue(cachedEntity != null ? convertUserEntityToModel(cachedEntity) : null);
                    Log.e("AnimalRepository", "API Failure", t);
                }
            });
        }
        return data;
    }

    private boolean shouldUpdateCache(int page, int size) {
        return page > 1 || animalDao.getAll().isEmpty();
    }

    private List<Animal> convertAnimalEntitiesToModels(List<AnimalEntity> entities) {
        return entities.stream()
                .map(this::convertAnimalEntityToModel)
                .collect(Collectors.toList());
    }

    private Animal convertAnimalEntityToModel(AnimalEntity entity) {
        Animal animal = new Animal();
        animal.setId(entity.getId());
        animal.setSlug(entity.getSlug());
        animal.setName(entity.getName());
        animal.setBreedId(entity.getBreedId());
        animal.setStatus(entity.getStatus());
        animal.setPhotos(entity.getPhotos());
        animal.setShelterId(entity.getShelterId());
        animal.setUserId(entity.getUserId());
        animal.setBirthday(entity.getBirthday());
        animal.setGender(entity.getGender());
        animal.setDescription(entity.getDescription());
        animal.setHealthStatus(entity.getHealthStatus());
        animal.setVideos(entity.getVideos());
        animal.setAdoptionRequirements(entity.getAdoptionRequirements());
        animal.setMicrochipId(entity.getMicrochipId());
        animal.setIdNumber(entity.getIdNumber());
        animal.setWeight(entity.getWeight());
        animal.setHeight(entity.getHeight());
        animal.setColor(entity.getColor());
        animal.setSterilized(entity.isSterilized());
        animal.setHaveDocuments(entity.isHaveDocuments());
        animal.setCreatedAt(entity.getCreatedAt());
        animal.setUpdatedAt(entity.getUpdatedAt());
        return animal;
    }

    private List<Shelter> convertShelterEntitiesToModels(List<ShelterEntity> entities) {
        return entities.stream()
                .map(this::convertShelterEntityToModel)
                .collect(Collectors.toList());
    }

    private Shelter convertShelterEntityToModel(ShelterEntity entity) {
        Shelter shelter = new Shelter();
        shelter.setId(entity.getId());
        shelter.setSlug(entity.getSlug());
        shelter.setName(entity.getName());
        shelter.setAddress(entity.getAddress());
        shelter.setCoordinates(entity.getCoordinates());
        shelter.setContactPhone(entity.getContactPhone());
        shelter.setContactEmail(entity.getContactEmail());
        shelter.setDescription(entity.getDescription());
        shelter.setCapacity(entity.getCapacity());
        shelter.setCurrentOccupancy(entity.getCurrentOccupancy());
        shelter.setPhotos(entity.getPhotos());
        shelter.setVirtualTourUrl(entity.getVirtualTourUrl());
        shelter.setWorkingHours(entity.getWorkingHours());
        shelter.setSocialMedia(entity.getSocialMedia());
        shelter.setManagerId(entity.getManagerId());
        shelter.setCreatedAt(entity.getCreatedAt());
        shelter.setUpdatedAt(entity.getUpdatedAt());
        return shelter;
    }

    private List<Species> convertSpeciesEntitiesToModels(List<SpeciesEntity> entities) {
        return entities.stream()
                .map(e -> {
                    Species species = new Species();
                    species.setId(e.getId());
                    species.setName(e.getName());
                    return species;
                })
                .collect(Collectors.toList());
    }

    private List<Breed> convertBreedEntitiesToModels(List<BreedEntity> entities) {
        return entities.stream()
                .map(e -> {
                    Breed breed = new Breed();
                    breed.setId(e.getId());
                    breed.setSpeciesId(e.getSpeciesId());
                    breed.setName(e.getName());
                    breed.setDescription(e.getDescription());
                    return breed;
                })
                .collect(Collectors.toList());
    }

    private User convertUserEntityToModel(UserEntity entity) {
        User user = new User();
        user.setId(entity.getId());
        user.setEmail(entity.getEmail());
        user.setFirstName(entity.getFirstName());
        user.setLastName(entity.getLastName());
        user.setPhone(entity.getPhone());
        user.setRole(entity.getRole());
        user.setLanguage(entity.getLanguage());
        return user;
    }

}