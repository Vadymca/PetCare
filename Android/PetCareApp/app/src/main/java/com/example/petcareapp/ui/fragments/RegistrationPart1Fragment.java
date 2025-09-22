package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.text.SpannableString;
import android.text.Spanned;
import android.text.method.LinkMovementMethod;
import android.text.style.ClickableSpan;
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
import com.example.petcareapp.databinding.FragmentRegistrationPart1Binding;
import com.example.petcareapp.di.AppComponent;

import javax.inject.Inject;

public class RegistrationPart1Fragment extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private FragmentRegistrationPart1Binding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentRegistrationPart1Binding.inflate(inflater, container, false);
        View view = binding.getRoot();

        ImageButton back = view.findViewById(R.id.button_back_registration);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }

        String helpText = "Потрібна допомога?";
        SpannableString spannable = new SpannableString(helpText);
        ClickableSpan clickableSpan = new ClickableSpan() {
            @Override
            public void onClick(@NonNull View widget) {
                NavController navController = Navigation.findNavController(widget);
                navController.navigate(R.id.action_part1_to_help);
            }
        };
        int start = helpText.indexOf("Потрібна допомога?");
        int end = start + "Потрібна допомога?".length();
        spannable.setSpan(clickableSpan, start, end, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        binding.textHelp.setText(spannable);
        binding.textHelp.setMovementMethod(LinkMovementMethod.getInstance());

        binding.buttonContinue.setOnClickListener(v -> {
            boolean valid = true;

            String name = binding.editName.getText().toString().trim();
            String surname = binding.editSurname.getText().toString().trim();
            String nickname = binding.editNickname.getText().toString().trim();

            // Имя
            if (name.isEmpty()) {
                binding.layoutName.setError("Поле не може бути порожнім");
                valid = false;
            } else {
                binding.layoutName.setError(null);
            }

            // Прізвище
            if (!surname.matches("^[A-Za-zА-Яа-яІіЇїЄєҐґ]+$")) {
                binding.layoutSurname.setError("Введіть прізвище без цифр або символів.");
                valid = false;
            }
            else if (surname.isEmpty()) {
                binding.layoutSurname.setError("Поле не може бути порожнім");
                valid = false;
            } else {
                binding.layoutSurname.setError(null);
            }
            // Нікнейм
            if (!nickname.matches("^[A-Za-zА-Яа-яІіЇїЄєҐґ0-9]+$")) {
                binding.layoutNickname.setError("Нікнейм має містити тільки букви або цифри (без пробілів).");
                valid = false;
            }
            else if (nickname.isEmpty()) {
                binding.layoutNickname.setError("Поле не може бути порожнім");
                valid = false;
            } else {
                binding.layoutNickname.setError(null);
            }

            if (valid) {
                NavController navController = Navigation.findNavController(v);
                navController.navigate(R.id.action_part1_to_part2);
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