package com.diyar.boids

import com.badlogic.gdx.Game
import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.g2d.BitmapFont
import com.badlogic.gdx.graphics.g2d.SpriteBatch

const val WIDTH = 1280f
const val HEIGHT = 720f
val BACKGROUND_COLOR: Color = Color.TEAL
val TEXT_COLOR: Color = Color.WHITE

class Core: Game() {
    lateinit var batch: SpriteBatch
    lateinit var font: BitmapFont

    override fun create() {
        batch = SpriteBatch()
        font = BitmapFont()
        setScreen(SimulationScreen(this))
    }

    override fun dispose() {
        batch.dispose()
        font.dispose()
    }
}
