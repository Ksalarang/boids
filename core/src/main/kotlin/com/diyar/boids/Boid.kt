package com.diyar.boids

import com.badlogic.gdx.graphics.g2d.Batch
import com.badlogic.gdx.graphics.g2d.TextureRegion
import com.badlogic.gdx.math.MathUtils
import com.badlogic.gdx.math.Rectangle
import com.badlogic.gdx.math.Vector2
import com.badlogic.gdx.scenes.scene2d.Actor
import com.diyar.boids.WorldBoundingMode.*

const val ROTATION_CONSTANT = 57.295776f

class Boid(private val textureRegion: TextureRegion): Actor() {
    private val velocity = Vector2(0f, 0f)
    private var rightX = x + width
    private var upperY = y + height

    init {
        setSize(textureRegion.regionWidth.toFloat(), textureRegion.regionHeight.toFloat())
    }

    fun setVelocity(x: Float, y: Float) {
        velocity.set(x, y)
    }

    //region override
    override fun act(delta: Float) {
        updateMotionAngle()
        when (worldBoundingMode) {
            NoBound -> {}
            WrapAround -> wrapAroundWorld()
            BounceOff -> bounceOffWalls()
        }
        moveBy(velocity.x * delta, velocity.y * delta)
        super.act(delta)
    }

    override fun draw(batch: Batch?, parentAlpha: Float) {
        batch?.draw(textureRegion, x, y, originX, originY, width, height, scaleX, scaleY, rotation)
    }

    override fun sizeChanged() {
        rightX = x + width
        upperY = y + height
    }
    //endregion

    private fun updateMotionAngle() {
        rotation = MathUtils.atan2(velocity.y, velocity.x) * ROTATION_CONSTANT
    }

    private fun wrapAroundWorld() {
        if (rightX <= 0) {
            x = worldBounds.width
        }
        if (x > worldBounds.width) {
            x = -width
        }
        if (upperY <= 0) {
            y = worldBounds.height
        }
        if (y > worldBounds.height) {
            y = -height
        }
    }

    private fun bounceOffWalls() {
        if (x < 0) {

        }
    }

    companion object {
        private val worldBounds = Rectangle()
        var worldBoundingMode = NoBound

        fun setWorldBounds(width: Float, height: Float) {
            worldBounds.set(0f, 0f, width, height)
        }
    }
}