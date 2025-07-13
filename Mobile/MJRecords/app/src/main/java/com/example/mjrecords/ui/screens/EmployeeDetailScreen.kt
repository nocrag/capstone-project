package com.example.mjrecords.ui.screens

import android.content.Intent
import android.net.Uri
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import com.example.mjrecords.model.EmployeeSearchResultDto
import com.example.mjrecords.ui.viewmodels.EmployeeDetailViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun EmployeeDetailScreen(
    navController: NavController
) {
    val viewModel: EmployeeDetailViewModel = viewModel(factory = EmployeeDetailViewModel.Factory)

    val employee by viewModel.employee.collectAsState()
    val isLoading by viewModel.isLoading.collectAsState()
    val errorMessage by viewModel.errorMessage.collectAsState()
    val context = LocalContext.current
    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Employee Details") },
                navigationIcon = {
                    IconButton(onClick = { navController.popBackStack() }) {
                        Icon(Icons.Default.ArrowBack, contentDescription = "Back")
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
            when {
                isLoading -> CircularProgressIndicator()
                errorMessage != null -> Text(errorMessage!!, color = Color.Red)
                employee != null -> {
                    Text("Name: ${employee!!.firstName} ${employee!!.middleInitial} ${employee!!.lastName}")
                    Text("Home Mailing Address: ${employee!!.homeAddress}")
                    Text(
                        text = "Email: ${employee!!.workEmail}",
                        color = Color.Blue,
                        modifier = Modifier.clickable {
                            launchEmail(context, employee!!.workEmail)
                        },
                        textDecoration = TextDecoration.Underline
                    )

                    Text(
                        text = "Work Phone: ${employee!!.workPhone}",
                        color = Color.Blue,
                        modifier = Modifier.clickable {
                            launchDialer(context, employee!!.workPhone)
                        },
                        textDecoration = TextDecoration.Underline
                    )

                    Text(
                        text = "Cellphone: ${employee!!.cellPhone}",
                        color = Color.Blue,
                        modifier = Modifier.clickable {
                            launchDialer(context, employee!!.cellPhone)
                        },
                        textDecoration = TextDecoration.Underline
                    )
                }
            }
        }
    }
}

fun launchEmail(context: android.content.Context, email: String) {
    val intent = Intent(Intent.ACTION_SENDTO).apply {
        data = Uri.parse("mailto:$email")
    }
    context.startActivity(intent)
}

fun launchDialer(context: android.content.Context, number: String) {
    val intent = Intent(Intent.ACTION_DIAL).apply {
        data = Uri.parse("tel:$number")
    }
    context.startActivity(intent)
}

