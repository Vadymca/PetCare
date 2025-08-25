package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.Navigation;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.di.AppModule;
import com.example.petcareapp.ui.viewmodels.AuthViewModel;

import javax.inject.Inject;

public class TwoFactorFragment extends Fragment {
    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private AuthViewModel viewModel;
    private EditText editTextCode;
    private Button buttonVerify;
    private ProgressBar progressBar;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component;
        component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
        viewModel = new ViewModelProvider(this, viewModelFactory).get(AuthViewModel.class);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_two_factor, container, false);
        editTextCode = view.findViewById(R.id.edit_text_code);
        buttonVerify = view.findViewById(R.id.button_verify);
        progressBar = view.findViewById(R.id.progress_bar);

        String userId = requireArguments().getString("userId");
        if (userId == null) {
            Toast.makeText(requireContext(), "User ID not found", Toast.LENGTH_SHORT).show();
            return view;
        }

        buttonVerify.setOnClickListener(v -> {
            String code = editTextCode.getText().toString().trim();
            if (code.isEmpty()) {
                Toast.makeText(requireContext(), "Enter 2FA code", Toast.LENGTH_SHORT).show();
                return;
            }
            progressBar.setVisibility(View.VISIBLE);
            viewModel.verifyTwoFactor(userId, code).observe(getViewLifecycleOwner(), result -> {
                progressBar.setVisibility(View.GONE);
                if (result != null) {
                    Navigation.findNavController(v).navigate(R.id.action_twoFactor_to_animalList);
                } else {
                    Toast.makeText(requireContext(), "Invalid 2FA code", Toast.LENGTH_SHORT).show();
                }
            });
        });

        return view;
    }
}