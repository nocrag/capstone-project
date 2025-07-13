package com.example.mjrecords.data

import com.example.mjrecords.model.Department
import com.example.mjrecords.network.ApiService

interface DepartmentRepository {
    suspend fun getDepartments(): List<Department>
}

class RemoteDepartmentRepository(
    private val apiService: ApiService
) : DepartmentRepository {
    override suspend fun getDepartments(): List<Department> {
        return apiService.getDepartments()
    }

}
