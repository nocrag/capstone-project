package com.example.mjrecords.ui

import androidx.compose.foundation.layout.padding
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Modifier
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import androidx.navigation.NavType
import com.example.mjrecords.ui.screens.BrowseDepartmentScreen
import com.example.mjrecords.ui.screens.BrowseEmployeeScreen
import com.example.mjrecords.ui.screens.DepartmentPOListScreen
import com.example.mjrecords.ui.screens.EmployeeDetailScreen
import com.example.mjrecords.ui.screens.HomeScreen
import com.example.mjrecords.ui.screens.LoginScreen
import com.example.mjrecords.ui.screens.PurchaseOrderDetailScreen
import com.example.mjrecords.ui.viewmodels.EmployeeSearchViewModel
import com.example.mjrecords.ui.viewmodels.LoginViewModel


@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainApp(
    navController: NavHostController = rememberNavController()
) {
    val loginViewModel: LoginViewModel = viewModel(factory = LoginViewModel.Factory)
    val isLoggedIn = loginViewModel.uiState.collectAsState().value.isLoggedIn

    val startDestination: String

    if (isLoggedIn) {
        startDestination = "home"
    } else {
        startDestination = "login"
    }


    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Text("MJRecords App")
                }
            )
        }
    ) { innerPadding ->
        NavHost(
            navController = navController,
            startDestination = startDestination,
            modifier = Modifier.padding(innerPadding)
        ) {
            composable(route = "login") {
                LoginScreen(
                    loginViewModel = loginViewModel,
                    onSuccessfulLogin = {
                        navController.navigate("home") {
                            popUpTo("login") { inclusive = true }
                        }
                    }
                )
            }
            composable(route = "home") {
                val isLoggedIn = loginViewModel.uiState.collectAsState().value.isLoggedIn

                LaunchedEffect(isLoggedIn) {
                    if (!isLoggedIn) {
                        navController.navigate("login") {
                            popUpTo("home") { inclusive = true }
                        }
                    }
                }

                HomeScreen(
                    loginViewModel = loginViewModel,
                    navController = navController
                )
            }

            composable("browse_departments") {
                BrowseDepartmentScreen(navController = navController)
            }


            composable(
                route = "department_pos/{departmentId}/{departmentName}",
                arguments = listOf(navArgument("departmentId") { type = NavType.IntType })
            ) { backStackEntry ->
                val departmentId = backStackEntry.arguments?.getInt("departmentId") ?: -1
                val departmentName = backStackEntry.arguments?.getString("departmentName") ?: ""
                DepartmentPOListScreen(
                    departmentId = departmentId,
                    departmentName = departmentName,
                    navController = navController)
            }


            composable("employee_directory") {
                BrowseEmployeeScreen(navController = navController)
            }

            composable(
                route = "employee_detail/{employeeId}",
                arguments = listOf(navArgument("employeeId") { type = NavType.StringType })
            ) {
                EmployeeDetailScreen(navController = navController)
            }

//            composable(
//                route = "employee_detail/{employeeId}",
//                arguments = listOf(navArgument("employeeId") { type = NavType.StringType })
//            ) { backStackEntry ->
//                val employeeId = backStackEntry.arguments?.getString("employeeId")
//                // Use shared ViewModel to fetch employee by ID
//                val viewModel: EmployeeSearchViewModel = viewModel(factory = EmployeeSearchViewModel.Factory)
//                val employee = viewModel.getEmployeeById(employeeId)
//                if (employee != null) {
//                    EmployeeDetailScreen(employee = employee, navController = navController)
//                } else {
//                    Text("Employee not found.")
//                }
//            }

            composable(
                route = "purchase_order/{poNumber}/{supervisorName}/{poStatus}",
                arguments = listOf(
                    navArgument("poNumber") { type = NavType.StringType },
                    navArgument("supervisorName") { type = NavType.StringType },
                    navArgument("poStatus") { type = NavType.IntType }
                )
            ) { backStackEntry ->
                val poNumber = backStackEntry.arguments?.getString("poNumber") ?: ""
                val supervisorName = backStackEntry.arguments?.getString("supervisorName") ?: ""
                val poStatus = backStackEntry.arguments?.getInt("poStatus") ?: -1

                PurchaseOrderDetailScreen(
                    poNumber = poNumber,
                    supervisorName = supervisorName,
                    poStatus = poStatus,
                    navController = navController
                )
            }



        }
    }
}
