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
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.databinding.ItemShelterBinding;

import java.util.List;

//public class ShelterAdapter extends RecyclerView.Adapter<ShelterAdapter.ShelterViewHolder> {
//    private List<Shelter> shelters;
//
//    public ShelterAdapter(List<Shelter> shelters) {
//        this.shelters = shelters;
//    }
//
//    @SuppressLint("NotifyDataSetChanged")
//    public void updateShelters(List<Shelter> newShelters) {
//        shelters.clear();
//        shelters.addAll(newShelters);
//        notifyDataSetChanged();
//    }
//
//    public void addShelters(List<Shelter> newShelters) {
//        int startPosition = shelters.size();
//        shelters.addAll(newShelters);
//        notifyItemRangeInserted(startPosition, newShelters.size());
//    }
//
//    @NonNull
//    @Override
//    public ShelterViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
//        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_shelter, parent, false);
//        return new ShelterViewHolder(view);
//    }
//
//    @Override
//    public void onBindViewHolder(@NonNull ShelterViewHolder holder, int position) {
//        Shelter shelter = shelters.get(position);
//        holder.nameTextView.setText(shelter.getName());
//        holder.addressTextView.setText(shelter.getAddress());
//        if (shelter.getPhotos() != null && !shelter.getPhotos().isEmpty()) {
//            Glide.with(holder.itemView.getContext())
//                    .load(shelter.getPhotos().get(0))
//                    .into(holder.photoImageView);
//        }
//    }
//
//    @Override
//    public int getItemCount() {
//        return shelters.size();
//    }
//
//    static class ShelterViewHolder extends RecyclerView.ViewHolder {
//        TextView nameTextView;
//        TextView addressTextView;
//        ImageView photoImageView;
//
//        ShelterViewHolder(@NonNull View itemView) {
//            super(itemView);
//            nameTextView = itemView.findViewById(R.id.text_view_name);
//            addressTextView = itemView.findViewById(R.id.text_view_address);
//            photoImageView = itemView.findViewById(R.id.image_view_photo);
//        }
//    }
//}

public class ShelterAdapter extends RecyclerView.Adapter<ShelterAdapter.ShelterViewHolder> {
    private List<Shelter> shelters;

    public ShelterAdapter(List<Shelter> shelters) {
        this.shelters = shelters;
    }

    @SuppressLint("NotifyDataSetChanged")
    public void updateShelters(List<Shelter> newShelters) {
        shelters.clear();
        shelters.addAll(newShelters);
        notifyDataSetChanged();
    }

    public void addShelters(List<Shelter> newShelters) {
        int startPosition = shelters.size();
        shelters.addAll(newShelters);
        notifyItemRangeInserted(startPosition, newShelters.size());
    }

    @NonNull
    @Override
    public ShelterViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_shelter, parent, false);
        return new ShelterViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ShelterViewHolder holder, int position) {
        Shelter shelter = shelters.get(position);
        holder.nameTextView.setText(shelter.getName());
        holder.addressTextView.setText(shelter.getAddress());

        // Обробка фото з placeholder/error
        String photoUrl = (shelter.getPhotos() != null && !shelter.getPhotos().isEmpty()) ? shelter.getPhotos().get(0) : null;
        Glide.with(holder.itemView.getContext())
                .load(photoUrl)
                .placeholder(R.drawable.placeholder_shelter)  // Базове зображення для притулку
                .error(R.drawable.error_image)
                .into(holder.photoImageView);
    }

    @Override
    public int getItemCount() {
        return shelters.size();
    }

    static class ShelterViewHolder extends RecyclerView.ViewHolder {
        TextView nameTextView;
        TextView addressTextView;
        ImageView photoImageView;

        ShelterViewHolder(@NonNull View itemView) {
            super(itemView);
            nameTextView = itemView.findViewById(R.id.text_view_name);
            addressTextView = itemView.findViewById(R.id.text_view_address);
            photoImageView = itemView.findViewById(R.id.image_view_photo);
        }
    }
}