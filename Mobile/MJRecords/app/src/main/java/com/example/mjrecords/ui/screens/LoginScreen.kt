package com.example.mjrecords.ui.screens

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.mjrecords.ui.viewmodels.LoginViewModel


@Composable
fun LoginScreen(
    loginViewModel: LoginViewModel = viewModel(factory = LoginViewModel.Factory),
    onSuccessfulLogin: () -> Unit,
    modifier: Modifier = Modifier) {

    val uiState by loginViewModel.uiState.collectAsStateWithLifecycle()

//    LaunchedEffect(uiState.isLoggedIn) {
//        if (uiState.isLoggedIn) {
//            onSuccessfulLogin()
//        }
//    }

    LaunchedEffect(Unit) {
        loginViewModel.loginSuccessEvent.collect {
            onSuccessfulLogin()
        }
    }

    Column(
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally,
        modifier = modifier
            .fillMaxSize()
    ) {
        OutlinedTextField(
            value = uiState.username,
            onValueChange = {
                loginViewModel.onUsernameChanged(it)
            },
            label = {
                Text(text = "Username")
            },
            singleLine = true,
            isError = !uiState.errorMessage.isNullOrEmpty(),
            modifier = Modifier.fillMaxWidth()
        )
        Spacer(
            modifier = Modifier.height(8.dp)
        )
        OutlinedTextField(
            value = uiState.password,
            onValueChange = {
                loginViewModel.onPasswordChanged(it)
            },
            label = {
                Text(text = "Password")
            },
            visualTransformation = PasswordVisualTransformation(),
            singleLine = true,
            isError = !uiState.errorMessage.isNullOrEmpty(),
            modifier = Modifier.fillMaxWidth()
        )
        Spacer(
            modifier = Modifier.height(8.dp)
        )

        if (!uiState.errorMessage.isNullOrEmpty()) {
            Text(text = uiState.errorMessage!!, color = androidx.compose.ui.graphics.Color.Red)
            Spacer(modifier = Modifier.height(8.dp))
        }
        Button(
            onClick = {
                loginViewModel.login()
            },
            enabled = !uiState.isLoading
        ) {
            if (uiState.isLoading) {
                CircularProgressIndicator(
                    strokeWidth = 2.dp
                )
            } else {
                Text(text = "Login")
            }
        }
    }
}