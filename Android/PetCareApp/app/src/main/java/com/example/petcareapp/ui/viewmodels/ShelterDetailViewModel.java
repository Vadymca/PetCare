package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.repository.AnimalRepository;
import javax.inject.Inject;

public class ShelterDetailViewModel extends ViewModel {
    private final AnimalRepository repository;

    @Inject
    public ShelterDetailViewModel(AnimalRepository repository) {
        this.repository = repository;
    }

    public LiveData<Shelter> getShelterBySlug(String slug) {
        return repository.getShelterBySlug(slug);
    }
}