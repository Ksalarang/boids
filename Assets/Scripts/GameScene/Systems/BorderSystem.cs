using Assets.Scripts.GameScene;
using UnityEngine;

namespace GameScene.Systems {
    public class BorderSystem : System {
        readonly Boid[] boids;
        readonly Predator predator;
        readonly Vector3 bottomLeft;
        readonly Vector3 topRight;
        readonly float halfSize;
        readonly float predatorHalfSize;
    
        public BorderSystem(Boid[] boids, Predator predator, Vector3 bottomLeft, Vector3 topRight, float boidSize) {
            this.boids = boids;
            this.predator = predator;
            this.bottomLeft = bottomLeft;
            this.topRight = topRight;
            halfSize = boidSize / 2;
            predatorHalfSize = predator.transform.localScale.x / 2;
        }
    
        public void update(float deltaTime) {
            foreach (var boid in boids) {
                var position = boid.transform.position;
                if (position.x + halfSize < bottomLeft.x) {
                    boid.transform.position = new Vector3(topRight.x, position.y, position.z);
                } else if (position.x - halfSize > topRight.x) {
                    boid.transform.position = new Vector3(bottomLeft.x, position.y, position.z);
                }
                if (position.y + halfSize < bottomLeft.y) {
                    boid.transform.position = new Vector3(position.x, topRight.y, position.z);
                } else if (position.y - halfSize > topRight.y) {
                    boid.transform.position = new Vector3(position.x, bottomLeft.y, position.z);
                }
            }
            updatePredator();
        }

        void updatePredator() {
            var position = predator.transform.position;
            if (position.x + predatorHalfSize < bottomLeft.x) {
                predator.transform.position = new Vector3(topRight.x, position.y, position.z);
            } else if (position.x - predatorHalfSize > topRight.x) {
                predator.transform.position = new Vector3(bottomLeft.x, position.y, position.z);
            }
            if (position.y + predatorHalfSize < bottomLeft.y) {
                predator.transform.position = new Vector3(position.x, topRight.y, position.z);
            } else if (position.y - predatorHalfSize > topRight.y) {
                predator.transform.position = new Vector3(position.x, bottomLeft.y, position.z);
            }
        }
    }
}