package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.navigation.fragment.NavHostFragment;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.databinding.FragmentRegistrationBinding;
import com.example.petcareapp.di.AppComponent;

import javax.inject.Inject;

public class RegistrationFragment extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private FragmentRegistrationBinding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentRegistrationBinding.inflate(inflater, container, false);
        View view = binding.getRoot();
        ImageButton back = view.findViewById(R.id.button_back_enter);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }
        binding.buttonEmail.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(view);
            navController.navigate(R.id.action_registration_to_part1);
        });
//        binding.buttonRegister.setOnClickListener(v -> {
//            // Add registration logic here (e.g., validate fields, call API)
//            NavController navController = Navigation.findNavController(view);
//            navController.navigate(R.id.action_registration_to_login);
//        });
        binding.buttonLogin.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(view);
            navController.navigate(R.id.action_registration_to_login);
        });

        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}