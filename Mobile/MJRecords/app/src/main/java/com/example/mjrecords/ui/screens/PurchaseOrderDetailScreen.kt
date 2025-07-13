package com.example.mjrecords.ui.screens

import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material.icons.Icons
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import com.example.mjrecords.ui.viewmodels.PODetailViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun PurchaseOrderDetailScreen(
    poNumber: String,
    supervisorName: String,
    poStatus: Int,
    navController: NavController
) {
    val viewModel: PODetailViewModel=
        viewModel(factory = PODetailViewModel.factory(poNumber))

    val totalItems by viewModel.totalItemCount.collectAsState()
    val grandTotal by viewModel.grandTotal.collectAsState()

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("PO #$poNumber Details") },
                navigationIcon = {
                    IconButton(onClick = { navController.popBackStack() }) {
                        Icon(
                            imageVector = Icons.AutoMirrored.Filled.ArrowBack,
                            contentDescription = "Back"
                        )
                    }
                }
            )
        }
    ) { innerPadding ->
        Column(
            modifier = Modifier
                .padding(innerPadding)
                .padding(16.dp)
        ) {
            Text("Supervisor: $supervisorName")
            Text("Status: ${mapStatus(poStatus)}")
            Text("Total Items: $totalItems (excluding removed items)")
            Text("Grand Total: $${String.format("%.2f", grandTotal)}")
        }
    }
}


