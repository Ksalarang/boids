package com.diyar.boids

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.ScreenAdapter
import com.badlogic.gdx.graphics.Color
import com.badlogic.gdx.graphics.OrthographicCamera
import com.badlogic.gdx.scenes.scene2d.InputEvent
import com.badlogic.gdx.scenes.scene2d.Stage
import com.badlogic.gdx.scenes.scene2d.ui.Label
import com.badlogic.gdx.scenes.scene2d.ui.Label.LabelStyle
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener
import com.badlogic.gdx.utils.Align
import com.badlogic.gdx.utils.Align.center
import com.badlogic.gdx.utils.ScreenUtils
import com.badlogic.gdx.utils.viewport.FitViewport

class MainMenu(private val core: Core): ScreenAdapter() {
    private var stage = Stage(FitViewport(WIDTH, HEIGHT), core.batch)
    private var camera = OrthographicCamera(WIDTH, HEIGHT)
    private var labelStyle = LabelStyle(core.font, TEXT_COLOR)
    private lateinit var startSimLabel: Label

    //region init
    init {
        Gdx.input.inputProcessor = stage
        createStartSimLabel()
    }

    private fun createStartSimLabel() {
        startSimLabel = Label("Start", labelStyle).apply {
            setPosition(WIDTH / 2f, HEIGHT / 2f, center)
            addListener(object: ClickListener() {
                override fun clicked(event: InputEvent?, x: Float, y: Float) {
                    core.screen = SimulationScreen(core)
                }
            })
        }
        stage.addActor(startSimLabel)
    }
    //endregion

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