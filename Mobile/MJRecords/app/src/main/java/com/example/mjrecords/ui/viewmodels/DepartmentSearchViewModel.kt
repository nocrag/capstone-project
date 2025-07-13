package com.example.mjrecords.ui.viewmodels

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import com.example.mjrecords.MJRecordsApplication
import com.example.mjrecords.data.DepartmentRepository
import com.example.mjrecords.model.Department
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch

class DepartmentSearchViewModel(
    private val departmentRepository: DepartmentRepository
) : ViewModel() {

    val searchQuery = MutableStateFlow("")

    private val _departments = MutableStateFlow<List<Department>>(emptyList())
    val departments: StateFlow<List<Department>> = _departments


    init {
        loadDepartments()
    }

    private fun loadDepartments() {
        viewModelScope.launch {
            try {
                val result = departmentRepository.getDepartments()
                println("Loaded departments: $result")
                _departments.value = result
            } catch (e: Exception) {
                println("ERROR fetching departments: ${e.message}")
                _departments.value = emptyList()
            }
        }
    }


    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val application = (this[APPLICATION_KEY] as MJRecordsApplication)
                val repo = application.container.departmentRepository
                DepartmentSearchViewModel(repo)
            }
        }
    }
}
