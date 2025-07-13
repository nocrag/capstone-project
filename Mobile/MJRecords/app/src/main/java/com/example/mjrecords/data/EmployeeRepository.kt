package com.example.mjrecords.data

import com.example.mjrecords.model.EmployeeDetailDto
import com.example.mjrecords.model.EmployeeSearchDto
import com.example.mjrecords.model.EmployeeSearchResultDto
import com.example.mjrecords.network.ApiService

interface EmployeeRepository {
    suspend fun getEmployeesFromSearch(searchDto: EmployeeSearchDto) : List<EmployeeSearchResultDto>
    suspend fun getEmployeeDetail(id: String) : EmployeeDetailDto
}

class RemoteEmployeeRepository(
    private val apiService: ApiService
) : EmployeeRepository {
    override suspend fun getEmployeesFromSearch(searchDto: EmployeeSearchDto): List<EmployeeSearchResultDto> {
        return apiService.getEmployeesFromSearch(
            departmentId = searchDto.departmentId,
            employeeId = searchDto.employeeId,
            lastName = searchDto.lastName
        )
    }

    override suspend fun getEmployeeDetail(id: String): EmployeeDetailDto {
        return apiService.getEmployeeDetail(id)
    }

}