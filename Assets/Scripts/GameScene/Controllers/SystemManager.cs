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
    [SerializeField] GameObject boidPrefab;
    [SerializeField] GameObject localCenter;
    [SerializeField] GameObject alignmentArrow;
    [SerializeField] GameObject separationArrow;

    [Inject] new Camera camera;
    [Inject] SaveService saveService;
    [Inject] PredatorController predatorController;

    GameSettings gameSettings;
    BoidSettings boidSettings;
    PredatorSettings predatorSettings;
    Dictionary<Type, Systems.System> systemDict;
    Systems.System[] systemArray;
    Vector3 cameraBottomLeft;
    Vector3 cameraTopRight;
    Predator predator;

    [HideInInspector] public List<Boid> boids;

    void Awake() {
        gameSettings = saveService.getSave().settings;
        boidSettings = gameSettings.boidSettings;
        predatorSettings = gameSettings.predatorSettings;
        boids = new List<Boid>(boidSettings.count);
        systemDict = new Dictionary<Type, Systems.System>();
        cameraBottomLeft = camera.getBottomLeft();
        cameraTopRight = camera.getTopRight();
        createBoids();
        createPredator();
        createSystems();
        initializeDebugViews();
    }

    void createBoids() {
        var boidSize = boidSettings.size;
        var viewAreaDiameter = 2 * boidSettings.viewDistance / boidSize;
        var viewAreaSize = new Vector3(viewAreaDiameter, viewAreaDiameter);
        for (var i = 0; i < boidSettings.count; i++) {
            var boid = Instantiate(boidPrefab).GetComponent<Boid>();
            boid.name = $"boid_{i}";
            // size
            boid.transform.localScale = new Vector3(boidSize, boidSize);
            // position and velocity
            randomizePositionAndVelocity(boid);
            // view area size
            boid.viewArea.transform.localScale = viewAreaSize;
            
            boids.Add(boid);
        }
        if (boids.Count == 0) {
            throw new Exception("no boid was created");
        }
    }

    void randomizePositionAndVelocity(Boid boid) {
        // position
        const float offset = 0.5f;
        boid.transform.position = new Vector3(
            RandomUtils.nextFloat(cameraBottomLeft.x + offset, cameraTopRight.x - offset),
            RandomUtils.nextFloat(cameraBottomLeft.y + offset, cameraTopRight.y - offset)
        );
        // direction
        boid.transform.rotation = Quaternion.Euler(0, 0, RandomUtils.nextFloat(0, 359));
        // speed
        boid.speed = RandomUtils.nextFloat(boidSettings.minSpeed, boidSettings.maxSpeed);
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
            new BorderSystem(boids, predator, camera.getBottomLeft(), camera.getTopRight(), boidSettings.size));
        systemDict.Add(typeof(FlockingSystem),
            new FlockingSystem(boids, gameSettings, boidSettings, localCenter, alignmentArrow, separationArrow));
        systemDict.Add(typeof(EvasionSystem), new EvasionSystem(boids, predator, gameSettings, camera));
        systemDict.Add(typeof(DragSystem), new DragSystem(boids, predator, gameSettings));
        systemDict.Add(typeof(SpeedSystem), new SpeedSystem(boids, boidSettings));
        systemDict.Add(typeof(BoidControlSystem), new BoidControlSystem(boids, gameSettings, camera));

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
        foreach (var boid in boids) randomizePositionAndVelocity(boid);
    }
}
}