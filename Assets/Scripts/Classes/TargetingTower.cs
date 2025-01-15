using UnityEngine;
using System;

public class TargetingTower: Tower {

    [Header("Further Targting Tower specific Fields and References")]
    [SerializeField]
    public GameObject bulletPrefab;

    private Transform currentTarget;

    void Start() {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMasks, towerRotationPoint, towerFiringPoint, towerPrefab,
        rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);
    }

    public override void OnMouseDown() {
        base.OnMouseDown();
    }

    public override void updateMethod() {
        findTargets();
        if (currentTarget != null) {
            RotateTowardsTarget();
        }
    }


    public override void findTargets() {
        //First entry
        LayerMask targetedEnemyMask = base.enemyMasks[0];
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, base.currentTargetingRange,
             (Vector2) transform.position, 0f, targetedEnemyMask);
        if (hits.Length > 0)
        {
            //First in Range
            currentTarget = hits[0].transform;
        }
    }

    public override void attack() {
        if(currentTarget == null) return;
        Shoot();
    }

    public override void upgrade() {

    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y,
            currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        base.towerRotationPoint.rotation = Quaternion.RotateTowards(base.towerRotationPoint.rotation, targetRotation,
            base.rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, base.towerFiringPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.setTarget(currentTarget);
    }
}