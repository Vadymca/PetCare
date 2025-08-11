package com.example.petcareapp.data.room.daos;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;

import com.example.petcareapp.data.room.entities.UserEntity;

import java.util.List;

@Dao
public interface UserDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)  // Додано REPLACE
    void insertAll(List<UserEntity> users);
    @Insert(onConflict = OnConflictStrategy.REPLACE)  // Додано REPLACE
    void insert(UserEntity user);
    @Query("SELECT * FROM users")
    List<UserEntity> getAllUsers();
    @Query("SELECT * FROM users WHERE id = :id")
    UserEntity getUserById(String id);
}