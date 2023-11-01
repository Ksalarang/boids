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

    GameSettings gameSettings;
    BoidSettings boidSettings;
    Vector3 cameraBottomLeft;
    Vector3 cameraTopRight;
    int boidCounter;
    FishType[] colorEnums;

    void Awake() {
        gameSettings = saveService.getSave().settings;
        boidSettings = gameSettings.boidSettings;
        colorEnums = (FishType[]) Enum.GetValues(typeof(FishType));
    }

    void Start() {
        cameraBottomLeft = gameSettings.worldRect.min;
        cameraTopRight = gameSettings.worldRect.max;
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
            setBoidSprite(boid, boidSettings.typeBasedFlockingEnabled, i, boidCount);
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

    public void setBoidSprite(Boid boid, bool typeBasedFlockingEnabled, int index, int boidCount) {
        boid.fishType = typeBasedFlockingEnabled ? getColorForBoidIndex(index, boidCount) : FishType.Blue;
        boid.GetComponent<SpriteRenderer>().sprite = getSpriteForColor(boid.fishType);
    }
    
    FishType getColorForBoidIndex(int index, int count) {
        var colorGroupCount = count / colorEnums.Length;
        if (colorGroupCount < colorEnums.Length) return FishType.Blue;
        
        for (var i = 0; i < colorEnums.Length; i++) {
            if (index < colorGroupCount * (i + 1)) return colorEnums[i];
        }
        return FishType.Blue;
    }

    Sprite getSpriteForColor(FishType fishType) {
        return fishType switch {
            FishType.Blue => blueFishSprite,
            FishType.Green => greenFishSprite,
            FishType.Yellow => yellowFishSprite,
            FishType.Orange => orangeFishSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(fishType), fishType, null)
        };
    }
}

public enum FishType {
    Blue, Green, Yellow, Orange,
}
}