package com.example.petcareapp.di;

import android.app.Application;
import com.example.petcareapp.data.api.ApiClient;
import com.example.petcareapp.data.room.PetCareDatabase;
import com.example.petcareapp.ui.fragments.AnimalDetailFragment;
import com.example.petcareapp.ui.fragments.AnimalListFragment;
import com.example.petcareapp.ui.fragments.ShelterDetailFragment;
import com.example.petcareapp.ui.fragments.ShelterListFragment;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel; // Додай, якщо ще не додав
import javax.inject.Singleton;
import dagger.Component;

@Singleton
@Component(modules = {AppModule.class})
public interface AppComponent {
    void inject(Application application);
    void inject(AnimalListFragment fragment);
    void inject(AnimalDetailFragment fragment);
    void inject(ShelterListFragment fragment);
    void inject(ShelterDetailFragment fragment);
    ApiClient getApiClient();
    PetCareDatabase getPetCareDatabase();
    AnimalViewModel getAnimalViewModel();
}