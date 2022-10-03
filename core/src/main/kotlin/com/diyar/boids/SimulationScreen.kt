package com.diyar.boids

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.ScreenAdapter
import com.badlogic.gdx.graphics.OrthographicCamera
import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.scenes.scene2d.Stage
import com.badlogic.gdx.scenes.scene2d.ui.Label
import com.badlogic.gdx.utils.Array
import com.badlogic.gdx.utils.ScreenUtils
import com.badlogic.gdx.utils.viewport.FitViewport
import kotlin.random.Random

class SimulationScreen(private val core: Core): ScreenAdapter() {
    private var stage = Stage(FitViewport(WIDTH, HEIGHT), core.batch)
    private var camera = OrthographicCamera(WIDTH, HEIGHT)
    private var labelStyle = Label.LabelStyle(core.font, TEXT_COLOR)
    private val boids = Array<Boid>()

    init {
        repeat(10) {
            createBoid()
        }
    }

    private fun createBoid() {
        val boid = Boid(Texture(Gdx.files.internal("square-16.png")))
        val x = Random.nextInt((WIDTH - boid.width).toInt()).toFloat()
        val y = Random.nextInt((HEIGHT - boid.height).toInt()).toFloat()
        boid.setPosition(x, y)
        stage.addActor(boid)
        boids.add(boid)
    }

    override fun render(delta: Float) {
        update(delta)
        draw()
    }

    private fun update(delta: Float) {
        stage.act(delta)
    }

    private fun draw() {
        ScreenUtils.clear(BACKGROUND_COLOR)
        camera.update()
        stage.draw()
    }

    override fun resize(width: Int, height: Int) {
        stage.viewport.update(width, height)
    }

    override fun dispose() {
        stage.dispose()
    }
}