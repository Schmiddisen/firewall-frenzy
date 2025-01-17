using UnityEngine;


public class TargetingTower: Tower {

    [Header("Further Targting Tower specific Fields and References")]
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public TargetingPriority targetPrio;

    private Transform currentTarget;

    void Awake() {
        // From Serialized Fields in Unity Editor
        base.setupTower(enemyMasks, towerRotationPoint, towerFiringPoint, shootingParticlePrefab, towerPrefab,
        rotationSpeed, baseUpgradeCosts, buildCost, baseTargetingRange, baseDMG, baseAPS, name);
    }

    public override void OnMouseDown() {
        base.OnMouseDown();
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
    }
    public override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);
    }


    public override void updateMethod() {
        if (currentTarget != null) {
            RotateTowardsTarget();
        }
    }

    public override void attack() {
        currentTarget = TargetingCalculator.getTargetAfterPriority(this.targetPrio, base.enemyTargets);
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