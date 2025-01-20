using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;


[System.Serializable]
public abstract class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public LayerMask enemyMask;

    public CircleCollider2D towerBaseCollider;
    public Transform towerRotationPoint;
    public Transform towerFiringPoint;

    public ParticleSystem shootingParticlePrefab;

    public GameObject towerPrefab;

    public LineRenderer rangeIndicator;

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

    public bool isActiv;
    
    public void setupTower(LayerMask enemyMask, CircleCollider2D towerBaseCollider, Transform towerRotationPoint, Transform towerFiringPoint, ParticleSystem shootingParticlePrefab,
                            GameObject towerPrefab, LineRenderer rangeIndicator, float rotationSpeed, int baseUpgradeCosts,
                            int buildCost, float baseTargetingRange, int baseDMG, float baseAPS, string name)
    {
        this.enemyMask = enemyMask;
        this.towerBaseCollider = towerBaseCollider;
        this.towerRotationPoint = towerRotationPoint;
        this.towerFiringPoint = towerFiringPoint;
        this.towerPrefab = towerPrefab;
        this.rangeIndicator = rangeIndicator;
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
        targetingRangeDetetector.offset = new Vector2(0, 0);

        isActiv = false;

        redrawRangeIndicator();
    }

    public void Update()
    {
        //If tower isnt active yet, return
        if (!isActiv) return;
        //Tower independent Update method
        this.updateMethod();

        //General time until fire advance
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / this.currentAPS)
        {
            attack();
            timeUntilFire = 0f;
        }

    }

    private void redrawRangeIndicator()
    {
        rangeIndicator.useWorldSpace = false;
        rangeIndicator.loop = true;
        int segments = 25;
        float radius = this.currentTargetingRange;
        rangeIndicator.positionCount = segments + 1;
        rangeIndicator.sortingOrder = 10;

        AnimationCurve curve = new AnimationCurve();
        float constantWidth = 0.08f;
        curve.AddKey(0f, constantWidth);
        curve.AddKey(1f, constantWidth);
        rangeIndicator.widthCurve = curve;

        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            rangeIndicator.SetPosition(i, position);
        }
    }

    protected void upgradeRange(float amount) {
        this.currentTargetingRange += amount;
        this.targetingRangeDetetector.radius = currentTargetingRange;
        redrawRangeIndicator();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //Return if the other object is not in the layer of the Hit-Able enemies
        if (((1 << other.gameObject.layer) & enemyMask) == 0) return;
        
        Transform enemyTransform = other.transform;
        if (!enemyTargets.Contains(enemyTransform)) // Only if the Enemy is not in the List
        {
            enemyTargets.Add(enemyTransform);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        //Return if the other object is not in the layer of the Hit-Able enemies
        if (((1 << other.gameObject.layer) & enemyMask) == 0) return;
        
        Transform enemyTransform = other.transform;
        if (enemyTargets.Contains(enemyTransform)) // Only if the Enemy is in the List
        {
            enemyTargets.Remove(enemyTransform);
        }
    }

    public void setActive(bool b) {
        isActiv = b;
    }

    public void showRangeIndicator(bool b) {
        this.rangeIndicator.enabled = b;
    }

    public abstract void updateMethod();
    public abstract void attack();
    public abstract void upgrade();

}