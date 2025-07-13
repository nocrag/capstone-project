package com.example.mjrecords.ui.viewmodels


import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import com.example.mjrecords.MJRecordsApplication
import com.example.mjrecords.data.PORepository
import com.example.mjrecords.model.ItemDto
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch

class PODetailViewModel(
    private val poNumber: String,
    private val repository: PORepository
) : ViewModel() {

    private val _items = MutableStateFlow<List<ItemDto>>(emptyList())
    val items: StateFlow<List<ItemDto>> = _items

    val filteredItems: StateFlow<List<ItemDto>> = items
        .map { it.filter { item -> item.status != 2 } }
        .stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    val totalItemCount: StateFlow<Int> = filteredItems
        .map { list -> list.sumOf { it.quantity } }
        .stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), 0)

    val grandTotal: StateFlow<Double> = filteredItems
        .map { list ->
            val subtotal = list.sumOf { it.price * it.quantity }
            val tax = subtotal * 0.15
            subtotal + tax
        }
        .stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), 0.0)


    init {
        loadItems()
    }

    private fun loadItems() {
        viewModelScope.launch {
            try {
                val result = repository.getPurchaseOrderItems(poNumber)
                _items.value = result
            } catch (e: Exception) {
                println("Error loading PO items: ${e.message}")
            }
        }
    }

    companion object {
        fun factory(poNumber: String) = viewModelFactory {
            initializer {
                val app = (this[APPLICATION_KEY] as MJRecordsApplication)
                val repo = app.container.poRepository
                PODetailViewModel(poNumber, repo)
            }
        }
    }
}
