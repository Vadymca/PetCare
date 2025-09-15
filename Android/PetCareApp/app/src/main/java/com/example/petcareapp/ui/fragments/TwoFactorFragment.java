package com.example.petcareapp.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.Navigation;
import androidx.navigation.fragment.NavHostFragment;

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

        ImageButton back = view.findViewById(R.id.button_back_registration2);
        if (back != null) {
            back.setOnClickListener(v ->
                    NavHostFragment.findNavController(this).navigateUp()
            );
        }

        EditText pin1 = view.findViewById(R.id.edit_text_pin_1);
        EditText pin2 = view.findViewById(R.id.edit_text_pin_2);
        EditText pin3 = view.findViewById(R.id.edit_text_pin_3);
        EditText pin4 = view.findViewById(R.id.edit_text_pin_4);

        buttonVerify = view.findViewById(R.id.button_verify);
        progressBar = view.findViewById(R.id.progress_bar);

        String userId = requireArguments().getString("userId");

        // Автопереход между полями
        setupPinAutoMove(pin1, pin2);
        setupPinAutoMove(pin2, pin3);
        setupPinAutoMove(pin3, pin4);

        buttonVerify.setOnClickListener(v -> {
            String code = pin1.getText().toString().trim() +
                    pin2.getText().toString().trim() +
                    pin3.getText().toString().trim() +
                    pin4.getText().toString().trim();

            if (code.isEmpty() || code.length() < 4) {
                Toast.makeText(requireContext(), "Введіть PIN-код", Toast.LENGTH_SHORT).show();
                return;
            }

            progressBar.setVisibility(View.VISIBLE);

            v.postDelayed(() -> {
                progressBar.setVisibility(View.GONE);

                if (code.equals("0000")) {
                    Navigation.findNavController(v).navigate(R.id.action_twoFactor_to_createPassword);
                } else {
                    Toast.makeText(requireContext(), "Невірний PIN-код", Toast.LENGTH_SHORT).show();
                }
            }, 1000);
        });

        return view;
    }

    private void setupPinAutoMove(EditText current, EditText next) {
        current.addTextChangedListener(new android.text.TextWatcher() {
            @Override public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
            @Override public void onTextChanged(CharSequence s, int start, int before, int count) {
                if (s.length() == 1) next.requestFocus();
            }
            @Override public void afterTextChanged(android.text.Editable s) {}
        });
    }
}