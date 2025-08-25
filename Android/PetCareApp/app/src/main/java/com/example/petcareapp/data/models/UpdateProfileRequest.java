package com.example.petcareapp.data.models;

public class UpdateProfileRequest {
    private String firstName;
    private String lastName;
    private String phone;
    private String language;

    public UpdateProfileRequest(String firstName, String lastName, String phone, String language) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.phone = phone;
        this.language = language;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public String getLanguage() {
        return language;
    }

    public void setLanguage(String language) {
        this.language = language;
    }
}
