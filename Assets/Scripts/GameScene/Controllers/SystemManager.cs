using System;
using System.Collections.Generic;
using GameScene.Systems;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace GameScene.Controllers {
public class SystemManager : MonoBehaviour {
    [SerializeField] GameObject boidPrefab;
    
    [Inject] new Camera camera;
    [Inject] GameSettings settings;

    public Boid[] boids;
    Dictionary<Type, Systems.System> systemDict;
    Systems.System[] systemArray;

    void Awake() {
        boids = new Boid[settings.flock.count];
        systemDict = new Dictionary<Type, Systems.System>();
        createBoids();
        createSystems();
    }

    void createBoids() {
        var bottomLeft = camera.getBottomLeft();
        var topRight = camera.getTopRight();
        var offset = 0.5f;
        var boidSize = settings.boids.size;
        for (var i = 0; i < boids.Length; i++) {
            var boid = Instantiate(boidPrefab).GetComponent<Boid>();
            boid.transform.localScale = new Vector3(boidSize, boidSize);
            // position
            boid.transform.position = new Vector3(
                RandomUtils.nextFloat(bottomLeft.x + offset, topRight.x - offset),
                RandomUtils.nextFloat(bottomLeft.y + offset, topRight.y - offset)
            );
            // rotation
            boid.transform.rotation = Quaternion.Euler(0, 0, RandomUtils.nextFloat(0, 359));
            boids[i] = boid;
        }
    }

    void createSystems() {
        systemDict.Add(typeof(MovementSystem), new MovementSystem(boids, settings.boids.speed));
        systemDict.Add(typeof(BorderSystem), new BorderSystem(boids, camera.getBottomLeft(), camera.getTopRight(), settings.boids.size));
        systemDict.Add(typeof(FlockingSystem), new FlockingSystem(boids, settings.flock));
        
        createSystemArray();
    }

    void createSystemArray() {
        systemArray = new Systems.System[systemDict.Count];
        var i = 0;
        foreach (var pair in systemDict) {
            systemArray[i++] = pair.Value;
        }
    }

    T getSystem<T>() where T : Systems.System {
        return (T) systemDict[typeof(T)];
    }

    void Update() {
        var delta = Time.deltaTime;
        foreach (var system in systemArray) {
            system.update(delta);
        }
    }
}
}