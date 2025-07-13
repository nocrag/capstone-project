package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class Department(
    val id: Int,
    val name: String,
    val description: String,
    val invocationDate: String
)
