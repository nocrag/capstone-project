package com.example.mjrecords.model

import kotlinx.serialization.Serializable

@Serializable
data class ItemDto(
    val id: Int,
    val purchaseOrderId: String,
    val name: String,
    val description: String,
    val quantity: Int,
    val price: Double,
    val justification: String,
    val purchaseLocation: String,
    val status: Int,
    val recordVersion: String,
    val denialReason: String,
    val errors: List<String> = emptyList()
)
