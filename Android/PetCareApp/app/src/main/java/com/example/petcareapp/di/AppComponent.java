package com.example.petcareapp.di;

import android.app.Application;
import com.example.petcareapp.data.api.ApiClient;
import com.example.petcareapp.data.room.PetCareDatabase;
import com.example.petcareapp.ui.fragments.AnimalDetailFragment;
import com.example.petcareapp.ui.fragments.AnimalListFragment;
import com.example.petcareapp.ui.fragments.EnterFragment;
import com.example.petcareapp.ui.fragments.HelpFragment;
import com.example.petcareapp.ui.fragments.HomeFragment;
import com.example.petcareapp.ui.fragments.LoginFragment;
import com.example.petcareapp.ui.fragments.OnboardingFragment1;
import com.example.petcareapp.ui.fragments.OnboardingFragment2;
import com.example.petcareapp.ui.fragments.OnboardingFragment3;
import com.example.petcareapp.ui.fragments.RegistrationFragment;
import com.example.petcareapp.ui.fragments.RegistrationPart1Fragment;
import com.example.petcareapp.ui.fragments.RegistrationPart2Fragment;
import com.example.petcareapp.ui.fragments.ResetPasswordFragment;
import com.example.petcareapp.ui.fragments.ShelterDetailFragment;
import com.example.petcareapp.ui.fragments.ShelterListFragment;
import com.example.petcareapp.ui.fragments.TwoFactorFragment;
import com.example.petcareapp.ui.viewmodels.AnimalViewModel; // Додай, якщо ще не додав
import com.example.petcareapp.ui.viewmodels.AuthViewModel;
import com.example.petcareapp.ui.viewmodels.HomeViewModel;
import com.example.petcareapp.ui.viewmodels.OnboardingViewModel;

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
    void inject(LoginFragment fragment);
    void inject(TwoFactorFragment fragment);
    void inject(OnboardingFragment1 fragment);
    void inject(OnboardingFragment2 fragment);
    void inject(OnboardingFragment3 fragment);
    void inject(EnterFragment fragment);
    void inject(RegistrationFragment fragment);
    void inject(ResetPasswordFragment resetPasswordFragment);
    void inject(RegistrationPart1Fragment fragment);
    void inject(RegistrationPart2Fragment fragment);
    void inject(HelpFragment fragment);

    void inject(HomeFragment fragment);

    ApiClient getApiClient();
    PetCareDatabase getPetCareDatabase();
    AnimalViewModel getAnimalViewModel();
    AuthViewModel getAuthViewModel();
    OnboardingViewModel getOnboardingViewModel();
}
