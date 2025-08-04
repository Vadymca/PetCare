package com.example.petcareapp.data.api;

import retrofit2.http.Query;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Shelter;
import java.util.List;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

public interface ApiService {
    @GET("animals")
    Call<List<Animal>> getAnimals(int page, int size);

    @GET("animals/{slug}")
    Call<Animal> getAnimalBySlug(@Path("slug") String slug);

    @GET("shelters")
    Call<List<Shelter>> getShelters(@Query("page") int page, @Query("size") int size);

    @GET("shelters/{slug}")
    Call<Shelter> getShelterBySlug(@Path("slug") String slug);
}