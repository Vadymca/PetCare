package com.example.petcareapp.data.room.converters;

import android.util.Log;

import com.example.petcareapp.data.models.Shelter;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import java.lang.reflect.Type;
import java.util.List;
import java.util.Map;

import androidx.room.TypeConverter;

public class JsonConverter {
    private static final Gson gson = new Gson();

    @TypeConverter
    public static List<String> fromStringList(String value) {
        if (value == null) {
            return null;
        }
        Type listType = new TypeToken<List<String>>() {}.getType();
        return gson.fromJson(value, listType);
    }

    @TypeConverter
    public static String toStringList(List<String> list) {
        if (list == null) {
            return null;
        }
        return gson.toJson(list);
    }

    @TypeConverter
    public static Shelter.Coordinates fromCoordinatesString(String value) {
        if (value == null) {
            return null;
        }
        return gson.fromJson(value, Shelter.Coordinates.class);
    }

    @TypeConverter
    public static String toCoordinatesString(Shelter.Coordinates coordinates) {
        if (coordinates == null) {
            return null;
        }
        return gson.toJson(coordinates);
    }

    @TypeConverter
    public static Map<String, String> fromSocialMediaString(String value) {
        if (value == null) {
            return null;
        }
        Type mapType = new TypeToken<Map<String, String>>() {}.getType();
        return gson.fromJson(value, mapType);
    }

    @TypeConverter
    public static String toSocialMediaString(Map<String, String> socialMedia) {
        if (socialMedia == null) {
            return null;
        }
        return gson.toJson(socialMedia);
    }

    public static String toJson(List<String> photos) {
        if (photos == null) {
            return null;
        }
        try {
            return gson.toJson(photos);
        } catch (Exception e) {
            Log.e("JsonConverter", "Failed to convert photos to JSON", e);
            return null;
        }
    }

    public static List<String> fromJson(String tmp) {
        if (tmp == null) {
            return null;
        }
        try {
            Type listType = new TypeToken<List<String>>() {}.getType();
            return gson.fromJson(tmp, listType);
        } catch (Exception e) {
            Log.e("JsonConverter", "Failed to parse JSON to photos", e);
            return null;
        }
    }
}