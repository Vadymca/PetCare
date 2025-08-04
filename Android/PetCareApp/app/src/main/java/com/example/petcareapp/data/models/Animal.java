package com.example.petcareapp.data.models;

import com.google.gson.annotations.SerializedName;
import java.util.List;

public class Animal {
    @SerializedName("id") public String id;
    @SerializedName("slug") public String slug;
    @SerializedName("name") public String name;
    @SerializedName("breedId") public String breedId;
    @SerializedName("status") public String status;
    @SerializedName("photos") public List<String> photos;
    @SerializedName("shelterId") public String shelterId;

    public Animal(String id, String slug, String name, String breedId, String status, List<String> photos, String shelterId) {
        this.id = id;
        this.slug = slug;
        this.name = name;
        this.breedId = breedId;
        this.status = status;
        this.photos = photos;
        this.shelterId = shelterId;
    }
}