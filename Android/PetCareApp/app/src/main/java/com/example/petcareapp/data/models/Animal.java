package com.example.petcareapp.data.models;

import com.google.gson.annotations.SerializedName;
import java.util.List;

public class Animal {
    @SerializedName("id")
    private String id;
    @SerializedName("slug")
    private String slug;
    @SerializedName("userId")
    private String userId;
    @SerializedName("name")
    private String name;
    @SerializedName("breedId")
    private String breedId;
    @SerializedName("birthday")
    private String birthday;
    @SerializedName("gender")
    private String gender;
    @SerializedName("description")
    private String description;
    @SerializedName("healthStatus")
    private String healthStatus;
    @SerializedName("photos")
    private List<String> photos;
    @SerializedName("videos")
    private List<String> videos;
    @SerializedName("shelterId")
    private String shelterId;
    @SerializedName("status")
    private String status;
    @SerializedName("adoptionRequirements")
    private String adoptionRequirements;
    @SerializedName("microchipId")
    private String microchipId;
    @SerializedName("idNumber")
    private int idNumber;
    @SerializedName("weight")
    private double weight;
    @SerializedName("height")
    private double height;
    @SerializedName("color")
    private String color;
    @SerializedName("isSterilized")
    private boolean isSterilized;
    @SerializedName("haveDocuments")
    private boolean haveDocuments;
    @SerializedName("createdAt")
    private String createdAt;
    @SerializedName("updatedAt")
    private String updatedAt;

    /// Геттери та сеттери
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