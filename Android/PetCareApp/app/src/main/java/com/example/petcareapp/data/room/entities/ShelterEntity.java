package com.example.petcareapp.data.room.entities;

import androidx.annotation.NonNull;
import androidx.room.Entity;
import androidx.room.Ignore;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;
import com.example.petcareapp.data.room.converters.JsonConverter;
import java.util.List;

@Entity(tableName = "shelters")
public class ShelterEntity {
    @PrimaryKey
    @NonNull
    public String id;

    public String slug;
    public String name;
    public String address;
    public String coordinates;
    @TypeConverters(JsonConverter.class)
    public List<String> photos;

    // Конструктор без аргументів
    public ShelterEntity() {}

    // Конструктор із усіма полями
    @Ignore
    public ShelterEntity(@NonNull String id, String slug, String name, String address, String coordinates, List<String> photos) {
        this.id = id;
        this.slug = slug;
        this.name = name;
        this.address = address;
        this.coordinates = coordinates;
        this.photos = photos;
    }

    @NonNull
    public String getId() {
        return id;
    }

    public void setId(@NonNull String id) {
        this.id = id;
    }

    public String getSlug() {
        return slug;
    }

    public void setSlug(String slug) {
        this.slug = slug;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(String coordinates) {
        this.coordinates = coordinates;
    }

    public List<String> getPhotos() {
        return photos;
    }

    public void setPhotos(List<String> photos) {
        this.photos = photos;
    }
}