package com.example.mjrecords.data

import android.content.Context
import com.example.mjrecords.model.LoginResponse
import kotlinx.serialization.json.Json
import kotlinx.serialization.encodeToString


class TokenManager(context: Context) {
    private val prefKey = "auth_token"
    private val sharedPreferences =
        context.getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)

    private val preferences = context.getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)

    fun saveToken(token: String) {
        sharedPreferences.edit().putString(prefKey, token).apply()
    }

    fun getToken(): String? {
        return sharedPreferences.getString(prefKey, null)
    }

    fun clearToken() {
        sharedPreferences.edit().remove(prefKey).apply()
    }


    fun saveUser(response: LoginResponse) {
        val json = Json.encodeToString(response)
        preferences.edit().putString("cached_user", json).apply()
    }

    fun getUser(): LoginResponse? {
        val json = preferences.getString("cached_user", null) ?: return null
        return try {
            Json.decodeFromString<LoginResponse>(json)
        } catch (e: Exception) {
            null
        }
    }

    fun clearUser() {
        preferences.edit().remove("cached_user").apply()
    }
}