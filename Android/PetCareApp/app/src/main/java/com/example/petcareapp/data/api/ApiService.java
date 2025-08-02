package com.example.petcareapp.data.api;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Shelter;
import java.util.List;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

public interface ApiService {
    @GET("animals")
    Call<List<Animal>> getAnimals();

    @GET("shelters")
    Call<List<Shelter>> getShelters();

    @GET("animals/{slug}")
    Call<Animal> getAnimalBySlug(@Path("slug") String slug);
}