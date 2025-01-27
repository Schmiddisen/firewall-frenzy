using UnityEngine;
using System.Collections.Generic;

public class PacketDefender : TargetingTower
{
    [Header("Upgrade JSON")]
    [SerializeField] TextAsset upgradeJson;

    [Header("Projectiles")]
    [SerializeField] GameObject knockbackBullet;
    [SerializeField] GameObject laserBullet;
    [SerializeField] LineRenderer laserLR;
 
    TowerPathUpgrades upgradeData;

    
    public void Start() {
        this.upgradeData = JsonUtility.FromJson<TowerPathUpgrades>(upgradeJson.text);
    }
    
    public override void updateMethod() {
        //Make a Copy because maybe an Enemy will exit the range right in the time when we iterate the list
        List<Transform> targetsCopy = new List<Transform>(base.enemyTargets);
        currentTarget = TargetingCalculator.getTargetAfterPriority(this.targetPrio, targetsCopy);

        if (currentTarget != null) {
            base.RotateTowardsTarget();
        }
        //Draw Laser if PathA maxlevel
        if (base.upgradePath == UpgradePath.PathA && base.currentLevel == 3) {
            drawLaser();
        }
    }

    public override void upgrade(UpgradePath path)
    {
        //Which path
        Upgrades upgradeData = path == UpgradePath.PathA ? this.upgradeData.PathA : this.upgradeData.PathB;

        base.applyUpgrade(upgradeData, path);

        if (base.upgradePath == UpgradePath.PathA && base.currentLevel == 3) {
            base.bulletPrefab = this.laserBullet;
        }

        if (base.upgradePath == UpgradePath.PathB && base.currentLevel == 3) {
            base.bulletPrefab = this.knockbackBullet;
        }
        
    }
    private void drawLaser() {
        if (currentTarget == null) {
            laserLR.enabled = false;
            return;
        }
        laserLR.enabled = true;
        laserLR.SetPosition(0, towerFiringPoint.position);
        laserLR.SetPosition(1, currentTarget.position);
    }
}
