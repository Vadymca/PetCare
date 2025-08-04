package com.example.petcareapp.data.models;

import com.google.gson.annotations.SerializedName;
import java.util.List;

public class Shelter {
    @SerializedName("id") public String id;
    @SerializedName("slug") public String slug;
    @SerializedName("name") public String name;
    @SerializedName("address") public String address;
    @SerializedName("coordinAAates") public String coordinates;
    @SerializedName("photos") public List<String> photos;

    public Shelter(String id, String slug, String name, String address, String coordinates, List<String> photos) {
        this.id = id;
        this.slug = slug;
        this.name = name;
        this.address = address;
        this.coordinates = coordinates;
        this.photos = photos;
    }
}