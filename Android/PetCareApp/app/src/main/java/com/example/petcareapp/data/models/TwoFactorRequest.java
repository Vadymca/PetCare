package com.example.petcareapp.data.models;

public class TwoFactorRequest {
    private String userId;
    private String code;

    public TwoFactorRequest(String userId, String code) {
        this.userId = userId;
        this.code = code;
    }

    public String getUserId() {
        return userId;
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public String setUserId(String userId) {
        this.userId = userId;
        return userId;
    }
}