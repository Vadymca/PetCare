package com.example.petcareapp.data.models;

public class LoginResponse {
    private String userId;
    private String message; // Наприклад, "2FA required" або "Login successful"
    private boolean twoFactorRequired;

    public String getUserId() { return userId; }
    public void setUserId(String userId) { this.userId = userId; }
    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }
    public boolean isTwoFactorRequired() { return twoFactorRequired; }
    public void setTwoFactorRequired(boolean twoFactorRequired) { this.twoFactorRequired = twoFactorRequired; }
}