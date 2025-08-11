package com.example.petcareapp.data.models;
import com.google.gson.annotations.SerializedName;

public class Breed {
    @SerializedName("id")
    private String id;
    @SerializedName("speciesId")
    private String speciesId;
    @SerializedName("name")
    private String name;
    @SerializedName("description")
    private String description;

    //// Getters and Setters
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