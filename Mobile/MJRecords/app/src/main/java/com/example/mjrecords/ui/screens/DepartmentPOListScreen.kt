package com.example.mjrecords.ui.screens


import android.net.Uri
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import com.example.mjrecords.ui.viewmodels.DepartmentPOListViewModel
import com.example.mjrecords.utility.formatDate


@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun DepartmentPOListScreen(departmentId: Int, departmentName: String, navController: NavController) {
    val viewModel: DepartmentPOListViewModel =
        viewModel(factory = DepartmentPOListViewModel.factory(departmentId))

    val poList by viewModel.poList.collectAsState()

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("POs for Department $departmentName") },
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
            if (poList.isEmpty()) {
                Text("No purchase orders found.")
            } else {
                LazyColumn {
                    items(poList) { po ->
                        Card(
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(vertical = 4.dp)
                        ) {
                            Column(
                                modifier = Modifier
                                    .padding(16.dp)
                                    .clickable {
                                    val encodedName = Uri.encode(po.supervisorName)
                                    navController.navigate("purchase_order/${po.poNumber}/$encodedName/${po.poStatus}")
                                })

                            {
                                Text("PO #: ${po.poNumber}", style = MaterialTheme.typography.titleMedium)
                                Text("Created at: ${formatDate(po.poCreationDate)}")
                                Text("Supervisor: ${po.supervisorName}")
                                Text("Status: ${mapStatus(po.poStatus)}")
                            }
                        }
                    }
                }
            }
        }
    }
}

fun mapStatus(status: Int): String {
    return when (status) {
        0 -> "Pending"
        1 -> "Under Review"
        2 -> "Closed"
        else -> "Unknown"
    }
}
