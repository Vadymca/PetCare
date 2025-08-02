package com.example.petcareapp.data.room.converters;

import android.util.Log;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import java.lang.reflect.Type;
import java.util.List;

import androidx.room.TypeConverter;

public class JsonConverter {
    private static final Gson gson = new Gson();

    @TypeConverter
    public static List<String> fromJson(String json) {
        if (json == null) return null;
        Type listType = new TypeToken<List<String>>(){}.getType();
        return gson.fromJson(json, listType);
    }

    @TypeConverter
    public static String toJson(List<String> list) {
        if (list == null) return null;
        return gson.toJson(list);
    }
}