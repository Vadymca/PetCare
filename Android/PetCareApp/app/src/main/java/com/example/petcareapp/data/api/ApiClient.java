package com.example.petcareapp.data.api;

import com.example.petcareapp.di.AppModule;
import javax.inject.Inject;
import retrofit2.Retrofit;

public class ApiClient {
    private final ApiService apiService;

    @Inject
    public ApiClient(Retrofit retrofit) {
        this.apiService = retrofit.create(ApiService.class);
    }

    public ApiService getApiService() {
        return apiService;
    }
}