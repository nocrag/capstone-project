package com.example.mjrecords.ui.viewmodels


import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.viewModelScope
import com.example.mjrecords.MJRecordsApplication
import com.example.mjrecords.data.PORepository
import com.example.mjrecords.model.POSummaryDto
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class DepartmentPOListViewModel(
    private val departmentId: Int,
    private val poRepository: PORepository
) : ViewModel() {

    private val _poList = MutableStateFlow<List<POSummaryDto>>(emptyList())
    val poList: StateFlow<List<POSummaryDto>> = _poList

    init {
        loadPOs()
    }

    private fun loadPOs() {
        viewModelScope.launch {
            try {
                val result = poRepository.getPOsByDepartment(departmentId)
                _poList.value = result
            } catch (e: Exception) {
                println("failed to load POs: ${e.message}")
            }
        }
    }


    companion object {
        fun factory(departmentId: Int) = viewModelFactory {
            initializer {
                val app = (this[APPLICATION_KEY] as MJRecordsApplication)
                DepartmentPOListViewModel(
                    departmentId = departmentId,
                    poRepository = app.container.poRepository
                )
            }
        }
    }
}
