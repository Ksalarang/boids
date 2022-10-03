package com.diyar.boids

import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.graphics.g2d.Batch
import com.badlogic.gdx.scenes.scene2d.Actor
import com.badlogic.gdx.utils.Align

class Boid(private val texture: Texture): Actor() {

    init {
        setSize(texture.width.toFloat(), texture.height.toFloat())
    }

    override fun draw(batch: Batch?, parentAlpha: Float) {
        batch?.draw(texture, x, y)
    }
}