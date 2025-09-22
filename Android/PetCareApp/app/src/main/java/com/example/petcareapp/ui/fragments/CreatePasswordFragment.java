package com.example.petcareapp.ui.fragments;

import android.annotation.SuppressLint;
import android.graphics.Color;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.navigation.fragment.NavHostFragment;

import com.example.petcareapp.R;

import java.util.regex.Pattern;

public class CreatePasswordFragment extends Fragment {

    private EditText editTextPassword, editTextConfirmPassword;
    private Button buttonCreateAccount;

    private TextView reqLength, reqLetter, reqDigit, reqSpecial;

    @SuppressLint("MissingInflatedId")
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_create_password, container, false);

        ImageButton back = view.findViewById(R.id.button_back_two_factor);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }

        editTextPassword = view.findViewById(R.id.edit_text_password);
        editTextConfirmPassword = view.findViewById(R.id.edit_text_confirm_password);
        buttonCreateAccount = view.findViewById(R.id.button_create_account);

        // требования
        reqLength = view.findViewById(R.id.req_length);
        reqLetter = view.findViewById(R.id.req_letter);
        reqDigit = view.findViewById(R.id.req_digit);
        reqSpecial = view.findViewById(R.id.req_special);

        // слушатель для проверки пароля во время ввода
        editTextPassword.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                validatePassword(s.toString());
            }

            @Override
            public void afterTextChanged(Editable s) {}
        });

        buttonCreateAccount.setOnClickListener(v -> {
            String pass = editTextPassword.getText().toString().trim();
            String confirm = editTextConfirmPassword.getText().toString().trim();

            if (pass.isEmpty() || confirm.isEmpty()) {
                Toast.makeText(requireContext(), "Заповніть усі поля", Toast.LENGTH_SHORT).show();
                return;
            }

            if (!pass.equals(confirm)) {
                Toast.makeText(requireContext(), "Паролі не збігаються", Toast.LENGTH_SHORT).show();
                return;
            }

            if (!isPasswordValid(pass)) {
                Toast.makeText(requireContext(), "Пароль не відповідає вимогам", Toast.LENGTH_SHORT).show();
                return;
            }

            // TODO: вызывать API когда будет бекенд
            Toast.makeText(requireContext(), "Акаунт створено!", Toast.LENGTH_SHORT).show();

            // Навигация на HomeFragment
            NavHostFragment.findNavController(this)
                    .navigate(R.id.homeFragment);
        });

        return view;
    }

    // Подсвечиваем требования
    private void validatePassword(String password) {
        // длина
        if (password.length() >= 9 && password.length() <= 32) {
            reqLength.setTextColor(Color.GREEN);
        } else {
            reqLength.setTextColor(Color.RED);
        }

        // хотя бы одна буква
        if (Pattern.compile("[A-Za-zА-Яа-я]").matcher(password).find()) {
            reqLetter.setTextColor(Color.GREEN);
        } else {
            reqLetter.setTextColor(Color.RED);
        }

        // хотя бы одна цифра
        if (Pattern.compile("[0-9]").matcher(password).find()) {
            reqDigit.setTextColor(Color.GREEN);
        } else {
            reqDigit.setTextColor(Color.RED);
        }

        // хотя бы один спецсимвол
        if (Pattern.compile("[!@#$%^&*()_+=\\-{}\\[\\]:;\"'<>,.?/]").matcher(password).find()) {
            reqSpecial.setTextColor(Color.GREEN);
        } else {
            reqSpecial.setTextColor(Color.RED);
        }
    }

    // Проверяем выполнение всех требований
    private boolean isPasswordValid(String password) {
        boolean length = password.length() >= 9 && password.length() <= 32;
        boolean letter = Pattern.compile("[A-Za-zА-Яа-я]").matcher(password).find();
        boolean digit = Pattern.compile("[0-9]").matcher(password).find();
        boolean special = Pattern.compile("[!@#$%^&*()_+=\\-{}\\[\\]:;\"'<>,.?/]").matcher(password).find();

        return length && letter && digit && special;
    }
}