package com.example.mjrecords.ui.screens

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.navigation.NavHostController
import com.example.mjrecords.R
import com.example.mjrecords.ui.viewmodels.LoginViewModel


@Composable
fun HomeScreen(
    loginViewModel: LoginViewModel,
    navController: NavHostController
) {
    val loginResponse = loginViewModel.loginResponse.collectAsState().value

    Surface(
        modifier = Modifier.fillMaxSize(),
        color = Color.Black
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            if (loginResponse != null) {
                Image(
                    painter = painterResource(id = R.drawable.logo_white),
                    contentDescription = "MJ Logo",
                    modifier = Modifier
                        .size(180.dp)
                        .padding(bottom = 24.dp)
                )

                Text(
                    text = "Welcome, ${loginResponse.firstName} ${loginResponse.lastName}",
                    color = Color.White
                )
                Text(
                    text = "Role: ${loginResponse.role}",
                    color = Color.White
                )

                Spacer(modifier = Modifier.height(32.dp))

                Button(
                    onClick = {  navController.navigate("employee_directory") },
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Text("Browse Employee Directory")
                }

                Button(
                    onClick = {  navController.navigate("browse_departments")},
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(top = 16.dp)
                ) {
                    Text("Browse POs by department")
                }

                Button(
                    onClick = {
                        loginViewModel.logout()
                        navController.navigate("login") {
                            popUpTo("home") { inclusive = true }
                        }
                    },
                    colors = ButtonDefaults.buttonColors(containerColor = Color.Red),
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(top = 32.dp)
                ) {
                    Text("Logout", color = Color.White)
                }
            } else {
                Text("User data not available", color = Color.White)
            }
        }
    }
}















//    Column(
//        verticalArrangement = Arrangement.Center,
//        horizontalAlignment = Alignment.CenterHorizontally
//    ) {
//        if (loginResponse != null) {
//            Text(text = "Welcome, ${loginResponse!!.firstName} ${loginResponse!!.lastName}")
//            Text(text = "Role: ${loginResponse!!.role}")
//            Text(text = "Token: ${loginResponse!!.token}")
//        } else {
//            Text(text = "User data not available")
//        }
//    }
//}