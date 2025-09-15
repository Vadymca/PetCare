package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.repository.AnimalRepository;

import java.util.List;

import javax.inject.Inject;

public class HomeViewModel extends ViewModel {
    private final AnimalRepository animalRepository;

    private final MutableLiveData<List<Animal>> animals = new MutableLiveData<>();
    private final MutableLiveData<List<Shelter>> shelters = new MutableLiveData<>();

    @Inject
    public HomeViewModel(AnimalRepository animalRepository) {
        this.animalRepository = animalRepository;
    }

    public LiveData<List<Animal>> getAnimals(int page, int size) {
        animalRepository.getAnimals(page, size).observeForever(animals::setValue);
        return animals;
    }

    public LiveData<List<Shelter>> getShelters(int page, int size) {
        animalRepository.getShelters(page, size).observeForever(shelters::setValue);
        return shelters;
    }
}