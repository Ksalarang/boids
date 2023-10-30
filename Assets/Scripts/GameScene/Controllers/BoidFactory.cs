using System;
using System.Collections.Generic;
using GameScene.Settings;
using Services.Saves;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Zenject;

namespace GameScene.Controllers {
public class BoidFactory : MonoBehaviour {
    [SerializeField] float positionOffset;
    [SerializeField] GameObject boidPrefab;
    [SerializeField] Sprite blueFishSprite;
    [SerializeField] Sprite greenFishSprite;
    [SerializeField] Sprite yellowFishSprite;
    [SerializeField] Sprite orangeFishSprite;

    [Inject] SaveService saveService;
    [Inject] new Camera camera;

    BoidSettings boidSettings;
    Vector3 cameraBottomLeft;
    Vector3 cameraTopRight;
    int boidCounter;
    FishColor[] colorEnums;

    void Awake() {
        var gameSettings = saveService.getSave().settings;
        boidSettings = gameSettings.boidSettings;
        cameraBottomLeft = camera.getBottomLeft();
        cameraTopRight = camera.getTopRight();
        colorEnums = (FishColor[]) Enum.GetValues(typeof(FishColor));
    }

    public List<Boid> createBoids() {
        var boids = new List<Boid>();
        var boidSize = boidSettings.size;
        var viewAreaDiameter = 2 * boidSettings.viewDistance / boidSize;
        var viewAreaSize = new Vector3(viewAreaDiameter, viewAreaDiameter);
        var boidCount = boidSettings.count;
        for (var i = 0; i < boidCount; i++) {
            var boid = Instantiate(boidPrefab).GetComponent<Boid>();
            boid.name = $"boid_{++boidCounter}";
            // sprite
            var color = boidSettings.colorfulModeEnabled ? getColorForBoidIndex(i, boidCount) : FishColor.Blue;
            boid.GetComponent<SpriteRenderer>().sprite = getSpriteForColor(color);
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
        return boids;
    }

    FishColor getColorForBoidIndex(int index, int count) {
        var colorGroupCount = count / colorEnums.Length;
        if (colorGroupCount < colorEnums.Length) return FishColor.Blue;
        
        for (var i = 0; i < colorEnums.Length; i++) {
            if (index < colorGroupCount * (i + 1)) return colorEnums[i];
        }
        return FishColor.Blue;
    }

    Sprite getSpriteForColor(FishColor color) {
        return color switch {
            FishColor.Blue => blueFishSprite,
            FishColor.Green => greenFishSprite,
            FishColor.Yellow => yellowFishSprite,
            FishColor.Orange => orangeFishSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public void randomizePositionAndVelocity(Boid boid) {
        // position
        boid.transform.position = new Vector3(
            RandomUtils.nextFloat(cameraBottomLeft.x + positionOffset, cameraTopRight.x - positionOffset),
            RandomUtils.nextFloat(cameraBottomLeft.y + positionOffset, cameraTopRight.y - positionOffset)
        );
        // direction
        boid.transform.rotation = Quaternion.Euler(0, 0, RandomUtils.nextFloat(0, 359));
        // speed
        boid.speed = RandomUtils.nextFloat(boidSettings.minSpeed, boidSettings.maxSpeed);
    }
}

public enum FishColor {
    Blue, Green, Yellow, Orange,
}
}