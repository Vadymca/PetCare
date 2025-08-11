package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Breed;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.models.Species;
import com.example.petcareapp.data.models.User;
import com.example.petcareapp.data.repository.AnimalRepository;
import com.example.petcareapp.data.room.entities.AnimalEntity;

import java.util.List;

import javax.inject.Inject;

public class AnimalViewModel extends ViewModel {
    private final AnimalRepository repository;
    private final MutableLiveData<List<Animal>> animals = new MutableLiveData<>();
    private final MutableLiveData<List<Shelter>> shelters = new MutableLiveData<>();
    private final MutableLiveData<List<Species>> species = new MutableLiveData<>();
    private final MutableLiveData<List<Breed>> breeds = new MutableLiveData<>();

    @Inject
    public AnimalViewModel(AnimalRepository repository) {
        this.repository = repository;
    }

    public LiveData<List<Animal>> getAnimals(int page, int size) {
        repository.getAnimals(page, size).observeForever(animals::setValue);
        return animals;
    }

    public LiveData<Animal> getAnimalBySlug(String slug) {
        return repository.getAnimalBySlug(slug);
    }

    public LiveData<List<Shelter>> getShelters(int page, int size) {
        repository.getShelters(page, size).observeForever(shelters::setValue);
        return shelters;
    }

    public LiveData<Shelter> getShelterBySlug(String slug) {
        return repository.getShelterBySlug(slug);
    }

    public LiveData<List<Species>> getSpecies() {
        repository.getSpecies().observeForever(species::setValue);
        return species;
    }

    public LiveData<List<Breed>> getBreeds() {
        repository.getBreeds().observeForever(breeds::setValue);
        return breeds;
    }

    public LiveData<User> getUserById(String id) {
        return repository.getUserById(id);
    }
}
//public class AnimalViewModel extends ViewModel {
//    private final AnimalRepository repository;
//    private final LiveData<List<Animal>> animals;
//    private final LiveData<List<Shelter>> shelters;
//    private final LiveData<List<Species>> species;
//    private final LiveData<List<Breed>> breeds;
//
//    @Inject
//    public AnimalViewModel(AnimalRepository repository) {
//        this.repository = repository;
//        this.animals = repository.getAnimals(1, 10); // Початкова сторінка
//        this.shelters = repository.getShelters(1, 10);
//        this.species = repository.getSpecies();
//        this.breeds = repository.getBreeds();
//    }
//
//    public LiveData<List<Animal>> getAnimals(int page, int size) {
//        return repository.getAnimals(page, size);
//    }
//
//    public LiveData<Animal> getAnimalBySlug(String slug) {
//        return repository.getAnimalBySlug(slug);
//    }
//
//    public LiveData<List<Shelter>> getShelters(int page, int size) {
//        return repository.getShelters(page, size);
//    }
//
//    public LiveData<Shelter> getShelterBySlug(String slug) {
//        return repository.getShelterBySlug(slug);
//    }
//
//    public LiveData<List<Species>> getSpecies() {
//        return species;
//    }
//
//    public LiveData<List<Breed>> getBreeds() {
//        return breeds;
//    }
//
//    public LiveData<User> getUserById(String id) {
//        return repository.getUserById(id);
//    }
//}