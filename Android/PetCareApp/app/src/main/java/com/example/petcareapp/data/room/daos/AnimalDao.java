package com.example.petcareapp.data.room.daos;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import java.util.List;

@Dao
public interface AnimalDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)  // Додано REPLACE
    void insert(AnimalEntity animal);
    @Insert(onConflict = OnConflictStrategy.REPLACE)  // Додано REPLACE
    void insertAll(List<AnimalEntity> animals);
    @Query("SELECT * FROM animals")
    List<AnimalEntity> getAll();
    @Query("SELECT * FROM animals WHERE slug = :slug LIMIT 1")
    AnimalEntity getBySlug(String slug);
}