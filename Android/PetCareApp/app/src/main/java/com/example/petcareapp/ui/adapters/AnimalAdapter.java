package com.example.petcareapp.ui.adapters;

import android.annotation.SuppressLint;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;

import androidx.paging.PagingDataAdapter;
import androidx.recyclerview.widget.DiffUtil;
import androidx.recyclerview.widget.RecyclerView;
import com.bumptech.glide.Glide;
import com.example.petcareapp.R;
import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.databinding.ItemAnimalBinding;

import java.util.List;

//public class AnimalAdapter extends RecyclerView.Adapter<AnimalAdapter.AnimalViewHolder> {
//    private List<Animal> animals;
//
//    public AnimalAdapter(List<Animal> animals) {
//        this.animals = animals;
//    }
//
//    @SuppressLint("NotifyDataSetChanged")
//    public void updateAnimals(List<Animal> newAnimals) {
//        animals.clear();
//        animals.addAll(newAnimals);
//        notifyDataSetChanged();
//    }
//
//    public void addAnimals(List<Animal> newAnimals) {
//        int startPosition = animals.size();
//        animals.addAll(newAnimals);
//        notifyItemRangeInserted(startPosition, newAnimals.size());
//    }
//
//    @NonNull
//    @Override
//    public AnimalViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
//        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_animal, parent, false);
//        return new AnimalViewHolder(view);
//    }
//
//    @Override
//    public void onBindViewHolder(@NonNull AnimalViewHolder holder, int position) {
//        Animal animal = animals.get(position);
//        holder.nameTextView.setText(animal.getName());
//        holder.statusTextView.setText(animal.getStatus());
//        if (animal.getPhotos() != null && !animal.getPhotos().isEmpty()) {
//            Glide.with(holder.itemView.getContext())
//                    .load(animal.getPhotos().get(0))
//                    .into(holder.photoImageView);
//        }
//    }
//
//    @Override
//    public int getItemCount() {
//        return animals.size();
//    }
//
//    static class AnimalViewHolder extends RecyclerView.ViewHolder {
//        TextView nameTextView;
//        TextView statusTextView;
//        ImageView photoImageView;
//
//        AnimalViewHolder(@NonNull View itemView) {
//            super(itemView);
//            nameTextView = itemView.findViewById(R.id.text_view_name);
//            statusTextView = itemView.findViewById(R.id.text_view_status);
//            photoImageView = itemView.findViewById(R.id.image_view_photo);
//        }
//    }
//}

public class AnimalAdapter extends RecyclerView.Adapter<AnimalAdapter.AnimalViewHolder> {
    private List<Animal> animals;

    public AnimalAdapter(List<Animal> animals) {
        this.animals = animals;
    }

    @SuppressLint("NotifyDataSetChanged")
    public void updateAnimals(List<Animal> newAnimals) {
        animals.clear();
        animals.addAll(newAnimals);
        notifyDataSetChanged();
    }

    public void addAnimals(List<Animal> newAnimals) {
        int startPosition = animals.size();
        animals.addAll(newAnimals);
        notifyItemRangeInserted(startPosition, newAnimals.size());
    }

    @NonNull
    @Override
    public AnimalViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_animal, parent, false);
        return new AnimalViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull AnimalViewHolder holder, int position) {
        Animal animal = animals.get(position);
        holder.nameTextView.setText(animal.getName());
        holder.statusTextView.setText(animal.getStatus());

        // Обробка фото з placeholder/error
        String photoUrl = (animal.getPhotos() != null && !animal.getPhotos().isEmpty()) ? animal.getPhotos().get(0) : null;
        Glide.with(holder.itemView.getContext())
                .load(photoUrl)
                .placeholder(R.drawable.placeholder_animal)  // Базове зображення, якщо фото немає або завантажується
                .error(R.drawable.error_image)  // Якщо помилка завантаження
                .into(holder.photoImageView);
    }

    @Override
    public int getItemCount() {
        return animals.size();
    }

    static class AnimalViewHolder extends RecyclerView.ViewHolder {
        TextView nameTextView;
        TextView statusTextView;
        ImageView photoImageView;

        AnimalViewHolder(@NonNull View itemView) {
            super(itemView);
            nameTextView = itemView.findViewById(R.id.text_view_name);
            statusTextView = itemView.findViewById(R.id.text_view_status);
            photoImageView = itemView.findViewById(R.id.image_view_photo);
        }
    }
}