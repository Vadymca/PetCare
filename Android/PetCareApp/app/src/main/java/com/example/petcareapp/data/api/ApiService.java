package com.example.petcareapp.data.api;

import retrofit2.http.Body;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Query;

import com.example.petcareapp.data.models.Animal;
import com.example.petcareapp.data.models.Breed;
import com.example.petcareapp.data.models.LoginRequest;
import com.example.petcareapp.data.models.LoginResponse;
import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.models.Species;
import com.example.petcareapp.data.models.TwoFactorRequest;
import com.example.petcareapp.data.models.UpdateProfileRequest;
import com.example.petcareapp.data.models.User;
import com.example.petcareapp.data.models.UserProfile;

import java.util.List;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
public interface ApiService {
    /// методи для роботи з API json-server
    @GET("animals")
    Call<List<Animal>> getAnimals(@Query("_page") int page, @Query("_limit") int limit);
    @GET("animals/{slug}")
    Call<Animal> getAnimalBySlug(@Path("slug") String slug);
    @GET("shelters")
    Call<List<Shelter>> getShelters(@Query("_page") int page, @Query("_limit") int limit);
    @GET("shelters/{slug}")
    Call<Shelter> getShelterBySlug(@Path("slug") String slug);
    @GET("species")
    Call<List<Species>> getSpecies();
    @GET("breeds")
    Call<List<Breed>> getBreeds();
    @GET("users/{id}")
    Call<User> getUserById(@Path("id") String id);

    // Нові методи для .NET-бекенду
    @POST("auth/login")
    Call<LoginResponse> login(@Body LoginRequest request);

    @POST("auth/2fa")
    Call<Void> verifyTwoFactor(@Body TwoFactorRequest request);

    @POST("auth/logout")
    Call<Void> logout();

    @GET("users/{id}/profile")
    Call<UserProfile> getUserProfile(@Path("id") String userId);

    @PUT("users/{id}/profile")
    Call<Void> updateUserProfile(@Path("id") String userId, @Body UpdateProfileRequest request);

    @POST("auth/refresh")
    Call<Void> refreshToken();
}