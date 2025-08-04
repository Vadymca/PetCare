package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.repository.AnimalRepository;
import com.example.petcareapp.data.room.entities.AnimalEntity;

import java.util.List;

import javax.inject.Inject;

public class AnimalViewModel extends ViewModel {
    private final AnimalRepository repository;
    private LiveData<List<Animal>> animals; // Змінено на Animal замість AnimalEntity
    private LiveData<Animal> selectedAnimal;

    @Inject
    public AnimalViewModel(AnimalRepository repository) {
        this.repository = repository;
    }

    public LiveData<List<Animal>> getAnimals() {
        if (animals == null) {
            animals = repository.getAnimals(1, 10); // Тимчасові значення page=1, size=10
        }
        return animals;
    }

    public LiveData<Animal> getAnimalBySlug(String slug) {
        if (selectedAnimal == null) {
            selectedAnimal = repository.getAnimalBySlug(slug);
        }
        return selectedAnimal;
    }
}