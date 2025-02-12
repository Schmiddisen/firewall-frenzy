using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


public class AreaOfEffectTower : Tower
{
    [Header("Ability Attributes")]
    public int timeBetweenPulses = 5; // in seconds
    public int knockbackStrength = 20;
    public ParticleSystem burnParticleSystem;

    [Header("Upgrade JSON")]
    [SerializeField] TextAsset upgradeJson;
    TowerPathUpgrades upgradeData;

    [Header("AOE Tower Runtime Attributes and Refrences")]
    protected bool burnEffectUnlocked = false;
    protected bool knockbackUnlocked = false;
    protected bool pulsesUnlocked = false;
    protected float lastPulseTime = 0f;

    BurnEffect currentBurn = new(5, 10, 1, false);

    void Start()
    {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMask, targetPrio, towerBaseCollider, towerRotationPoint, towerFiringPoint, shootingParticlePrefab, towerPrefab,
        rangeIndicator, rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);

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
        bool pulsing = false;

        // Check if enough time has passed since last pulse
        if (pulsesUnlocked && Time.time - lastPulseTime >= timeBetweenPulses)
        {
            ParticleSystem pulseParticle = Instantiate(base.shootingParticlePrefab, base.towerRotationPoint.position, this.towerRotationPoint.rotation);
            SetParticleRadius(pulseParticle);

            ParticleSystem.MainModule mainModule = pulseParticle.main;
            mainModule.startColor = Color.black;

            // set pulsing to true to double damage
            pulsing = true;
            // Update last pulse time
            lastPulseTime = Time.time;
        }
        else
        {
            // default animation
            ParticleSystem defaultParticle = Instantiate(base.shootingParticlePrefab, base.towerRotationPoint.position, this.towerRotationPoint.rotation);
            SetParticleRadius(defaultParticle);
        }

        if (base.enemyTargets == null || base.enemyTargets.Count == 0) return;
        //Make a Copy because maybe an Enemy will exit the range right in the time when we iterate the list
        List<Transform> targetsCopy = new List<Transform>(base.enemyTargets);
        foreach (Transform target in targetsCopy)
        {
            if (target == null) continue;
            Enemy enemy = target.gameObject.GetComponent<Enemy>();

            if (pulsing)
            {
                enemy.takeDamage(base.currentDMG * 2); // double damage on pulse
            }
            else
            {
                enemy.takeDamage(base.currentDMG);
            }

            if (burnEffectUnlocked)
            {
                enemy.ApplyBurnEffect(currentBurn);
                ParticleSystem burnEffectParticles = Instantiate(burnParticleSystem, enemy.transform.position, burnParticleSystem.transform.rotation, target);
            }

            if (knockbackUnlocked)
            {
                enemy.knockback(knockbackStrength);
            }
        }
    }

    public override void upgrade(UpgradePath path)
    {

        //Which path
        Upgrades upgradeData = path == UpgradePath.PathA ? this.upgradeData.PathA : this.upgradeData.PathB;

        applyUpgrade(upgradeData, path);

        // ability upgrade logic
        if (upgradePath == UpgradePath.PathA && getCurrentLevel() >= 1)
        {
            // Path A upgrade logic
            burnEffectUnlocked = true;
            // DoT upgrades
            if (getCurrentLevel() >= 2)
            {
                currentBurn = new BurnEffect(5, 20, 1, false); //increase burn DoT by 1
                if (getCurrentLevel() >= 3)
                {
                    Debug.Log("BONUSDMG UNLOCKEED");
                    currentBurn = new BurnEffect(5, 20, 1, true); // add bonus dmg effect for enemies having the DoT
                }
            }
        }
        else if (upgradePath == UpgradePath.PathB && getCurrentLevel() >= 1)
        {
            // Path B upgrade logic
            if (getCurrentLevel() >= 2)
            {
                knockbackUnlocked = true;
                if (getCurrentLevel() >= 3)
                {
                    Debug.Log("Knockback UNLOCKEED");
                    pulsesUnlocked = true;
                }
            }
        }

    }

    // increase CircleCollider Range for Particles so that they match the actual tower range
    private void SetParticleRadius(ParticleSystem particleInstance)
    {
        CircleCollider2D collider = particleInstance.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = currentTargetingRange; // use current tower range
        }
    }
}

[Serializable]
public class BurnEffect
{
    public double duration;
    public int damage;
    public double timeBetweenBurns;

    public bool applyBonusDmg;

    public BurnEffect(double duration, int damage, double timeBetweenBurns, bool applyBonusDmg)
    {
        this.duration = duration;
        this.damage = damage;
        this.timeBetweenBurns = timeBetweenBurns;
        this.applyBonusDmg = applyBonusDmg;
    }
}
