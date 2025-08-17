package com.example.petcareapp.ui.fragments;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.databinding.FragmentAnimalDetailBinding;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.di.AppModule;
import com.example.petcareapp.di.DaggerAppComponent;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;

import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;

import javax.inject.Inject;

public class AnimalDetailFragment extends Fragment {
    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private AnimalViewModel viewModel;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ((PetCareApplication) requireActivity().getApplication()).getAppComponent().inject(this);
        viewModel = new ViewModelProvider(this, viewModelFactory).get(AnimalViewModel.class);
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_animal_detail, container, false);
        TextView nameTextView = view.findViewById(R.id.text_view_name);
        TextView genderTextView = view.findViewById(R.id.text_view_gender);
        TextView birthdayTextView = view.findViewById(R.id.text_view_birthday);
        TextView descriptionTextView = view.findViewById(R.id.text_view_description);
        TextView statusTextView = view.findViewById(R.id.text_view_status);
        ImageView photoImageView = view.findViewById(R.id.image_view_photo);

        String slug = requireArguments().getString("slug");
        viewModel.getAnimalBySlug(slug).observe(getViewLifecycleOwner(), animal -> {
            if (animal != null) {
                nameTextView.setText(animal.getName());
                genderTextView.setText(animal.getGender());
                birthdayTextView.setText(animal.getBirthday());
                descriptionTextView.setText(animal.getDescription());
                statusTextView.setText(animal.getStatus());
                if (animal.getPhotos() != null && !animal.getPhotos().isEmpty()) {
                    Glide.with(requireContext())
                            .load(animal.getPhotos().get(0))
                            .into(photoImageView);
                }
            }
        });

        return view;
    }
}