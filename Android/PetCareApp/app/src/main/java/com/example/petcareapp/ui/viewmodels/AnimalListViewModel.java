package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.repository.AnimalRepository;

import java.util.List;

import javax.inject.Inject;

public class AnimalListViewModel extends ViewModel {
    private final AnimalRepository repository;
    private LiveData<List<Animal>> animals;

    @Inject
    public AnimalListViewModel(AnimalRepository repository) {
        this.repository = repository;
    }

    public LiveData<List<Animal>> getAnimals() {
        if (animals == null) {
            animals = repository.getAnimals(1, 10); // Припустимо, що є метод для PagingData
        }
        return animals;
    }
}