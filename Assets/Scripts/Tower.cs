using UnityEngine;
using System;

[System.Serializable]
public abstract class Tower : MonoBehaviour
{
    [Header("References")]
    public LayerMask[] enemyMasks;
    public Transform turretRotationPoint;
    public Transform turretFiringPoint;

    public GameObject towerPrefab;
    private Transform[] enemyTargets;

    [Header("Attributes")]

    public float rotationSpeed;
    public int baseUpgradeCosts;
    private int currentUpgradeCosts;
    public int buildCost;
    private int currentLevel = 1;

    public float baseTargetingRange;
    private float currentTargetingRange;

    public int baseDMG;
    private int currentDMG;

    public float baseAPS;
    private float currentAPS;

    private float timeUntilFire = 0;

    public string name;

    public void setupTower(LayerMask[] enemyMasks, Transform turretRotationPoint, Transform turretFiringPoint, 
                            GameObject towerPrefab, float rotationSpeed, int baseUpgradeCosts, int buildCost, float baseTargetingRange, 
                            int baseDMG, float baseAPS, string name)
    {
        this.enemyMasks = enemyMasks;
        this.turretRotationPoint = turretRotationPoint;
        this.turretFiringPoint = turretFiringPoint;
        this.rotationSpeed = rotationSpeed;
        this.baseUpgradeCosts = baseUpgradeCosts;
        this.buildCost = buildCost;
        this.baseTargetingRange = baseTargetingRange;
        this.baseAPS = baseAPS;
        this.name = name;
        this.enemyTargets = new Transform[0];
        this.currentUpgradeCosts = baseUpgradeCosts;
        this.currentTargetingRange = baseTargetingRange;
        this.currentDMG = this.baseDMG;
        this.currentAPS = baseAPS;
    }

    public void Update()
    {
        this.updateMethod();
        // If selected Draw Range
    }

    public abstract void updateMethod();
    public abstract void findTargets();
    public abstract void attack();
    public abstract void upgrade();
}