package com.example.petcareapp.di;

import android.app.Application;

import androidx.annotation.NonNull;
import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;
import androidx.room.Room;
import com.example.petcareapp.data.api.ApiClient;
import com.example.petcareapp.data.api.ApiService;
import com.example.petcareapp.data.repository.AnimalRepository;
import com.example.petcareapp.data.room.PetCareDatabase;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel;
import okhttp3.OkHttpClient;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import javax.inject.Singleton;
import dagger.Module;
import dagger.Provides;

@Module
public class AppModule {

    private static final String BASE_URL = "http://10.0.2.2:3000/"; // Для емулятора
    // private static final String BASE_URL = "https://api.petcare.com/api/"; // Для реального бекенду
    private final Application application;

    public AppModule(Application application) {
        this.application = application;
    }

    @Provides
    @Singleton
    public OkHttpClient provideOkHttpClient() {
        HttpLoggingInterceptor logging = new HttpLoggingInterceptor();
        logging.setLevel(HttpLoggingInterceptor.Level.BODY);
        return new OkHttpClient.Builder()
                .addInterceptor(logging)
                .build();
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
    public PetCareDatabase provideDatabase() {
        return Room.databaseBuilder(application, PetCareDatabase.class, "petcare-db")
                .allowMainThreadQueries() // Тільки для тестування!
                .build();
    }

    @Provides
    @Singleton
    public ApiClient provideApiClient(Retrofit retrofit) {
        return new ApiClient(retrofit);
    }

    @Provides
    @Singleton
    public ApiService provideApiService(Retrofit retrofit) {
        return retrofit.create(ApiService.class);
    }

    @Provides
    @Singleton
    public AnimalRepository provideAnimalRepository(ApiService apiService, PetCareDatabase database) {
        return new AnimalRepository(
                apiService,
                database.animalDao(),
                database.shelterDao(),
                database.speciesDao(),
                database.breedDao(),
                database.userDao()
        );
    }

    @Provides
    @Singleton
    public AnimalViewModel provideAnimalViewModel(AnimalRepository repository) {
        return new AnimalViewModel(repository);
    }

    @Provides
    @Singleton
    public ViewModelProvider.Factory provideViewModelFactory(AnimalRepository repository) {
        return new ViewModelProvider.Factory() {
            @NonNull
            @Override
            public <T extends ViewModel> T create(@NonNull Class<T> modelClass) {
                if (modelClass.isAssignableFrom(AnimalViewModel.class)) {
                    return (T) new AnimalViewModel(repository);
                }
                throw new IllegalArgumentException("Unknown ViewModel class");
            }
        };
    }
}
//    private static final String BASE_URL = "http://json-server:3000/";
//    // private static final String BASE_URL = "https://api.petcare.com/api/";
//    private final Application application;
//
//
//    @Provides
//    @Singleton
//    public OkHttpClient provideOkHttpClient() {
//        HttpLoggingInterceptor logging = new HttpLoggingInterceptor();
//        logging.setLevel(HttpLoggingInterceptor.Level.BODY);
//        return new OkHttpClient.Builder()
//                .addInterceptor(logging)
//                .build();
//    }
//
//    @Provides
//    @Singleton
//    public Retrofit provideRetrofit(OkHttpClient okHttpClient) {
//        return new Retrofit.Builder()
//                .baseUrl(BASE_URL)
//                .client(okHttpClient)
//                .addConverterFactory(GsonConverterFactory.create())
//                .build();
//    }
//
//    @Provides
//    @Singleton
//    public PetCareDatabase provideDatabase() {
//        return Room.databaseBuilder(application, PetCareDatabase.class, "petcare-db").build();
//    }
//
//    @Provides
//    @Singleton
//    public ApiClient provideApiClient(Retrofit retrofit) {
//        return new ApiClient(retrofit);
//    }
//
//    @Provides
//    @Singleton
//    public ApiService provideApiService(Retrofit retrofit) {
//        return retrofit.create(ApiService.class);
//    }
//
//    @Provides
//    @Singleton
//    public AnimalRepository provideAnimalRepository(ApiService apiService, PetCareDatabase database) {
//        return new AnimalRepository(apiService, database.animalDao(), database.shelterDao());
//    }
//

