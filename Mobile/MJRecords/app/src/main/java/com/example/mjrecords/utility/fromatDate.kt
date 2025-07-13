package com.example.mjrecords.utility

import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

fun formatDate(input: String): String {
    return try {
        val parsedDate = LocalDateTime.parse(input)
        val formatter = DateTimeFormatter.ofPattern("MMM dd, yyyy")
        parsedDate.format(formatter)
    } catch (e: Exception) {
        input
    }
}
