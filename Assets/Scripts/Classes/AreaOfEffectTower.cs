using UnityEngine;
using System;

public class AreaOfEffectTower: Tower {

    void Start() {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMasks, towerRotationPoint, towerFiringPoint, towerPrefab, 
        rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);
    }


    public override void OnMouseDown() {
        base.OnMouseDown();
    }

    
    public override void updateMethod() {
        
    }


    public override void findTargets() {

    }

    public override void attack() {

    }

    public override void upgrade() {

    }
}