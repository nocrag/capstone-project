package com.example.mjrecords.data


import com.example.mjrecords.model.ItemDto
import com.example.mjrecords.model.POSummaryDto
import com.example.mjrecords.network.ApiService

interface PORepository {
    suspend fun getPOsByDepartment(departmentId: Int): List<POSummaryDto>
    suspend fun getPurchaseOrderItems(poNumber: String): List<ItemDto>

}

class RemotePORepository(
    private val apiService: ApiService
) : PORepository {
    override suspend fun getPOsByDepartment(departmentId: Int): List<POSummaryDto> {
        return apiService.getPOsByDepartment(departmentId)
    }
    override suspend fun getPurchaseOrderItems(poNumber: String): List<ItemDto> {
        return apiService.getPurchaseOrderItems(poNumber)
    }
}

