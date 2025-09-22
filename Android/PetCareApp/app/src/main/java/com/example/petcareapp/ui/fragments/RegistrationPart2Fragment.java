package com.example.petcareapp.ui.fragments;


import android.os.Bundle;
import android.text.Editable;
import android.text.SpannableString;
import android.text.Spanned;
import android.text.TextWatcher;
import android.text.method.LinkMovementMethod;
import android.text.style.ClickableSpan;
import android.util.Patterns;
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
import com.example.petcareapp.databinding.FragmentRegistrationPart2Binding;
import com.example.petcareapp.di.AppComponent;
import com.google.android.material.textfield.TextInputLayout;

import javax.inject.Inject;

public class RegistrationPart2Fragment extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private FragmentRegistrationPart2Binding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
    }


    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentRegistrationPart2Binding.inflate(inflater, container, false);
        View view = binding.getRoot();
        ImageButton back = view.findViewById(R.id.button_back_registration_part1);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }
        // Ссылка "Потрібна допомога?"
        String helpText = "Потрібна допомога?";
        SpannableString spannable = new SpannableString(helpText);
        ClickableSpan clickableSpan = new ClickableSpan() {
            @Override
            public void onClick(@NonNull View widget) {
                NavController navController = Navigation.findNavController(widget);
                navController.navigate(R.id.action_part2_to_help);
            }
        };
        spannable.setSpan(clickableSpan, 0, helpText.length(), Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        binding.textHelp.setText(spannable);
        binding.textHelp.setMovementMethod(LinkMovementMethod.getInstance());

        // Валидация телефона и email
        setupValidation();

        // Кнопка "Продовжити"
        binding.buttonContinue.setOnClickListener(v -> {
            if (validateFields()) {
                NavController navController = Navigation.findNavController(v);
                navController.navigate(R.id.action_part2_to_twoFactor);
            }
        });

        return view;
    }

    private void setupValidation() {
        binding.editPhone.addTextChangedListener(new TextWatcher() {
            @Override public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
            @Override public void onTextChanged(CharSequence s, int start, int before, int count) {}
            @Override
            public void afterTextChanged(Editable s) {
                validatePhone();
                toggleContinueButton();
            }
        });

        binding.editEmail.addTextChangedListener(new TextWatcher() {
            @Override public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
            @Override public void onTextChanged(CharSequence s, int start, int before, int count) {}
            @Override
            public void afterTextChanged(Editable s) {
                validateEmail();
                toggleContinueButton();
            }
        });
    }

    private boolean validatePhone() {
        String phone = binding.editPhone.getText() != null ? binding.editPhone.getText().toString().trim() : "";
        TextInputLayout phoneLayout = binding.layoutPhone;

        if (!phone.startsWith("+380") || phone.length() < 13) {
            phoneLayout.setError("Введіть номер у форматі +380...");
            return false;
        } else {
            phoneLayout.setError(null);
            return true;
        }
    }

    private boolean validateEmail() {
        String email = binding.editEmail.getText() != null ? binding.editEmail.getText().toString().trim() : "";
        TextInputLayout emailLayout = binding.layoutEmail;

        if (!Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            emailLayout.setError("Введіть коректну адресу, наприклад: name@email.com");
            return false;
        } else {
            emailLayout.setError(null);
            return true;
        }
    }

    private boolean validateFields() {
        boolean validPhone = validatePhone();
        boolean validEmail = validateEmail();
        return validPhone && validEmail;
    }

    private void toggleContinueButton() {
        boolean enable = validatePhone() && validateEmail();
        binding.buttonContinue.setEnabled(enable);
        binding.buttonContinue.setAlpha(enable ? 1f : 0.5f); // серое состояние как в макете
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}