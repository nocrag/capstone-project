package com.example.mjrecords.ui.viewmodels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import com.example.mjrecords.MJRecordsApplication
import com.example.mjrecords.data.LoginRepository
import com.example.mjrecords.data.TokenManager
import com.example.mjrecords.model.LoginRequest
import com.example.mjrecords.model.LoginResponse
import kotlinx.coroutines.channels.Channel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.receiveAsFlow
import kotlinx.coroutines.launch

class LoginViewModel(
    private val loginRepository: LoginRepository,
    private val tokenManager: TokenManager
): ViewModel() {
    private val _uiState = MutableStateFlow(LoginUiState())
    val uiState: StateFlow<LoginUiState> = _uiState.asStateFlow()

    private val _loginSuccessEvent = Channel<Unit>()
    val loginSuccessEvent = _loginSuccessEvent.receiveAsFlow()

    private val _loginResponse = MutableStateFlow<LoginResponse?>(null)
    val loginResponse: StateFlow<LoginResponse?> = _loginResponse.asStateFlow()

    init {
        viewModelScope.launch {
            val token = tokenManager.getToken()
            val user = tokenManager.getUser()
            if (!token.isNullOrEmpty() && user != null) {
                _loginResponse.value = user
                _uiState.value = _uiState.value.copy(isLoggedIn = true)
                _loginSuccessEvent.send(Unit)
            }
        }
    }


    fun onUsernameChanged(newUserName: String) {
        _uiState.value = _uiState.value.copy(username = newUserName)
    }

    fun onPasswordChanged(newPassword: String) {
        _uiState.value = _uiState.value.copy(password = newPassword)
    }

    fun login() {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(isLoading = true, errorMessage = null)
            try {
                val response = loginRepository.login(
                    LoginRequest(_uiState.value.username, _uiState.value.password))
                tokenManager.saveToken(response.token)
                tokenManager.saveUser(response)
                _loginResponse.value = response
                _uiState.value = _uiState.value.copy(isLoggedIn = true, isLoading = false)
                _loginSuccessEvent.send(Unit)
            } catch (e: Exception) {
                _uiState.value = _uiState.value.copy(isLoggedIn = false, isLoading = false,
                    errorMessage = "Login Failed. Please try again.")
            }
        }
    }

    fun logout() {
        viewModelScope.launch {
            tokenManager.clearToken()
            _loginResponse.value = null
            _uiState.value = LoginUiState()
        }
    }


    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val application = (this[APPLICATION_KEY] as MJRecordsApplication)
                val loginRepository = application.container.loginRepository
                val tokenManager = TokenManager(application.applicationContext)
                LoginViewModel(
                    loginRepository = loginRepository,
                    tokenManager = tokenManager)
            }
        }
    }
}

data class LoginUiState(
    val username: String = "",
    val password: String = "",
    val errorMessage: String? = null,
    val isLoading: Boolean = false,
    val isLoggedIn: Boolean = false

)