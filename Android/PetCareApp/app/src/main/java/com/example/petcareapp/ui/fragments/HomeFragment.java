package com.example.petcareapp.ui.fragments;

import android.content.Context;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.databinding.FragmentHomeBinding;
import com.example.petcareapp.ui.adapters.AnimalAdapter;
import com.example.petcareapp.ui.viewmodels.HomeViewModel;

import java.util.ArrayList;

import javax.inject.Inject;

public class HomeFragment extends Fragment {

    private FragmentHomeBinding binding;
    private HomeViewModel homeViewModel;
    private AnimalAdapter animalAdapter;

    @Inject
    ViewModelProvider.Factory viewModelFactory;

    @Override
    public void onAttach(@NonNull Context context) {
        super.onAttach(context);
        ((PetCareApplication) requireActivity().getApplication())
                .getAppComponent()
                .inject(this);
    }
    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        binding = FragmentHomeBinding.inflate(inflater, container, false);

        setupDrawerMenu();
        setupRecyclerView();

        homeViewModel = new ViewModelProvider(this, viewModelFactory).get(HomeViewModel.class);

        observeData();

        return binding.getRoot();
    }

    private void setupDrawerMenu() {
        DrawerLayout drawerLayout = binding.drawerLayout;
        binding.buttonMenu.setOnClickListener(v ->
                drawerLayout.openDrawer(binding.navigationView));
    }

    private void setupRecyclerView() {
        animalAdapter = new AnimalAdapter(new ArrayList<>());
        binding.recyclerPets.setLayoutManager(new LinearLayoutManager(getContext()));
        binding.recyclerPets.setAdapter(animalAdapter);
    }

    private void observeData() {
        homeViewModel.getAnimals(1, 10).observe(getViewLifecycleOwner(), animals -> {
            if (animals != null) {
                animalAdapter.updateAnimals(animals);
            }
        });
    }
}