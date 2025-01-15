using UnityEngine;
using System;

[System.Serializable]
public abstract class Tower : MonoBehaviour
{
    [Header("References")]
    public LayerMask[] enemyMasks;
    public Transform towerRotationPoint;
    public Transform towerFiringPoint;

    public GameObject towerPrefab;
    protected Transform[] enemyTargets;

    [Header("Attributes")]

    public float rotationSpeed;
    public int baseUpgradeCosts;
    protected int currentUpgradeCosts;
    public int buildCost;
    protected int currentLevel = 1;

    public float baseTargetingRange;
    protected float currentTargetingRange;

    public int baseDMG;
    protected int currentDMG;

    public float baseAPS;
    protected float currentAPS;

    protected float timeUntilFire = 0;

    public string name;

    public void setupTower(LayerMask[] enemyMasks, Transform towerRotationPoint, Transform towerFiringPoint, 
                            GameObject towerPrefab, float rotationSpeed, int baseUpgradeCosts, int buildCost, float baseTargetingRange, 
                            int baseDMG, float baseAPS, string name)
    {
        this.enemyMasks = enemyMasks;
        this.towerRotationPoint = towerRotationPoint;
        this.towerFiringPoint = towerFiringPoint;
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
        //Tower independent Update method
        this.updateMethod();

        //General time until fire advance
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / this.currentAPS)
        {
            this.attack();
            timeUntilFire = 0f;
        }
        // If selected Draw Range
    }

        private void OnMouseEnter()
    {
        //sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        //sr.color = startColor;
    }

    public virtual void OnMouseDown()
    {
        Debug.Log("Mouse Down");
    }

    public abstract void updateMethod();
    public abstract void findTargets();
    public abstract void attack();
    public abstract void upgrade();
}