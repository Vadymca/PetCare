package com.example.petcareapp.data.room;

import android.content.Context;

import androidx.room.Database;
import androidx.room.Room;
import androidx.room.RoomDatabase;
import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.data.room.daos.AnimalDao;
import com.example.petcareapp.data.room.daos.ShelterDao;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import com.example.petcareapp.data.room.entities.ShelterEntity;

import javax.inject.Singleton;

import dagger.Provides;

@Database(entities = {AnimalEntity.class, ShelterEntity.class}, version = 1, exportSchema = false)
public abstract class PetCareDatabase extends RoomDatabase {
    public abstract AnimalDao animalDao();
    public abstract ShelterDao shelterDao();

//    @Provides
//    @Singleton
//    public PetCareDatabase provideDatabase() {
//        Context application = PetCareApplication.getInstance();
//        return Room.databaseBuilder(application, PetCareDatabase.class, "petcare-db").build();
//    }
}