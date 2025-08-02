package com.example.petcareapp.data.room;

import androidx.room.Database;
import androidx.room.RoomDatabase;
import com.example.petcareapp.data.room.daos.AnimalDao;
import com.example.petcareapp.data.room.daos.ShelterDao;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import com.example.petcareapp.data.room.entities.ShelterEntity;

@Database(entities = {AnimalEntity.class, ShelterEntity.class}, version = 1, exportSchema = false)
public abstract class PetCareDatabase extends RoomDatabase {
    public abstract AnimalDao animalDao();
    public abstract ShelterDao shelterDao();
}