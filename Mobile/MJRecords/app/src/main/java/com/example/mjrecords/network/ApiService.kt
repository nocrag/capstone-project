package com.example.mjrecords.network
import com.example.mjrecords.model.Department
import com.example.mjrecords.model.EmployeeDetailDto
import com.example.mjrecords.model.EmployeeSearchDto
import com.example.mjrecords.model.EmployeeSearchResultDto
import com.example.mjrecords.model.ItemDto
import com.example.mjrecords.model.LoginRequest
import com.example.mjrecords.model.LoginResponse
import com.example.mjrecords.model.POSummaryDto
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Query

interface ApiService {
    @POST("Login")
    suspend fun login(@Body request: LoginRequest) : LoginResponse


    @GET("Department")
    suspend fun getDepartments(): List<Department>

    @GET("PurchaseOrder/Department")
    suspend fun getPOsByDepartment(
        @Query("departmentId") departmentId: Int
    ): List<POSummaryDto>

    @GET("PurchaseOrder/Items")
    suspend fun getPurchaseOrderItems(
        @Query("poId") poId: String
    ): List<ItemDto>

    @GET("Employee")
    suspend fun getEmployeesFromSearch(
        @Query("departmentId") departmentId: Int?,
        @Query("employeeId") employeeId: String,
        @Query("lastName") lastName: String
    ): List<EmployeeSearchResultDto>

    @GET("Employee/Detail/{id}")
    suspend fun getEmployeeDetail(
        @Path("id") employeeId: String
    ) : EmployeeDetailDto

}