package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.ViewModel;

import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.repository.AnimalRepository;

import java.util.List;

import javax.inject.Inject;

public class ShelterListViewModel extends ViewModel {
    private final AnimalRepository repository;
    private LiveData<List<Shelter>> shelters;

    @Inject
    public ShelterListViewModel(AnimalRepository repository) {
        this.repository = repository;
    }

    public LiveData<List<Shelter>> getShelters() {
        if (shelters == null) {
            shelters = repository.getShelters(1, 10);
        }
        return shelters;
    }
}