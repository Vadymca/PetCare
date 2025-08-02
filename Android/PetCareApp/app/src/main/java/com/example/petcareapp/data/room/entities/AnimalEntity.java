package com.example.petcareapp.data.room.entities;

import androidx.annotation.NonNull;
import androidx.room.Entity;
import androidx.room.Ignore;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;
import com.example.petcareapp.data.room.converters.JsonConverter;
import java.util.List;

@Entity(tableName = "animals")
public class AnimalEntity {
    @PrimaryKey
    @NonNull
    public String id;

    public String slug;
    public String name;
    public String breedId;
    public String status;
    @TypeConverters(JsonConverter.class)
    public List<String> photos;

    // Конструктор без аргументів (використовується Room)
    public AnimalEntity() {}

    // Конструктор із аргументами (позначимо як @Ignore)
    @Ignore
    public AnimalEntity(@NonNull String id, String slug, String name, String breedId, String status, List<String> photos) {
        this.id = id;
        this.slug = slug;
        this.name = name;
        this.breedId = breedId;
        this.status = status;
        this.photos = photos;
    }

    @NonNull
    public String getId() { return id; }
    public void setId(@NonNull String id) { this.id = id; }
    public String getSlug() { return slug; }
    public void setSlug(String slug) { this.slug = slug; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getBreedId() { return breedId; }
    public void setBreedId(String breedId) { this.breedId = breedId; }
    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }
    public List<String> getPhotos() { return photos; }
    public void setPhotos(List<String> photos) { this.photos = photos; }
}