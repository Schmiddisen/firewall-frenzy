using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public abstract class Tower : MonoBehaviour
{
    [Header("References")]
    public LayerMask[] enemyMasks;
    public Transform towerRotationPoint;
    public Transform towerFiringPoint;

    public ParticleSystem shootingParticlePrefab;

    public GameObject towerPrefab;

    [Header("Attributes")]

    public float rotationSpeed;
    public int baseUpgradeCosts;
    public int buildCost;
    
    public float baseTargetingRange;

    public int baseDMG;

    public float baseAPS;
    public string name;


    [Header("Runtime Attributes and Refrences")]
    protected List<Transform> enemyTargets;
    protected int currentUpgradeCosts;
    protected int currentLevel = 1;
    protected float currentTargetingRange;
    protected int currentDMG;
    protected float currentAPS;
    protected float timeUntilFire = 0;

    protected CircleCollider2D targetingRangeDetetector;
    

    public void setupTower(LayerMask[] enemyMasks, Transform towerRotationPoint, Transform towerFiringPoint, ParticleSystem shootingParticlePrefab,
                            GameObject towerPrefab, float rotationSpeed, int baseUpgradeCosts, int buildCost, float baseTargetingRange, 
                            int baseDMG, float baseAPS, string name)
    {
        this.enemyMasks = enemyMasks;
        this.towerRotationPoint = towerRotationPoint;
        this.towerFiringPoint = towerFiringPoint;
        this.towerPrefab = towerPrefab;
        this.shootingParticlePrefab = shootingParticlePrefab;
        this.rotationSpeed = rotationSpeed;
        this.baseUpgradeCosts = baseUpgradeCosts;
        this.buildCost = buildCost;
        this.baseTargetingRange = baseTargetingRange;
        this.baseDMG = baseDMG;
        this.baseAPS = baseAPS;
        this.name = name;
        enemyTargets = new List<Transform>();
        currentUpgradeCosts = this.baseUpgradeCosts;
        currentTargetingRange = this.baseTargetingRange;
        currentDMG = this.baseDMG;
        currentAPS = this.baseAPS;

        // Add a Circle Collider for detection
        targetingRangeDetetector = gameObject.AddComponent<CircleCollider2D>();
        targetingRangeDetetector.isTrigger = true;
        targetingRangeDetetector.radius = baseTargetingRange;
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
        //Needs implementation
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.forward, baseTargetingRange);
    }

    protected void upgradeRange(float amount) {
        this.currentTargetingRange += amount;
        this.targetingRangeDetetector.radius = currentTargetingRange;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //Return if the other object is not in the layer of the Hit-Able enemies
        if (enemyMasks.Contains(other.gameObject.layer)) return;
        
        Transform enemyTransform = other.transform;
        if (!enemyTargets.Contains(enemyTransform)) // Only if the Enemy is not in the List
        {
            enemyTargets.Add(enemyTransform);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        //Return if the other object is not in the layer of the Hit-Able enemies
        if (enemyMasks.Contains(other.gameObject.layer)) return;
        
        Transform enemyTransform = other.transform;
        if (enemyTargets.Contains(enemyTransform)) // Only if the Enemy is in the List
        {
            enemyTargets.Remove(enemyTransform);
        }
    }

    public abstract void updateMethod();
    public abstract void attack();
    public abstract void upgrade();
}