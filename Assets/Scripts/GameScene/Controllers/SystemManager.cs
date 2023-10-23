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

    GameSettings settings;
    Dictionary<Type, Systems.System> systemDict;
    Systems.System[] systemArray;
    Vector3 cameraBottomLeft;
    Vector3 cameraTopRight;
    Predator predator;

    [HideInInspector] public Boid[] boids;

    void Awake() {
        settings = saveService.getSave().settings;
        boids = new Boid[settings.count];
        systemDict = new Dictionary<Type, Systems.System>();
        cameraBottomLeft = camera.getBottomLeft();
        cameraTopRight = camera.getTopRight();
        createBoids();
        createPredator();
        createSystems();
        initializeDebugViews();
    }

    void createBoids() {
        var boidSize = settings.size;
        for (var i = 0; i < boids.Length; i++) {
            var boid = Instantiate(boidPrefab).GetComponent<Boid>();
            boid.name = $"boid_{i}";
            // size
            boid.transform.localScale = new Vector3(boidSize, boidSize);
            // position and direction
            randomizePositionAndDirection(boid);
            // view area size
            var viewAreaDiameter = 2 * settings.viewDistance / boidSize;
            boid.viewArea.transform.localScale = new Vector3(viewAreaDiameter, viewAreaDiameter);

            boids[i] = boid;
        }
        // set up the debug boid
        boids[0].GetComponent<SpriteRenderer>().color = Color.black;
    }

    void randomizePositionAndDirection(Boid boid) {
        // position
        boid.transform.position = new Vector3(
            RandomUtils.nextFloat(cameraBottomLeft.x, cameraTopRight.x),
            RandomUtils.nextFloat(cameraBottomLeft.y, cameraTopRight.y)
        );
        // rotation
        boid.transform.rotation = Quaternion.Euler(0, 0, RandomUtils.nextFloat(0, 359));
    }

    void createPredator() {
        predator = predatorController.createPredator(settings.predatorSettings);
    }

    void createSystems() {
        systemDict.Add(typeof(MovementSystem), new MovementSystem(boids, settings.speed));
        systemDict.Add(typeof(BorderSystem),
            new BorderSystem(boids, predator, camera.getBottomLeft(), camera.getTopRight(), settings.size));
        systemDict.Add(typeof(FlockingSystem),
            new FlockingSystem(boids, settings, localCenter, alignmentArrow, separationArrow));
        systemDict.Add(typeof(EvasionSystem), new EvasionSystem(boids, predator, settings));

        createSystemArray();
    }

    void createSystemArray() {
        systemArray = new Systems.System[systemDict.Count];
        var i = 0;
        foreach (var pair in systemDict) systemArray[i++] = pair.Value;
    }

    void initializeDebugViews() {
        localCenter.transform.localScale *= settings.size;
        alignmentArrow.transform.localScale *= settings.size;
        separationArrow.transform.localScale *= settings.size;
        onToggleLocalCenter(settings.showLocalCenter);
    }

    void Update() {
        var delta = Time.deltaTime * settings.gameSpeed;
        foreach (var system in systemArray) system.update(delta);
    }

    public void onToggleLocalCenter(bool value) {
        localCenter.gameObject.SetActive(value);
        alignmentArrow.gameObject.SetActive(value);
        separationArrow.gameObject.SetActive(value);
    }

    public void onRandomizeBoids() {
        foreach (var boid in boids) randomizePositionAndDirection(boid);
    }
}
}