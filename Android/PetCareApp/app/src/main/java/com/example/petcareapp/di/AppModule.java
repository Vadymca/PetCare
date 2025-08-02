package com.example.petcareapp.di;

import android.app.Application;
import androidx.room.Room;
import com.example.petcareapp.data.api.ApiClient;
import com.example.petcareapp.data.repository.AnimalRepository;
import com.example.petcareapp.data.room.PetCareDatabase;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;
import okhttp3.OkHttpClient;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import javax.inject.Singleton;
import dagger.Module;
import dagger.Provides;

@Module
public class AppModule {

    //private static final String BASE_URL = "https://api.petcare.com/api/";
    private static final String BASE_URL = "http://json-server:3000/";
    private final Application application;

    public AppModule(Application application) {
        this.application = application;
    }

    @Provides
    @Singleton
    public OkHttpClient provideOkHttpClient() {
        return new OkHttpClient();
    }

    @Provides
    @Singleton
    public Retrofit provideRetrofit(OkHttpClient okHttpClient) {
        return new Retrofit.Builder()
                .baseUrl(BASE_URL)
                .client(okHttpClient)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
    }

    @Provides
    @Singleton
    public ApiClient provideApiClient(Retrofit retrofit) {
        return new ApiClient(retrofit);
    }

    @Provides
    @Singleton
    public PetCareDatabase provideDatabase() {
        return Room.databaseBuilder(application,
                        PetCareDatabase.class, "petcare-db")
                .build();
    }

    @Provides
    @Singleton
    public AnimalRepository provideAnimalRepository(ApiClient apiClient, PetCareDatabase database) {
        return new AnimalRepository(apiClient, database.animalDao());
    }

    @Provides
    @Singleton
    public AnimalViewModel provideAnimalViewModel(AnimalRepository repository) {
        return new AnimalViewModel(repository);
    }
}