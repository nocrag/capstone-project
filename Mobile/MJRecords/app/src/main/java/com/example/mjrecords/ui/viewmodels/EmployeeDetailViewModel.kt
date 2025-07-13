package com.example.mjrecords.ui.viewmodels

import androidx.lifecycle.SavedStateHandle
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.createSavedStateHandle
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import com.example.mjrecords.MJRecordsApplication
import com.example.mjrecords.data.EmployeeRepository
import com.example.mjrecords.model.EmployeeDetailDto
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class EmployeeDetailViewModel(
    savedStateHandle: SavedStateHandle,
    private val repository: EmployeeRepository
) : ViewModel() {

    private val _employee = MutableStateFlow<EmployeeDetailDto?>(null)
    val employee: StateFlow<EmployeeDetailDto?> = _employee

    private val _isLoading = MutableStateFlow(true)
    val isLoading: StateFlow<Boolean> = _isLoading

    private val _errorMessage = MutableStateFlow<String?>(null)
    val errorMessage: StateFlow<String?> = _errorMessage

    init {
        val employeeId = savedStateHandle.get<String>("employeeId") ?: ""

        viewModelScope.launch {
            try {
                _employee.value = repository.getEmployeeDetail(employeeId)
            } catch (e: Exception) {
                _errorMessage.value = e.localizedMessage ?: "Failed to load employee details"
            } finally {
                _isLoading.value = false
            }
        }
    }

    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val application = this[APPLICATION_KEY] as MJRecordsApplication
                val repository = application.container.employeeRepository
                val savedStateHandle = createSavedStateHandle()
                EmployeeDetailViewModel(savedStateHandle, repository)
            }
        }
    }
}