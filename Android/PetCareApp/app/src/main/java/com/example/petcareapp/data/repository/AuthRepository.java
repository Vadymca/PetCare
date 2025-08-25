package com.example.petcareapp.data.repository;

import android.content.SharedPreferences;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;

import com.example.petcareapp.data.api.ApiService;
import com.example.petcareapp.data.models.LoginRequest;
import com.example.petcareapp.data.models.LoginResponse;
import com.example.petcareapp.data.models.TwoFactorRequest;
import com.example.petcareapp.data.models.UpdateProfileRequest;
import com.example.petcareapp.data.models.UserProfile;

import javax.inject.Inject;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AuthRepository {
    private final ApiService apiService;
    private final SharedPreferences prefs;

    @Inject
    public AuthRepository(ApiService apiService, SharedPreferences prefs) {
        this.apiService = apiService;
        this.prefs = prefs;
    }

    public LiveData<LoginResponse> login(String email, String password) {
        MutableLiveData<LoginResponse> result = new MutableLiveData<>();
        LoginRequest request = new LoginRequest(email, password);
        apiService.login(request).enqueue(new Callback<LoginResponse>() {
            @Override
            public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    // Зберігаємо userId у SharedPreferences
                    prefs.edit().putString("userId", response.body().getUserId()).apply();
                    result.setValue(response.body());
                } else {
                    result.setValue(null);
                }
            }

            @Override
            public void onFailure(Call<LoginResponse> call, Throwable t) {
                result.setValue(null);
            }
        });
        return result;
    }

    public LiveData<Void> verifyTwoFactor(String userId, String code) {
        MutableLiveData<Void> result = new MutableLiveData<>();
        TwoFactorRequest request = new TwoFactorRequest(userId, code);
        apiService.verifyTwoFactor(request).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    result.setValue(null);
                } else {
                    result.setValue(null);
                }
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                result.setValue(null);
            }
        });
        return result;
    }

    public LiveData<Void> logout() {
        MutableLiveData<Void> result = new MutableLiveData<>();
        apiService.logout().enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    prefs.edit().remove("userId").apply();
                    result.setValue(null);
                } else {
                    result.setValue(null);
                }
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                result.setValue(null);
            }
        });
        return result;
    }

    public LiveData<UserProfile> getUserProfile(String userId) {
        MutableLiveData<UserProfile> result = new MutableLiveData<>();
        apiService.getUserProfile(userId).enqueue(new Callback<UserProfile>() {
            @Override
            public void onResponse(Call<UserProfile> call, Response<UserProfile> response) {
                if (response.isSuccessful() && response.body() != null) {
                    result.setValue(response.body());
                } else {
                    result.setValue(null);
                }
            }

            @Override
            public void onFailure(Call<UserProfile> call, Throwable t) {
                result.setValue(null);
            }
        });
        return result;
    }

    public LiveData<Void> updateUserProfile(String userId, UpdateProfileRequest request) {
        MutableLiveData<Void> result = new MutableLiveData<>();
        apiService.updateUserProfile(userId, request).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    result.setValue(null);
                } else {
                    result.setValue(null);
                }
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                result.setValue(null);
            }
        });
        return result;
    }

    public String getUserId() {
        return prefs.getString("userId", null);
    }
}