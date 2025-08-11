package com.example.petcareapp.data.room.entities;

import androidx.annotation.NonNull;
import androidx.room.Entity;
import androidx.room.Ignore;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.example.petcareapp.data.models.Shelter;
import com.example.petcareapp.data.room.converters.JsonConverter;
import java.util.List;
import java.util.Map;

@Entity(tableName = "shelters")
public class ShelterEntity {
    @PrimaryKey@NonNull
    private String id;

    private String slug;
    private String name;
    private String address;


    public ShelterEntity(String id, String slug, String name, String address, Shelter.Coordinates coordinates, String contactPhone, String contactEmail, String description, int capacity, int currentOccupancy, List<String> photos, String virtualTourUrl, String workingHours, Map<String, String> socialMedia, String managerId, String createdAt, String updatedAt) {
        this.id = id;
        this.slug = slug;
        this.name = name;
        this.address = address;
        this.coordinates = coordinates;
        this.contactPhone = contactPhone;
        this.contactEmail = contactEmail;
        this.description = description;
        this.capacity = capacity;
        this.currentOccupancy = currentOccupancy;
        this.photos = photos;
        this.virtualTourUrl = virtualTourUrl;
        this.workingHours = workingHours;
        this.socialMedia = socialMedia;
        this.managerId = managerId;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }

    @TypeConverters(JsonConverter.class)
    private Shelter.Coordinates coordinates;

    private String contactPhone;
    private String contactEmail;
    private String description;
    private int capacity;
    private int currentOccupancy;

    @TypeConverters(JsonConverter.class)
    private List<String> photos;

    private String virtualTourUrl;
    private String workingHours;

    @TypeConverters(JsonConverter.class)
    private Map<String, String> socialMedia;

    private String managerId;
    private String createdAt;
    private String updatedAt;

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

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public Shelter.Coordinates getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Shelter.Coordinates coordinates) {
        this.coordinates = coordinates;
    }

    public String getContactPhone() {
        return contactPhone;
    }

    public void setContactPhone(String contactPhone) {
        this.contactPhone = contactPhone;
    }

    public String getContactEmail() {
        return contactEmail;
    }

    public void setContactEmail(String contactEmail) {
        this.contactEmail = contactEmail;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public int getCapacity() {
        return capacity;
    }

    public void setCapacity(int capacity) {
        this.capacity = capacity;
    }

    public int getCurrentOccupancy() {
        return currentOccupancy;
    }

    public void setCurrentOccupancy(int currentOccupancy) {
        this.currentOccupancy = currentOccupancy;
    }

    public List<String> getPhotos() {
        return photos;
    }

    public void setPhotos(List<String> photos) {
        this.photos = photos;
    }

    public String getVirtualTourUrl() {
        return virtualTourUrl;
    }

    public void setVirtualTourUrl(String virtualTourUrl) {
        this.virtualTourUrl = virtualTourUrl;
    }

    public String getWorkingHours() {
        return workingHours;
    }

    public void setWorkingHours(String workingHours) {
        this.workingHours = workingHours;
    }

    public Map<String, String> getSocialMedia() {
        return socialMedia;
    }

    public void setSocialMedia(Map<String, String> socialMedia) {
        this.socialMedia = socialMedia;
    }

    public String getManagerId() {
        return managerId;
    }

    public void setManagerId(String managerId) {
        this.managerId = managerId;
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