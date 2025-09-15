package com.example.petcareapp.ui.fragments;


import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.databinding.FragmentHelpBinding;
import com.example.petcareapp.di.AppComponent;

import javax.inject.Inject;

public class HelpFragment extends Fragment {

    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private FragmentHelpBinding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        AppComponent component = ((PetCareApplication) requireActivity().getApplication()).getAppComponent();
        component.inject(this);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentHelpBinding.inflate(inflater, container, false);
        View view = binding.getRoot();

        // Add help content here (text, links, etc.)

        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}