package com.example.petcareapp.data.models;

public class VerifyEmailRequest {
    private String token; // Из email
    public VerifyEmailRequest(String token) { this.token = token; }
    public String getToken() { return token; }
    public void setToken(String token) { this.token = token; }
}