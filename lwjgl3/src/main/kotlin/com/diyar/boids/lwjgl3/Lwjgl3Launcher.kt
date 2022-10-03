@file:JvmName("Lwjgl3Launcher")

package com.diyar.boids.lwjgl3

import com.badlogic.gdx.backends.lwjgl3.Lwjgl3Application
import com.badlogic.gdx.backends.lwjgl3.Lwjgl3ApplicationConfiguration
import com.diyar.boids.Core
import com.diyar.boids.HEIGHT
import com.diyar.boids.WIDTH

/** Launches the desktop (LWJGL3) application. */
fun main() {
    Lwjgl3Application(Core(), Lwjgl3ApplicationConfiguration().apply {
        setTitle("boids")
        setWindowedMode(WIDTH.toInt(), HEIGHT.toInt())
        setForegroundFPS(60)
        useVsync(true)
        setWindowIcon(*(arrayOf(128, 64, 32, 16).map { "libgdx$it.png" }.toTypedArray()))
    })
}
