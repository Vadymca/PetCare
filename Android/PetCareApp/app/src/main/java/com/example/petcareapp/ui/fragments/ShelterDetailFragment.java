package com.example.petcareapp.ui.fragments;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.databinding.FragmentShelterDetailBinding;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;
import com.example.petcareapp.ui.viewmodels.ShelterDetailViewModel;
import javax.inject.Inject;

public class ShelterDetailFragment extends Fragment {
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
        View view = inflater.inflate(R.layout.fragment_shelter_detail, container, false);
        TextView nameTextView = view.findViewById(R.id.text_view_name);
        TextView addressTextView = view.findViewById(R.id.text_view_address);
        TextView contactPhoneTextView = view.findViewById(R.id.text_view_contact_phone);
        TextView workingHoursTextView = view.findViewById(R.id.text_view_working_hours);
        ImageView photoImageView = view.findViewById(R.id.image_view_photo);

        String slug = requireArguments().getString("slug");
        viewModel.getShelterBySlug(slug).observe(getViewLifecycleOwner(), shelter -> {
            if (shelter != null) {
                nameTextView.setText(shelter.getName());
                addressTextView.setText(shelter.getAddress());
                contactPhoneTextView.setText(shelter.getContactPhone());
                workingHoursTextView.setText(shelter.getWorkingHours());
                if (shelter.getPhotos() != null && !shelter.getPhotos().isEmpty()) {
                    Glide.with(requireContext())
                            .load(shelter.getPhotos().get(0))
                            .into(photoImageView);
                }
            }
        });

        return view;
    }
}