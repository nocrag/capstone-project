package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class EmployeeSearchDto(
    val departmentId: Int? = null,
    val employeeId: String = "",
    val lastName: String = ""
)