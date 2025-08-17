package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.repository.AnimalRepository;
import javax.inject.Inject;

public class AnimalDetailViewModel extends ViewModel {
    private final AnimalRepository repository;
    @Inject
    public AnimalDetailViewModel(AnimalRepository repository) {
        this.repository = repository;
    }
    public LiveData<Animal> getAnimalBySlug(String slug) {
        return repository.getAnimalBySlug(slug);
    }
}