package com.example.petcareapp;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.util.Log;

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


public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        Log.d("MainActivity", "NavHostFragment ID: " + R.id.nav_host_fragment);
        if (binding.navHostFragment == null) {
            Log.e("MainActivity", "NavHostFragment is null in binding!");
            return;
        }

        binding.navHostFragment.post(() -> {
            try {
                NavController navController = Navigation.findNavController(binding.navHostFragment);
                Log.d("MainActivity", "NavController: " + navController.toString());
                AppBarConfiguration appBarConfiguration = new AppBarConfiguration.Builder(
                        R.id.animalListFragment, R.id.shelterListFragment)
                        .build();
                NavigationUI.setupActionBarWithNavController(this, navController, appBarConfiguration);
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


//public class MainActivity extends AppCompatActivity {
//    private ActivityMainBinding binding;
//
//    @Override
//    protected void onCreate(Bundle savedInstanceState) {
//        super.onCreate(savedInstanceState);
//        binding = ActivityMainBinding.inflate(getLayoutInflater());
//        setContentView(binding.getRoot());
//
//        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
//        AppBarConfiguration appBarConfiguration = new AppBarConfiguration.Builder(
//                R.id.animalListFragment, R.id.shelterListFragment).build();
//        NavigationUI.setupActionBarWithNavController(this, navController, appBarConfiguration);
//    }
//
//    @Override
//    public boolean onSupportNavigateUp() {
//        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
//        return navController.navigateUp() || super.onSupportNavigateUp();
//    }
//}