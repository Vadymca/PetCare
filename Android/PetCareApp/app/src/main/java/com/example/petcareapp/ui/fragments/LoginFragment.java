package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.Navigation;
import androidx.navigation.fragment.NavHostFragment;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.ui.viewmodels.AuthViewModel;
import com.google.android.material.textfield.TextInputLayout;

import javax.inject.Inject;

public class LoginFragment extends Fragment {
    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private AuthViewModel viewModel;
    private EditText editTextEmail, editTextPassword;
    private Button buttonLogin;
    private ProgressBar progressBar;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
        viewModel = new ViewModelProvider(this, viewModelFactory).get(AuthViewModel.class);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_login, container, false);
        editTextEmail = view.findViewById(R.id.edit_text_email);
        editTextPassword = view.findViewById(R.id.edit_text_password);
        buttonLogin = view.findViewById(R.id.button_login);
        progressBar = view.findViewById(R.id.progress_bar);
        ImageButton back = view.findViewById(R.id.button_back_registration);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }
        TextInputLayout layoutEmail = view.findViewById(R.id.layout_email);
        TextInputLayout layoutPassword = view.findViewById(R.id.layout_password);

        TextView forgotPassword = view.findViewById(R.id.text_forgot_password);
        forgotPassword.setOnClickListener(v -> {
            Navigation.findNavController(v).navigate(R.id.resetPasswordFragment);
        });
        buttonLogin.setOnClickListener(v -> {
            String email = editTextEmail.getText().toString().trim();
            String password = editTextPassword.getText().toString().trim();

            boolean hasError = false;

            if (email.isEmpty()) {
                layoutEmail.setError("Введіть номер телефону або адресу електронної пошти");
                hasError = true;
            } else {
                layoutEmail.setError(null);
            }

            if (password.isEmpty()) {
                layoutPassword.setError("Введіть пароль");
                hasError = true;
            } else {
                layoutPassword.setError(null);
            }

            if (hasError) return;

            progressBar.setVisibility(View.VISIBLE);

            viewModel.login(email, password).observe(getViewLifecycleOwner(), response -> {
                progressBar.setVisibility(View.GONE);
                if (response == null) {
                    layoutPassword.setError("Невірний пароль. Спробуйте ще раз або натисніть «Забули пароль?».");
                } else {
                    Navigation.findNavController(v).navigate(R.id.action_login_to_animalList);
                }
            });
        });
//        buttonLogin.setOnClickListener(v -> {
//            String email = editTextEmail.getText().toString().trim();
//            String password = editTextPassword.getText().toString().trim();
//            if (email.isEmpty() || password.isEmpty()) {
//                Toast.makeText(requireContext(), "Enter email and password", Toast.LENGTH_SHORT).show();
//                return;
//            }
//            progressBar.setVisibility(View.VISIBLE);
//            viewModel.login(email, password).observe(getViewLifecycleOwner(), response -> {
//                progressBar.setVisibility(View.GONE);
//                if (response != null) {
//                    if (response.isTwoFactorRequired()) {
//                        Bundle bundle = new Bundle();
//                        bundle.putString("userId", response.getUserId());
//                        Navigation.findNavController(v).navigate(R.id.action_login_to_twoFactor, bundle);
//                    } else {
//                        Navigation.findNavController(v).navigate(R.id.action_login_to_animalList);
//                    }
//                } else {
//                    Toast.makeText(requireContext(), "Login failed", Toast.LENGTH_SHORT).show();
//                }
//            });
//        });

        return view;
    }
}