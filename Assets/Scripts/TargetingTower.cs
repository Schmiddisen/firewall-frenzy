using UnityEngine;
using System;

public class TargetingTower: Tower {

    void Start() {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMasks, turretRotationPoint, turretFiringPoint, towerPrefab,
        rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);
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