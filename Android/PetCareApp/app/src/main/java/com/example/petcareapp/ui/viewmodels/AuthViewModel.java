package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.example.petcareapp.data.models.LoginResponse;
import com.example.petcareapp.data.models.UpdateProfileRequest;
import com.example.petcareapp.data.models.UserProfile;
import com.example.petcareapp.data.repository.AuthRepository;

import javax.inject.Inject;

public class AuthViewModel extends ViewModel {
    private final AuthRepository repository;

    @Inject
    public AuthViewModel(AuthRepository repository) {
        this.repository = repository;
    }

    public LiveData<LoginResponse> login(String email, String password) {
        return repository.login(email, password);
    }

    public LiveData<Void> verifyTwoFactor(String userId, String code) {
        return repository.verifyTwoFactor(userId, code);
    }

    public LiveData<Void> logout() {
        return repository.logout();
    }

    public LiveData<UserProfile> getUserProfile(String userId) {
        return repository.getUserProfile(userId);
    }

    public LiveData<Void> updateUserProfile(String userId, UpdateProfileRequest request) {
        return repository.updateUserProfile(userId, request);
    }

    public LiveData<Boolean> resetPassword(String email) {
        MutableLiveData<Boolean> result = new MutableLiveData<>();
        // Тут вызов в репозиторий / API
        // Например, имитация успешного результата:
        result.setValue(true);
        return result;
    }
    public String getUserId() {
        return repository.getUserId();
    }
}