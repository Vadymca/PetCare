package com.example.petcareapp.data.room.daos;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;

import com.example.petcareapp.data.room.entities.SpeciesEntity;

import java.util.List;

@Dao
public interface SpeciesDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)  // Додано REPLACE
    void insertAll(List<SpeciesEntity> species);
    @Query("SELECT * FROM species")
    List<SpeciesEntity> getAllSpecies();
    @Query("SELECT * FROM species WHERE id = :id")
    SpeciesEntity getSpeciesById(String id);
}