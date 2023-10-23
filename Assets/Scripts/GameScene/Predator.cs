using System.Collections.Generic;
using GameScene.Settings;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace GameScene {
public class Predator : MonoBehaviour {
    State state;
    Vector3 velocity;
    float speed;
    List<Boid> nearestBoids;
    Boid target;

    [HideInInspector] public new Transform transform;
    [HideInInspector] public Boid[] boids;
    [HideInInspector] public PredatorSettings settings;

    void Awake() {
        transform = base.transform;
        nearestBoids = new List<Boid>();
    }

    void Start() {
        state = State.Resting;
        speed = settings.restingSpeed;
    }

    void Update() {
        var delta = Time.deltaTime;
        updateRestProgress(delta);
        updateRotation(delta);
        updateMovement(delta);
        updateHuntingProgress();
    }

    float restProgress;

    void updateRestProgress(float delta) {
        if (state == State.Hunting) return;
        restProgress += delta;
        if (restProgress > settings.restPeriod) {
            restProgress = 0;
            state = State.Hunting;
            speed = settings.huntingSpeed;
        }
    }

    #region update rotation
    void updateRotation(float delta) {
        switch (state) {
            case State.Resting:
                break;
            case State.Hunting:
                rotateTowardsNearestBoid(delta);
                break;
        }
    }

    void rotateTowardsNearestBoid(float delta) {
        findNearestBoids();
        if (nearestBoids.Count == 0) return;
        nearestBoids.Sort((b1, b2) => b1.distanceTemp.CompareTo(b2.distanceTemp));
        target = nearestBoids[0];
        var direction = target.transform.position - transform.position;
        var angle = MathUtils.vectorToAngle(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            delta * settings.chaseSteeringSpeed);
    }

    void findNearestBoids() {
        nearestBoids.Clear();
        var position = transform.position;
        foreach (var boid in boids) {
            boid.distanceTemp = boid.transform.position.distanceTo(position);
            if (boid.distanceTemp < settings.viewDistance) {
                nearestBoids.Add(boid);
            }
        }
    }
    #endregion

    void updateMovement(float delta) {
        var angleRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        velocity.x = speed * Mathf.Cos(angleRadians);
        velocity.y = speed * Mathf.Sin(angleRadians);
        transform.position += velocity * delta;
    }

    void updateHuntingProgress() {
        if (state == State.Resting || target is null) return;
        var distance = transform.position.distanceTo(target.transform.position);
        if (distance < transform.localScale.x / 2 + target.transform.localScale.y / 2) {
            state = State.Resting;
            target = null;
            speed = settings.restingSpeed;
        }
    }

    enum State {
        Resting,
        Hunting,
    }
}
}