package com.example.petcareapp.di;

import android.app.Application;
import com.example.petcareapp.data.api.ApiClient;
import com.example.petcareapp.data.room.PetCareDatabase;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel; // Додай, якщо ще не додав
import javax.inject.Singleton;
import dagger.Component;

@Singleton
@Component(modules = {AppModule.class})
public interface AppComponent {
    void inject(Application application);
    ApiClient getApiClient();
    PetCareDatabase getPetCareDatabase();
    AnimalViewModel getAnimalViewModel();
}