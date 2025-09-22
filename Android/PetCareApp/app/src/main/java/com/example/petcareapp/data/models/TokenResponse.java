package com.example.petcareapp.data.models;

import com.google.gson.annotations.SerializedName;

public class TokenResponse {
    @SerializedName("access_token") private String accessToken;
    @SerializedName("refresh_token") private String refreshToken;
    @SerializedName("expires_in") private long expiresIn; // В секундах
    @SerializedName("token_type") private String tokenType; // "Bearer"
    @SerializedName("twoFactorRequired") private boolean twoFactorRequired; // Для 2FA после login
    @SerializedName("userId") private String userId;


    public String getAccessToken() { return accessToken; }
    public void setAccessToken(String accessToken) { this.accessToken = accessToken; }

    public String getRefreshToken() {return refreshToken; }

    public void setRefreshToken(String refreshToken) {this.refreshToken = refreshToken;}

    public long getExpiresIn() {return expiresIn;}

    public void setExpiresIn(long expiresIn) {this.expiresIn = expiresIn;}

    public String getTokenType() {return tokenType;}

    public void setTokenType(String tokenType) {this.tokenType = tokenType;}

    public boolean isTwoFactorRequired() {return twoFactorRequired;}

    public void setTwoFactorRequired(boolean twoFactorRequired) {this.twoFactorRequired = twoFactorRequired;}

    public String getUserId() {return userId;}

    public void setUserId(String userId) {this.userId = userId;}
}