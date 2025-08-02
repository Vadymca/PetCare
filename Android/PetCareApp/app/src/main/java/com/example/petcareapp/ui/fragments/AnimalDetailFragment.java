package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import androidx.fragment.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.databinding.FragmentAnimalDetailBinding;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.di.AppModule;
import com.example.petcareapp.di.DaggerAppComponent;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;

import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;

public class AnimalDetailFragment extends Fragment {
    private FragmentAnimalDetailBinding binding;
    private AnimalViewModel viewModel;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentAnimalDetailBinding.inflate(inflater, container, false);
        View view = binding.getRoot();

        // Ініціалізація ViewModel з Dagger
        viewModel = new ViewModelProvider(this, new ViewModelProvider.Factory() {
            @Override
            public <T extends ViewModel> T create(Class<T> modelClass) {
                AppComponent appComponent = DaggerAppComponent.builder()
                        .appModule(new AppModule(requireActivity().getApplication()))
                        .build();
                return (T) appComponent.getAnimalViewModel(); // Додай інжекцію
            }
        }).get(AnimalViewModel.class);

        // Отримай slug з аргументів
        String slug = getArguments() != null ? getArguments().getString("animalSlug", "") : "";

        // Спостерігай за даними з ViewModel
        viewModel.getAnimalBySlug(slug).observe(getViewLifecycleOwner(), animal -> {
            if (animal != null) {
                binding.textViewAnimalName.setText("Тварина: " + animal.name);
                binding.textViewAnimalStatus.setText("Статус: " + animal.status);
                // Додай Glide для фото пізніше
            }
        });

        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}