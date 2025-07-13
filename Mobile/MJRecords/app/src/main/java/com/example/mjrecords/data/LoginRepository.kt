package com.example.mjrecords.data

import android.util.Log
import com.example.mjrecords.model.LoginRequest
import com.example.mjrecords.model.LoginResponse
import com.example.mjrecords.network.ApiService

interface LoginRepository {
    suspend fun login(loginRequest: LoginRequest): LoginResponse
}

class RemoteLoginRepository(
    private val apiService: ApiService
) : LoginRepository {
    override suspend fun login(loginRequest: LoginRequest): LoginResponse {
        Log.d("LOGIN_DEBUG", "Sending login request: ${loginRequest.username}, ${loginRequest.password}")

        return apiService.login(loginRequest)
    }
}