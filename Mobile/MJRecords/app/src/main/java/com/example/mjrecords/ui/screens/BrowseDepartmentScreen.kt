package com.example.mjrecords.ui.screens

import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import com.example.mjrecords.model.Department
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import com.example.mjrecords.ui.viewmodels.DepartmentSearchViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun BrowseDepartmentScreen(
    navController: NavController,
    viewModel: DepartmentSearchViewModel = viewModel(factory = DepartmentSearchViewModel.Factory)
) {
    var expanded by remember { mutableStateOf(false) }
    var selectedDepartment by remember { mutableStateOf<Department?>(null) }


    val departmentList by viewModel.departments.collectAsState()

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Browse POs by Department") },
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
                .fillMaxSize()
        ) {
            ExposedDropdownMenuBox(
                expanded = expanded,
                onExpandedChange = { expanded = !expanded }
            ) {
                OutlinedTextField(
                    readOnly = true,
                    value = selectedDepartment?.name ?: "",
                    onValueChange = {},
                    label = { Text("Select Department") },
                    trailingIcon = {
                        ExposedDropdownMenuDefaults.TrailingIcon(expanded = expanded)
                    },
                    modifier = Modifier
                        .menuAnchor()
                        .fillMaxWidth()
                )

                ExposedDropdownMenu(
                    expanded = expanded,
                    onDismissRequest = { expanded = false }
                ) {
                    departmentList.forEach { department ->
                        DropdownMenuItem(
                            text = { Text(department.name) },
                            onClick = {
                                selectedDepartment = department
                                expanded = false
                                navController.navigate("department_pos/${department.id}/${department.name}")

                            }
                        )
                    }
                }
                if (departmentList.isEmpty()) {
                    Text("No departments available", color = MaterialTheme.colorScheme.error)
                }
            }
        }
    }
}
