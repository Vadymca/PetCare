package com.example.petcareapp.data.room.entities;
import androidx.annotation.NonNull;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

@Entity(tableName = "breeds")
public class BreedEntity {
    /// Attributes
    @PrimaryKey @NonNull
    private String id;

    private String speciesId;
    private String name;
    private String description;
    /// Constructor
    public BreedEntity(String id, String speciesId, String name, String description) {
        this.id = id;
        this.speciesId = speciesId;
        this.name = name;
        this.description = description;
    }

    /// Getters and Setters
    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getSpeciesId() {
        return speciesId;
    }

    public void setSpeciesId(String speciesId) {
        this.speciesId = speciesId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}