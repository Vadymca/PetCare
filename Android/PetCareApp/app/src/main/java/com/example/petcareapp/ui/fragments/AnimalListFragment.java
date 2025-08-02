package com.example.petcareapp.ui.fragments;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.databinding.FragmentAnimalListBinding;
import com.example.petcareapp.di.AppComponent;
import com.example.petcareapp.di.AppModule;
import com.example.petcareapp.di.DaggerAppComponent;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;

import java.util.ArrayList;
import java.util.List;

public class AnimalListFragment extends Fragment {
    private FragmentAnimalListBinding binding;
    private AnimalViewModel viewModel;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        binding = FragmentAnimalListBinding.inflate(inflater, container, false);
        View view = binding.getRoot();

        viewModel = new ViewModelProvider(this, new ViewModelProvider.Factory() {
            @Override
            public <T extends ViewModel> T create(Class<T> modelClass) {
                AppComponent appComponent = DaggerAppComponent.builder()
                        .appModule(new AppModule(requireActivity().getApplication()))
                        .build();
                return (T) appComponent.getAnimalViewModel(); // Додай інжекцію
            }
        }).get(AnimalViewModel.class);

        binding.recyclerViewAnimals.setLayoutManager(new LinearLayoutManager(getContext()));

        // Приклад адаптера (додай пізніше)
        binding.recyclerViewAnimals.setAdapter(new AnimalAdapter(animal -> {
            Bundle args = new Bundle();
            args.putString("animalSlug", animal.slug);
            Navigation.findNavController(view).navigate(R.id.action_animalListFragment_to_animalDetailFragment, args);
        }));

        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

    // Проста реалізація адаптера (додай у ui.adapters)
    private static class AnimalAdapter extends RecyclerView.Adapter<AnimalAdapter.ViewHolder> {
        private final List<Animal> animals;
        private final OnItemClickListener listener;

        AnimalAdapter(OnItemClickListener listener) {
            this.animals = new ArrayList<>();
            this.listener = listener;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(android.R.layout.simple_list_item_1, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            com.example.petcareapp.data.models.Animal animal = animals.get(position);
            holder.itemView.setOnClickListener(v -> listener.onItemClick(animal));
        }

        @Override
        public int getItemCount() {
            return animals.size();
        }

        static class ViewHolder extends RecyclerView.ViewHolder {
            ViewHolder(View itemView) {
                super(itemView);
            }
        }

        interface OnItemClickListener {
            void onItemClick(com.example.petcareapp.data.models.Animal animal);
        }
    }
}