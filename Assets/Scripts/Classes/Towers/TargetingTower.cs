using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.EventSystems;

public class TargetingTower: Tower {

    [Header("Further Targting Tower specific Fields and References")]
    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField] public float barrelRotationOffset = 0;

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

    public override void upgrade(UpgradePath path) {
        //for range upgrades base.upgradeRange(x)
        Debug.Log("Empty Method body, because upgrade gets implemented for the individual Towers, so if you see this message, the wrong upgrade method gets called");
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y,
            currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg;
        
        angle += this.barrelRotationOffset;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        base.towerRotationPoint.rotation = Quaternion.RotateTowards(base.towerRotationPoint.rotation, targetRotation,
            base.rotationSpeed * Time.deltaTime);
    }

    public virtual void Shoot()
    {
        Vector3 direction = (currentTarget.position - base.towerFiringPoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        GameObject bulletObj = Instantiate(bulletPrefab, base.towerFiringPoint.position, rotation);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.setTarget(currentTarget, base.currentDMG);
    }
}