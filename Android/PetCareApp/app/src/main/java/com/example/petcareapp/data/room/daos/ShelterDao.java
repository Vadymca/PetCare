package com.example.petcareapp.data.room.daos;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import com.example.petcareapp.data.room.entities.ShelterEntity;
import java.util.List;

@Dao
public interface ShelterDao {
    @Insert
    void insert(ShelterEntity shelter);

    @Query("SELECT * FROM shelters")
    List<ShelterEntity> getAll();

    @Query("SELECT * FROM shelters WHERE slug = :slug LIMIT 1")
    ShelterEntity getBySlug(String slug);
}