using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections.Generic;


public class AreaOfEffectTower : Tower
{

    [Header("Upgrade JSON")]
    [SerializeField] TextAsset upgradeJson;
    TowerPathUpgrades upgradeData;

    // TODO: set this only to true when it is unlocked
    bool burnEffectUnlocked = true;

    void Start()
    {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMask, targetPrio, towerBaseCollider, towerRotationPoint, towerFiringPoint, shootingParticlePrefab, towerPrefab,
        rangeIndicator, rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);

        //When Start, set CircleCollider Range for Particles so that they match the actual tower range
        setParticleColliderRadius(base.baseTargetingRange);

        this.upgradeData = JsonUtility.FromJson<TowerPathUpgrades>(upgradeJson.text);
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }


    public override void updateMethod()
    {

    }

    public override void attack()
    {
        Instantiate(base.shootingParticlePrefab, base.towerRotationPoint.position, this.towerRotationPoint.rotation);
        if (base.enemyTargets == null || base.enemyTargets.Count == 0) return;
        //Make a Copy because maybe an Enemy will exit the range right in the time when we iterate the list
        List<Transform> targetsCopy = new List<Transform>(base.enemyTargets);
        foreach (Transform target in targetsCopy)
        {
            if (target == null) continue;
            Enemy enemy = target.gameObject.GetComponent<Enemy>();
            enemy.takeDamage(base.currentDMG);

            if (burnEffectUnlocked){
                enemy.ApplyBurnEffect(new BurnEffect(5, 1, 1));
            }
        }
    }

    public override void upgrade(UpgradePath path)
    {

        //Which path
        Upgrades upgradeData = path == UpgradePath.PathA ? this.upgradeData.PathA : this.upgradeData.PathB;

        //When range Upgrade, increase CircleCollider Range for Particles so that they match the actual tower range
        float newParticleRange = upgradeData.upgrades[base.currentLevel + 1 > 2 ? 2 : base.currentLevel].range;
        setParticleColliderRadius(newParticleRange);

        base.applyUpgrade(upgradeData, path);

    }

    // increase CircleCollider Range for Particles so that they match the actual tower range
    private void setParticleColliderRadius(float r)
    {
        CircleCollider2D collider = base.shootingParticlePrefab.GetComponent<CircleCollider2D>();
        collider.radius = r;
    }
}

[Serializable]
public class BurnEffect
{
    public double duration;
    public int damage;
    public double timeBetweenBurns;

    public BurnEffect(double duration, int damage, double timeBetweenBurns)
    {
        this.duration = duration;
        this.damage = damage;
        this.timeBetweenBurns = timeBetweenBurns;
    }
}
