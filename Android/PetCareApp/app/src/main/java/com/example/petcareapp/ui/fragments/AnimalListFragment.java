package com.example.petcareapp.ui.fragments;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.databinding.FragmentAnimalListBinding;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.di.AppModule;
import com.example.petcareapp.di.DaggerAppComponent;
import com.example.petcareapp.ui.adapters.AnimalAdapter;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;

import java.util.ArrayList;
import java.util.List;

import javax.inject.Inject;

public class AnimalListFragment extends Fragment {
    @Inject
    ViewModelProvider.Factory viewModelFactory;
    private AnimalViewModel viewModel;
    private AnimalAdapter adapter;
    private RecyclerView recyclerView;
    private ProgressBar progressBar;
    private int currentPage = 1;
    private final int pageSize = 10;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("AnimalListFragment", "onCreate called");
        ((PetCareApplication) requireActivity().getApplication()).getAppComponent().inject(this);
        viewModel = new ViewModelProvider(this, viewModelFactory).get(AnimalViewModel.class);
        adapter = new AnimalAdapter(new ArrayList<>());
        Log.d("AnimalListFragment", "ViewModel and Adapter initialized");
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        Log.d("AnimalListFragment", "onCreateView called");
        View view = inflater.inflate(R.layout.fragment_animal_list, container, false);
        recyclerView = view.findViewById(R.id.recycler_view_animals);
        progressBar = view.findViewById(R.id.progress_bar);
        recyclerView.setLayoutManager(new LinearLayoutManager(requireContext()));
        recyclerView.setAdapter(adapter);
        // Після setAdapter
        TextView emptyView = view.findViewById(R.id.empty_view);
        SwipeRefreshLayout swipeRefresh = view.findViewById(R.id.swipe_refresh_layout);
        swipeRefresh.setOnRefreshListener(() -> {
            currentPage = 1;
            progressBar.setVisibility(View.VISIBLE);
            viewModel.getAnimals(currentPage, pageSize).observe(getViewLifecycleOwner(), animals -> {
                swipeRefresh.setRefreshing(false);
                progressBar.setVisibility(View.GONE);
                if (animals != null && !animals.isEmpty()) {
                    adapter.updateAnimals(animals);
                    emptyView.setVisibility(View.GONE);
                } else {
                    emptyView.setVisibility(View.VISIBLE);
                }
            });
        });

        Log.d("AnimalListFragment", "RecyclerView and ProgressBar initialized");

        recyclerView.addOnScrollListener(new RecyclerView.OnScrollListener() {
            @Override
            public void onScrolled(@NonNull RecyclerView recyclerView, int dx, int dy) {
                super.onScrolled(recyclerView, dx, dy);
                LinearLayoutManager layoutManager = (LinearLayoutManager) recyclerView.getLayoutManager();
                if (layoutManager != null && layoutManager.findLastCompletelyVisibleItemPosition() == adapter.getItemCount() - 1) {
                    loadMoreAnimals();
                }
            }
        });

        progressBar.setVisibility(View.VISIBLE);
        viewModel.getAnimals(currentPage, pageSize).observe(getViewLifecycleOwner(), animals -> {
            progressBar.setVisibility(View.GONE);
            if (animals != null) {
                adapter.updateAnimals(animals);
                Log.d("AnimalListFragment", "Animals loaded: " + animals.size());
            } else {
                Toast.makeText(requireContext(), "Failed to load animals", Toast.LENGTH_SHORT).show();
                Log.e("AnimalListFragment", "Failed to load animals");
            }
        });

        return view;
    }

    private void loadMoreAnimals() {
        currentPage++;
        progressBar.setVisibility(View.VISIBLE);
        viewModel.getAnimals(currentPage, pageSize).observe(getViewLifecycleOwner(), animals -> {
            progressBar.setVisibility(View.GONE);
            if (animals != null) {
                adapter.addAnimals(animals);
                Log.d("AnimalListFragment", "More animals loaded: " + animals.size());
            } else {
                Toast.makeText(requireContext(), "Failed to load more animals", Toast.LENGTH_SHORT).show();
                Log.e("AnimalListFragment", "Failed to load more animals");
            }
        });
    }
}