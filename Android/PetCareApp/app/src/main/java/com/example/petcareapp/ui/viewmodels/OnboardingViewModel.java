package com.example.petcareapp.ui.viewmodels;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

public class OnboardingViewModel extends ViewModel {
    private final MutableLiveData<Integer> currentPage = new MutableLiveData<>(0);

    public LiveData<Integer> getCurrentPage() {
        return currentPage;
    }

    public void setCurrentPage(int page) {
        currentPage.setValue(page);
        // Логіка навігації (наприклад, через Navigation Component)
        // Додамо пізніше в MainActivity або фрагментах
    }
}