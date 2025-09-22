package com.example.petcareapp;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.navigation.ui.AppBarConfiguration;
import androidx.navigation.ui.NavigationUI;

import com.example.petcareapp.databinding.ActivityMainBinding;
import com.google.android.material.bottomnavigation.BottomNavigationView;

public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        if (binding.navHostFragment == null) {
            Log.e("MainActivity", "NavHostFragment is null in binding!");
            return;
        }

        binding.navHostFragment.post(() -> {
            try {
                NavController navController = Navigation.findNavController(binding.navHostFragment);
                Log.d("MainActivity", "NavController: " + navController.toString());

                BottomNavigationView bottomNav = binding.bottomNavigationView;
                NavigationUI.setupWithNavController(bottomNav, navController);

                // слушатель смены фрагментов
                navController.addOnDestinationChangedListener((controller, destination, arguments) -> {
                    int destId = destination.getId();
                    if (destId == R.id.homeFragment) {
                        bottomNav.setVisibility(View.VISIBLE);  // показываем на основных экранах
                    } else {
                        bottomNav.setVisibility(View.GONE);     // скрываем на онбординге/логине/регистрации
                    }
                });

                Log.d("MainActivity", "NavController initialized successfully");
            } catch (IllegalStateException e) {
                Log.e("MainActivity", "Failed to initialize NavController: " + e.getMessage());
            }
        });
    }

    @Override
    public boolean onSupportNavigateUp() {
        if (binding.navHostFragment == null) {
            Log.e("MainActivity", "NavHostFragment is null in onSupportNavigateUp!");
            return super.onSupportNavigateUp();
        }
        try {
            NavController navController = Navigation.findNavController(binding.navHostFragment);
            return navController.navigateUp() || super.onSupportNavigateUp();
        } catch (IllegalStateException e) {
            Log.e("MainActivity", "Failed to navigate up: " + e.getMessage());
            return super.onSupportNavigateUp();
        }
    }
}
