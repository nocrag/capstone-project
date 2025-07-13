package com.example.mjrecords.ui.viewmodels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import com.example.mjrecords.MJRecordsApplication
import com.example.mjrecords.data.EmployeeRepository
import com.example.mjrecords.model.EmployeeSearchDto
import com.example.mjrecords.model.EmployeeSearchResultDto
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

class EmployeeSearchViewModel (
    private val employeeRepository : EmployeeRepository
) : ViewModel() {

    private val _searchDTO = MutableStateFlow(EmployeeSearchDto())
    val searchDto: StateFlow<EmployeeSearchDto> = _searchDTO

    private val _searchResults = MutableStateFlow<List<EmployeeSearchResultDto>>(emptyList())
    val searchResults: StateFlow<List<EmployeeSearchResultDto>> get() = _searchResults

    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> get() = _isLoading

    private val _errorMessage = MutableStateFlow<String?>(null)
    val errorMessage: StateFlow<String?> get() = _errorMessage

    // start a search
    fun searchEmployee() {
        viewModelScope.launch {
            _isLoading.value = true
            _errorMessage.value = null
            try {
                val result = employeeRepository.getEmployeesFromSearch(_searchDTO.value)
                _searchResults.value = result
            } catch (e: Exception) {
                _errorMessage.value = e.message ?: "An unexpected error occurred."
            } finally {
                _isLoading.value = false
            }
        }
    }

    // update the search criteria
    fun updateSearchDto(newDto: EmployeeSearchDto) {
        _searchDTO.value = newDto
    }

    fun getEmployeeById(id: String?): EmployeeSearchResultDto? {
        return searchResults.value.find { it.id == id }
    }

    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val application = (this[APPLICATION_KEY] as MJRecordsApplication)
                EmployeeSearchViewModel(
                    employeeRepository = application.container.employeeRepository
                )
            }
        }
    }
}