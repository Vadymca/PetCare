package com.example.petcareapp.ui.fragments;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.example.petcareapp.MainActivity;
import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.databinding.FragmentOnboarding3Binding;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.ui.viewmodels.OnboardingViewModel;

import javax.inject.Inject;

public class OnboardingFragment3 extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private OnboardingViewModel viewModel;
    private FragmentOnboarding3Binding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
        viewModel = new ViewModelProvider(requireActivity(), viewModelFactory).get(OnboardingViewModel.class);
    }

    @SuppressLint("SetTextI18n")
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentOnboarding3Binding.inflate(inflater, container, false);
        View view = binding.getRoot();

        binding.textTitle.setText("Почнемо\n знайомство?");
        binding.textDescription.setText("Ми допоможемо обрати тварину\n" +
                                         "або знайти те, чим ти можеш бути корисним.");
        binding.textDescription2.setText("Вітаємо тебе ");

        binding.buttonNext.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(view);
            navController.navigate(R.id.action_onboarding3_to_login);
        });

        binding.buttonSkip.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(view);
            navController.navigate(R.id.action_onboarding3_to_login);
        });

        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}