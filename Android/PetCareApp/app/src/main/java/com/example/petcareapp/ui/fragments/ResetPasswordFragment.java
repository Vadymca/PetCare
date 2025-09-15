package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.ProgressBar;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.fragment.NavHostFragment;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.ui.viewmodels.AuthViewModel;
import com.google.android.material.button.MaterialButton;
import com.google.android.material.snackbar.Snackbar;
import com.google.android.material.textfield.TextInputEditText;
import com.google.android.material.textfield.TextInputLayout;

import javax.inject.Inject;

public class ResetPasswordFragment extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private AuthViewModel viewModel;

    private TextInputLayout layoutEmail;
    private TextInputEditText editTextEmail;
    private MaterialButton buttonReset;
    private ProgressBar progressBar;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
        viewModel = new ViewModelProvider(this, viewModelFactory).get(AuthViewModel.class);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_reset_password, container, false);

        layoutEmail = view.findViewById(R.id.layout_email_reset);
        editTextEmail = view.findViewById(R.id.edit_text_email_reset);
        buttonReset = view.findViewById(R.id.button_reset_password);
        progressBar = view.findViewById(R.id.progress_bar_reset);
        ImageButton back = view.findViewById(R.id.button_back_reset);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }

        buttonReset.setOnClickListener(v -> {
            String email = editTextEmail.getText().toString().trim();

            if (TextUtils.isEmpty(email)) {
                layoutEmail.setError("Введіть email або номер телефону");
                return;
            } else {
                layoutEmail.setError(null);
            }

            progressBar.setVisibility(View.VISIBLE);

            viewModel.resetPassword(email).observe(getViewLifecycleOwner(), success -> {
                progressBar.setVisibility(View.GONE);
                if (success != null && success) {
                    Snackbar.make(view, "Інструкція для відновлення пароля надіслана", Snackbar.LENGTH_LONG).show();
                } else {
                    layoutEmail.setError("Користувача не знайдено");
                }
            });
        });

        return view;
    }
}