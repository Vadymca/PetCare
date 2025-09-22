package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.databinding.FragmentOnboarding2Binding;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.ui.viewmodels.OnboardingViewModel;

import javax.inject.Inject;

public class OnboardingFragment2 extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private OnboardingViewModel viewModel;
    private FragmentOnboarding2Binding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
        viewModel = new ViewModelProvider(requireActivity(), viewModelFactory).get(OnboardingViewModel.class);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentOnboarding2Binding.inflate(inflater, container, false);
        View view = binding.getRoot();

        binding.textTitle.setText("Твоя допомога \n важлива");
        binding.textDescription.setText("Усиновлення, волонтерство чи донат — кожен обирає свій спосіб підтримки.");

        binding.buttonNext.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(view);
            navController.navigate(R.id.action_onboarding2_to_onboarding3);
        });

        binding.buttonSkip.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(view);
            navController.navigate(R.id.action_onboarding2_to_login);
        });

        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}