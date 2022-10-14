package com.diyar.boids

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.ScreenAdapter
import com.badlogic.gdx.graphics.OrthographicCamera
import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.graphics.g2d.TextureRegion
import com.badlogic.gdx.scenes.scene2d.Stage
import com.badlogic.gdx.scenes.scene2d.ui.Label
import com.badlogic.gdx.utils.Array
import com.badlogic.gdx.utils.ScreenUtils
import com.badlogic.gdx.utils.viewport.FitViewport
import com.diyar.boids.WorldBoundingMode.BounceOff
import com.diyar.boids.WorldBoundingMode.WrapAround

class SimulationScreen(private val core: Core): ScreenAdapter() {
    private val boidAmount = 20
    private var stage = Stage(FitViewport(WIDTH, HEIGHT), core.batch)
    private var camera = OrthographicCamera(WIDTH, HEIGHT)
    private var labelStyle = Label.LabelStyle(core.font, TEXT_COLOR)
    private val boids = Array<Boid>()
    private val boidTexture = Texture(Gdx.files.internal("boid-16.png"))

    init {
        Boid.setWorldBounds(WIDTH, HEIGHT)
//        Boid.worldBoundingMode = BounceOff
        repeat(boidAmount) {
            spawnBoid()
        }
    }

    private fun spawnBoid() {
        val boid = Boid(TextureRegion(boidTexture))
        val x = randomFloat(0f, WIDTH - boid.width)
        val y = randomFloat(0f, HEIGHT - boid.height)
        boid.setPosition(x, y)
        boid.setVelocity(
            randomFloatWithNegative(10, 200),
            randomFloatWithNegative(10, 200)
        )
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
        boidTexture.dispose()
    }
}