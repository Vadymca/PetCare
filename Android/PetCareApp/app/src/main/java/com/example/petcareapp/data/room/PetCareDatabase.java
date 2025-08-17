package com.example.petcareapp.data.room;

import android.content.Context;

import androidx.room.Database;
import androidx.room.Room;
import androidx.room.RoomDatabase;
import androidx.room.TypeConverters;

import com.example.petcareapp.PetCareApplication;
import com.example.petcareapp.data.room.converters.JsonConverter;
import com.example.petcareapp.data.room.daos.AnimalDao;
import com.example.petcareapp.data.room.daos.BreedDao;
import com.example.petcareapp.data.room.daos.ShelterDao;
import com.example.petcareapp.data.room.daos.SpeciesDao;
import com.example.petcareapp.data.room.daos.UserDao;
import com.example.petcareapp.data.room.entities.AnimalEntity;
import com.example.petcareapp.data.room.entities.BreedEntity;
import com.example.petcareapp.data.room.entities.ShelterEntity;
import com.example.petcareapp.data.room.entities.SpeciesEntity;
import com.example.petcareapp.data.room.entities.UserEntity;

import javax.inject.Singleton;

import dagger.Provides;

@Database(entities = {AnimalEntity.class, ShelterEntity.class, SpeciesEntity.class, BreedEntity.class, UserEntity.class}, version = 1, exportSchema = false)
@TypeConverters({JsonConverter.class})
public abstract class PetCareDatabase extends RoomDatabase {
    public abstract AnimalDao animalDao();
    public abstract ShelterDao shelterDao();
    public abstract SpeciesDao speciesDao();
    public abstract BreedDao breedDao();
    public abstract UserDao userDao();
}
//@Database(entities = {AnimalEntity.class, ShelterEntity.class}, version = 1, exportSchema = false)
//public abstract class PetCareDatabase extends RoomDatabase {
//    public abstract AnimalDao animalDao();
//    public abstract ShelterDao shelterDao();
//
////    @Provides
////    @Singleton
////    public PetCareDatabase provideDatabase() {
////        Context application = PetCareApplication.getInstance();
////        return Room.databaseBuilder(application, PetCareDatabase.class, "petcare-db").build();
////    }
//}