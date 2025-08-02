package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;
import com.example.petcareapp.data.repository.AnimalRepository;
import com.example.petcareapp.data.room.entities.AnimalEntity;

import java.util.List;

import javax.inject.Inject;

public class AnimalViewModel extends ViewModel {
    private final AnimalRepository repository;
    private LiveData<List<AnimalEntity>> animals;
    private LiveData<AnimalEntity> selectedAnimal;

    @Inject
    public AnimalViewModel(AnimalRepository repository) {
        this.repository = repository;
    }

    public LiveData<List<AnimalEntity>> getAnimals() {
        if (animals == null) {
            animals = repository.getAnimals();
        }
        return animals;
    }

    public LiveData<AnimalEntity> getAnimalBySlug(String slug) {
        if (selectedAnimal == null) {
            selectedAnimal = repository.getAnimalBySlug(slug);
        }
        return selectedAnimal;
    }
}