package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class LoginResponse(
    val id: String,
    val firstName: String,
    val lastName: String,
    val middleInitial: String?,
    val role: String,
    val token: String,
)