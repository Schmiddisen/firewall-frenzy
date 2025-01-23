using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.EventSystems;

public class TargetingTower: Tower {

    [Header("Further Targting Tower specific Fields and References")]
    [SerializeField]
    public GameObject bulletPrefab;

    private Transform currentTarget;

    void Awake() {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMask, targetPrio,towerBaseCollider, towerRotationPoint, towerFiringPoint, shootingParticlePrefab, towerPrefab,
        rangeIndicator, rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);
    }


    public override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
    }
    public override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);
    }


    public override void updateMethod() {
        //Make a Copy because maybe an Enemy will exit the range right in the time when we iterate the list
        List<Transform> targetsCopy = new List<Transform>(base.enemyTargets);
        currentTarget = TargetingCalculator.getTargetAfterPriority(this.targetPrio, targetsCopy);

        if (currentTarget != null) {
            RotateTowardsTarget();
        }
    }

    public override void attack() {
        if(currentTarget == null) return;
        Shoot();
    }

    public override void upgrade() {
        //for range upgrades base.upgradeRange(x)
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y,
            currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg;
        //Since the sprite is drawn with a 45 Deg rotation, add this to the calculation
        angle += 45;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        base.towerRotationPoint.rotation = Quaternion.RotateTowards(base.towerRotationPoint.rotation, targetRotation,
            base.rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, base.towerFiringPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.setTarget(currentTarget, base.currentDMG);
    }
}