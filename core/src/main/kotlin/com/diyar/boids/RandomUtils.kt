package com.diyar.boids

import kotlin.random.Random

fun randomInt(from: Int, to: Int): Int {
    return from + Random.nextInt(to)
}

fun randomFloat(from: Int, to: Int): Float {
    return (from + Random.nextInt(to)).toFloat()
}

fun randomFloat(from: Float, to: Float): Float {
    return (from.toInt() + Random.nextInt(to.toInt())).toFloat()
}

fun randomIntWithNegative(from: Int, to: Int): Int {
    val random = randomInt(from, to)
    return if (Random.nextBoolean()) random else -random
}

fun randomFloatWithNegative(from: Int, to: Int): Float {
    val random = randomFloat(from, to)
    return if (Random.nextBoolean()) random else -random
}