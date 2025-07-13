package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class EmployeeSearchResultDto(
    val id : String,
    val firstName: String,
    val lastName: String,
    val workPhone: String,
    val cellPhone: String,
    val email: String,
    val officeLocation: String,
    val position: String
)
