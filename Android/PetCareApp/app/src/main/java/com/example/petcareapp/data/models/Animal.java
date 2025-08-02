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
}