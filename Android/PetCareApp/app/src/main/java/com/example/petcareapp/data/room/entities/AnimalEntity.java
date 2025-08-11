package com.example.petcareapp.data.room.entities;

import androidx.annotation.NonNull;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;
import com.example.petcareapp.data.room.converters.JsonConverter;

import java.util.Collections;
import java.util.List;

@Entity(tableName = "animals")
public class AnimalEntity {
    @NonNull
    @PrimaryKey
    private String id;
    private String slug;
    private String userId;
    private String name;
    private String breedId;
    private String birthday;
    private String gender;
    private String description;
    private String healthStatus;

    @TypeConverters(JsonConverter.class)
    private List<String> photos;

    @TypeConverters(JsonConverter.class)
    private List<String> videos;

    private String shelterId;
    private String status;
    private String adoptionRequirements;
    private String microchipId;
    private int idNumber;
    private double weight;
    private double height;
    private String color;
    private boolean isSterilized;
    private boolean haveDocuments;
    private String createdAt;
    private String updatedAt;

    public AnimalEntity() {}
    public AnimalEntity(String id, String slug, String userId, String name, String breedId, List<String> birthday, String gender, String description, String healthStatus, String photos, String videos, String shelterId, List<String> status, String adoptionRequirements, String microchipId, int idNumber, double weight, double height, String color, boolean isSterilized, boolean haveDocuments, String createdAt, String updatedAt) {
        this.id = id;
        this.slug = slug;
        this.userId = userId;
        this.name = name;
        this.breedId = breedId;
        this.birthday = String.valueOf(birthday);
        this.gender = gender;
        this.description = description;
        this.healthStatus = healthStatus;
        this.photos = Collections.singletonList(photos);
        this.videos = Collections.singletonList(videos);
        this.shelterId = shelterId;
        this.status = String.valueOf(status);
        this.adoptionRequirements = adoptionRequirements;
        this.microchipId = microchipId;
        this.idNumber = idNumber;
        this.weight = weight;
        this.height = height;
        this.color = color;
        this.isSterilized = isSterilized;
        this.haveDocuments = haveDocuments;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getSlug() {
        return slug;
    }

    public void setSlug(String slug) {
        this.slug = slug;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getBreedId() {
        return breedId;
    }

    public void setBreedId(String breedId) {
        this.breedId = breedId;
    }

    public String getBirthday() {
        return birthday;
    }

    public void setBirthday(String birthday) {
        this.birthday = birthday;
    }

    public String getGender() {
        return gender;
    }

    public void setGender(String gender) {
        this.gender = gender;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getHealthStatus() {
        return healthStatus;
    }

    public void setHealthStatus(String healthStatus) {
        this.healthStatus = healthStatus;
    }

    public List<String> getPhotos() {
        return photos;
    }

    public void setPhotos(List<String> photos) {
        this.photos = photos;
    }

    public List<String> getVideos() {
        return videos;
    }

    public void setVideos(List<String> videos) {
        this.videos = videos;
    }

    public String getShelterId() {
        return shelterId;
    }

    public void setShelterId(String shelterId) {
        this.shelterId = shelterId;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getAdoptionRequirements() {
        return adoptionRequirements;
    }

    public void setAdoptionRequirements(String adoptionRequirements) {
        this.adoptionRequirements = adoptionRequirements;
    }

    public String getMicrochipId() {
        return microchipId;
    }

    public void setMicrochipId(String microchipId) {
        this.microchipId = microchipId;
    }

    public int getIdNumber() {
        return idNumber;
    }

    public void setIdNumber(int idNumber) {
        this.idNumber = idNumber;
    }

    public double getWeight() {
        return weight;
    }

    public void setWeight(double weight) {
        this.weight = weight;
    }

    public double getHeight() {
        return height;
    }

    public void setHeight(double height) {
        this.height = height;
    }

    public String getColor() {
        return color;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public boolean isSterilized() {
        return isSterilized;
    }

    public void setSterilized(boolean sterilized) {
        isSterilized = sterilized;
    }

    public boolean isHaveDocuments() {
        return haveDocuments;
    }

    public void setHaveDocuments(boolean haveDocuments) {
        this.haveDocuments = haveDocuments;
    }

    public String getCreatedAt() {
        return createdAt;
    }

    public void setCreatedAt(String createdAt) {
        this.createdAt = createdAt;
    }

    public String getUpdatedAt() {
        return updatedAt;
    }

    public void setUpdatedAt(String updatedAt) {
        this.updatedAt = updatedAt;
    }
}