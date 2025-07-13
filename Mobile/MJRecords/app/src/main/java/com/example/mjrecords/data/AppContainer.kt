package com.example.mjrecords.data

import android.content.Context
import com.example.mjrecords.network.ApiService
import com.example.mjrecords.network.AuthInterceptor
import com.jakewharton.retrofit2.converter.kotlinx.serialization.asConverterFactory
import kotlinx.serialization.json.Json
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

interface AppContainer {
    val tokenManager: TokenManager
    val loginRepository: LoginRepository
    val departmentRepository: DepartmentRepository
    val poRepository: PORepository
    val employeeRepository: EmployeeRepository
}

class DefaultAppContainer(
    private val appContext: Context
) : AppContainer {
    // If you're using the android emulator use this address to refer to
    // the host machine
    private val baseUrl = "http://10.0.2.2:7030/api/"
    // For physical android device use the computer ip address
    // private val baseUrl = "http://10.0.0.121:7030/api/"

    //For running on API running on Win VM
//    private val baseUrl = "http://10.211.55.3:7030/api/"
    override val tokenManager: TokenManager by lazy {
        TokenManager(appContext)
    }

    private val client = OkHttpClient.Builder()
        .addInterceptor(AuthInterceptor(tokenManager))
        .build()

    private val json = Json {
        ignoreUnknownKeys = true
    }

    private val retrofit = Retrofit.Builder()
        .baseUrl(baseUrl)
        .addConverterFactory(GsonConverterFactory.create())
        .client(client)
        .build()

    private val apiService: ApiService by lazy {
        retrofit.create(ApiService::class.java)
    }

    override val loginRepository: LoginRepository by lazy {
        RemoteLoginRepository(apiService)
    }

    override val departmentRepository: DepartmentRepository by lazy {
        RemoteDepartmentRepository(apiService)
    }

    override val poRepository: PORepository by lazy {
        RemotePORepository(apiService)
    }

    override val employeeRepository: EmployeeRepository by lazy {
        RemoteEmployeeRepository(apiService)
    }

}