package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class EmployeeDetailDto(
    val firstName: String,
    val middleInitial: String?,
    val lastName: String,
    val homeAddress: String,
    val workPhone: String,
    val cellPhone: String,
    val workEmail: String,
)
