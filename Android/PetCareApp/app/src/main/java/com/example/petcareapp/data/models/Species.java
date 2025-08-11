package com.example.petcareapp.data.models;
import com.google.gson.annotations.SerializedName;

public class Species {
    @SerializedName("id")
    private String id;
    @SerializedName("name")
    private String name;

    /// Getters and Setters
    public String getId() {
        return id;
    }
    public void setId(String id) {
        this.id = id;
    }
    public String getName() {
        return name;
    }
    public void setName(String name) {
        this.name = name;
    }
}