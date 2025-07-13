package com.example.mjrecords.ui.screens

import androidx.compose.foundation.clickable
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.navigation.NavController
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material.icons.Icons
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.mjrecords.model.Department
import com.example.mjrecords.ui.viewmodels.DepartmentSearchViewModel
import com.example.mjrecords.ui.viewmodels.EmployeeSearchViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun BrowseEmployeeScreen(
    viewModel: EmployeeSearchViewModel = viewModel(factory = EmployeeSearchViewModel.Factory),
    dptVM: DepartmentSearchViewModel = viewModel(factory = DepartmentSearchViewModel.Factory),
    navController: NavController
) {
    val departments by dptVM.departments.collectAsState()
    val searchDto by viewModel.searchDto.collectAsState()
    val searchResults by viewModel.searchResults.collectAsState()
    val isLoading by viewModel.isLoading.collectAsState()
    val errorMessage by viewModel.errorMessage.collectAsState()

    var expanded by remember { mutableStateOf(false) }
    var selectedDepartment by remember { mutableStateOf<Department?>(null) }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Employee Directory") },
                navigationIcon = {
                    IconButton(onClick = { navController.popBackStack() }) {
                        Icon(Icons.AutoMirrored.Filled.ArrowBack, contentDescription = "Back")
                    }
                }
            )
        }
    ) { innerPadding ->
        Column(
            modifier = Modifier
                .padding(innerPadding)
                .padding(16.dp)
                .verticalScroll(rememberScrollState())
        ) {
            OutlinedTextField(
                value = searchDto.employeeId,
                onValueChange = {
                    viewModel.updateSearchDto(searchDto.copy(employeeId = it))
                },
                label = { Text("Employee ID") },
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(modifier = Modifier.height(8.dp))

            OutlinedTextField(
                value = searchDto.lastName,
                onValueChange = {
                    viewModel.updateSearchDto(searchDto.copy(lastName = it))
                },
                label = { Text("Last Name") },
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(modifier = Modifier.height(8.dp))

            ExposedDropdownMenuBox(
                expanded = expanded,
                onExpandedChange = { expanded = !expanded }
            ) {
                OutlinedTextField(
                    readOnly = true,
                    value = selectedDepartment?.name ?: "",
                    onValueChange = {},
                    label = { Text("Select Department") },
                    trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(expanded) },
                    modifier = Modifier
                        .menuAnchor()
                        .fillMaxWidth()
                )

                ExposedDropdownMenu(
                    expanded = expanded,
                    onDismissRequest = { expanded = false }
                ) {
                    departments.forEach { department ->
                        DropdownMenuItem(
                            text = { Text(department.name) },
                            onClick = {
                                selectedDepartment = department
                                expanded = false
                                viewModel.updateSearchDto(searchDto.copy(departmentId = department.id))
                            }
                        )
                    }
                }
            }

            Spacer(modifier = Modifier.height(12.dp))

            Button(
                onClick = { viewModel.searchEmployee() },
                modifier = Modifier.fillMaxWidth()
            ) {
                Text("Search")
            }

            Spacer(modifier = Modifier.height(16.dp))

            if (isLoading) {
                CircularProgressIndicator(modifier = Modifier.align(Alignment.CenterHorizontally))
            }

            if (errorMessage != null) {
                Text(
                    text = errorMessage!!,
                    color = MaterialTheme.colorScheme.error,
                    style = MaterialTheme.typography.bodyMedium
                )
            }

            searchResults.forEach { employee ->
                Card(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 4.dp)
                        .clickable {
                            navController.navigate("employee_detail/${employee.id}")
                        }
                ) {
                    Column(modifier = Modifier.padding(12.dp)) {
                        Text("Name: ${employee.lastName}, ${employee.firstName}")
                        Text("Position: ${employee.position}")
                    }
                }
            }
        }
    }
}

