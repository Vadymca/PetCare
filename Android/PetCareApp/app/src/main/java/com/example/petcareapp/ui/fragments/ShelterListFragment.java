package com.example.petcareapp.ui.fragments;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.paging.PagingData;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.databinding.FragmentShelterListBinding;
import com.example.petcareapp.ui.adapters.ShelterAdapter;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;
import com.example.petcareapp.ui.viewmodels.ShelterListViewModel;

import java.util.ArrayList;

import javax.inject.Inject;

public class ShelterListFragment extends Fragment {
    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private AnimalViewModel viewModel;
    private ShelterAdapter adapter;
    private RecyclerView recyclerView;
    private int currentPage = 1;
    private final int pageSize = 10;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ((PetCareApplication) requireActivity().getApplication()).getAppComponent().inject(this);
        viewModel = new ViewModelProvider(this, viewModelFactory).get(AnimalViewModel.class);
        adapter = new ShelterAdapter(new ArrayList<>());
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_shelter_list, container, false);
        recyclerView = view.findViewById(R.id.recycler_view_shelters);
        recyclerView.setLayoutManager(new LinearLayoutManager(requireContext()));
        recyclerView.setAdapter(adapter);

        recyclerView.addOnScrollListener(new RecyclerView.OnScrollListener() {
            @Override
            public void onScrolled(@NonNull RecyclerView recyclerView, int dx, int dy) {
                super.onScrolled(recyclerView, dx, dy);
                LinearLayoutManager layoutManager = (LinearLayoutManager) recyclerView.getLayoutManager();
                if (layoutManager != null && layoutManager.findLastCompletelyVisibleItemPosition() == adapter.getItemCount() - 1) {
                    loadMoreShelters();
                }
            }
        });

        viewModel.getShelters(currentPage, pageSize).observe(getViewLifecycleOwner(), shelters -> {
            if (shelters != null) {
                adapter.updateShelters(shelters);
            }
        });

        return view;
    }

    private void loadMoreShelters() {
        currentPage++;
        viewModel.getShelters(currentPage, pageSize).observe(getViewLifecycleOwner(), shelters -> {
            if (shelters != null) {
                adapter.addShelters(shelters);
            }
        });
    }
}