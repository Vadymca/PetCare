package com.example.petcareapp.data.room.daos;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;

import com.example.petcareapp.data.room.entities.BreedEntity;

import java.util.List;

@Dao
public interface BreedDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)  // Додано REPLACE
    void insertAll(List<BreedEntity> breeds);
    @Query("SELECT * FROM breeds")
    List<BreedEntity> getAllBreeds();
    @Query("SELECT * FROM breeds WHERE id = :id")
    BreedEntity getBreedById(String id);
}