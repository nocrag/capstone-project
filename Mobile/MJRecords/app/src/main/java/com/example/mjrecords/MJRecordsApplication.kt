package com.example.mjrecords

import android.app.Application
import com.example.mjrecords.data.AppContainer
import com.example.mjrecords.data.DefaultAppContainer

class MJRecordsApplication: Application() {
    lateinit var container: AppContainer
    override fun onCreate() {
        super.onCreate()
        container = DefaultAppContainer(applicationContext)
    }
}