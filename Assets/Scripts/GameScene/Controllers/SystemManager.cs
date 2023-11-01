using System;
using System.Collections.Generic;
using GameScene.Settings;
using GameScene.Systems;
using Services.Saves;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace GameScene.Controllers {
public class SystemManager : MonoBehaviour {
    [SerializeField] float boidAreaFactor = 10f;
    [SerializeField] GameObject localCenter;
    [SerializeField] GameObject alignmentArrow;
    [SerializeField] GameObject separationArrow;

    // [Inject] new Camera camera;
    [Inject] SaveService saveService;
    [Inject] PredatorController predatorController;
    [Inject] BoidFactory boidFactory;

    Log log;
    GameSettings gameSettings;
    BoidSettings boidSettings;
    PredatorSettings predatorSettings;
    Dictionary<Type, Systems.System> systemDict;
    Systems.System[] systemArray;
    Predator predator;

    [HideInInspector] public List<Boid> boids;

    void Awake() {
        log = new Log(GetType());
        gameSettings = saveService.getSave().settings;
        boidSettings = gameSettings.boidSettings;
        predatorSettings = gameSettings.predatorSettings;
        boids = new List<Boid>(boidSettings.count);
        systemDict = new Dictionary<Type, Systems.System>();
    }

    void Start() {
        calculateWorldSize();
        createBoids();
        createPredator();
        createSystems();
        initializeDebugViews();
    }

    void calculateWorldSize() {
        var area = boidSettings.size * boidAreaFactor * boidSettings.count;
        if (area == 0) {
            throw new Exception("world area is 0");
        }
        var ratio = (float) Screen.width / Screen.height;
        var height = Mathf.Sqrt(area / ratio);
        var width = ratio * height;
        gameSettings.worldRect = new Rect(-width / 2, -height / 2, width, height);
        log.log($"world rect: {gameSettings.worldRect}, area: {area}, ratio: {ratio}");
    }

    void createBoids() {
        boids = boidFactory.createBoids();
    }

    void createPredator() {
        predator = predatorController.createPredator(gameSettings);
        var size = predatorSettings.size;
        predator.transform.localScale = new Vector3(size, size);
        var viewAreaSize = 2 * predatorSettings.viewDistance / size;
        predator.viewArea.transform.localScale = new Vector3(viewAreaSize, viewAreaSize);
        predator.gameObject.SetActive(boidSettings.predatorEvasionEnabled);
    }

    void createSystems() {
        systemDict.Add(typeof(MovementSystem), new MovementSystem(boids));
        systemDict.Add(typeof(BorderSystem),
            new BorderSystem(boids, predator, gameSettings));
        systemDict.Add(typeof(FlockingSystem),
            new FlockingSystem(boids, gameSettings, boidSettings, localCenter, alignmentArrow, separationArrow));
        systemDict.Add(typeof(EvasionSystem), new EvasionSystem(boids, predator, gameSettings));
        systemDict.Add(typeof(DragSystem), new DragSystem(boids, predator, gameSettings));
        systemDict.Add(typeof(SpeedSystem), new SpeedSystem(boids, boidSettings));
        // systemDict.Add(typeof(BoidControlSystem), new BoidControlSystem(boids, gameSettings, camera));

        createSystemArray();
    }

    void createSystemArray() {
        systemArray = new Systems.System[systemDict.Count];
        var i = 0;
        foreach (var pair in systemDict) systemArray[i++] = pair.Value;
    }

    void initializeDebugViews() {
        // scale views
        localCenter.transform.localScale *= boidSettings.size;
        alignmentArrow.transform.localScale *= boidSettings.size;
        separationArrow.transform.localScale *= boidSettings.size;
        // toggle views
        onToggleViewAreas(gameSettings.showViewAreas);
        onToggleBoidForces(gameSettings.showBoidForces);
        onTogglePredatorEvasion(boidSettings.predatorEvasionEnabled);
    }

    void Update() {
        var delta = Time.deltaTime * gameSettings.gameSpeed;
        foreach (var system in systemArray) system.update(delta);
    }

    public void onToggleViewAreas(bool value) {
        predator.viewArea.SetActive(value);
        foreach (var boid in boids) boid.viewArea.gameObject.SetActive(value);
    }

    public void onToggleBoidForces(bool value) {
        localCenter.gameObject.SetActive(value);
        alignmentArrow.gameObject.SetActive(value);
        separationArrow.gameObject.SetActive(value);
    }

    public void onTogglePredatorEvasion(bool value) {
        predator.gameObject.SetActive(value);
    }

    public void onRandomizeBoids() {
        foreach (var boid in boids) boidFactory.randomizePositionAndVelocity(boid);
    }

    public void onToggleTypeBasedFlocking(bool value) {
        for (var i = 0; i < boids.Count; i++) {
            boidFactory.setBoidSprite(boids[i], value, i, boids.Count);
        }
    }
}
}