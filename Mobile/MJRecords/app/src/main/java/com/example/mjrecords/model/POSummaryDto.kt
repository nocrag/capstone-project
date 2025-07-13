package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class POSummaryDto(
    val poNumber: String,
    val poCreationDate: String,
    val supervisorName: String,
    val poStatus: Int
)
