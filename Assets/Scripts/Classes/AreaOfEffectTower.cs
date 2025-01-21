using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections.Generic;


public class AreaOfEffectTower: Tower {

    void Start() {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMask, targetPrio, towerBaseCollider, towerRotationPoint, towerFiringPoint, shootingParticlePrefab, towerPrefab, 
        rangeIndicator, rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);

        //When Start, set CircleCollider Range for Particles so that they match the actual tower range
        setParticleColliderRadius(base.baseTargetingRange);
    }


    public override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
    }
    public override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);
    }

    
    public override void updateMethod() {

    }

    public override void attack() {
        Instantiate(base.shootingParticlePrefab, base.towerRotationPoint.position, this.towerRotationPoint.rotation);
        if(base.enemyTargets == null || base.enemyTargets.Count == 0) return;
        //Make a Copy because maybe an Enemy will exit the range right in the time when we iterate the list
        List<Transform> targetsCopy = new List<Transform>(base.enemyTargets);
        foreach(Transform target in targetsCopy) {
            if (target == null) continue;
            target.gameObject.GetComponent<Enemy>().takeDamage(base.currentDMG);
        }
    }

    public override void upgrade() {
        //When range Upgrade, increase CircleCollider Range for Particles so that they match the actual tower range
        setParticleColliderRadius(0);

        //for range upgrades base.upgradeRange(x)
    }

    // increase CircleCollider Range for Particles so that they match the actual tower range
    private void setParticleColliderRadius(float r) {
        CircleCollider2D collider = base.shootingParticlePrefab.GetComponent<CircleCollider2D>();
        collider.radius = r;
    }
}