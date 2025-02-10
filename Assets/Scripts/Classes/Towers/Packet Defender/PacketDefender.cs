using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

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
        base.updateMethod();
        if (base.upgradePath == UpgradePath.PathA && base.getCurrentLevel() == 3) {
            drawLaser();
        }
    }

    public override void upgrade(UpgradePath path)
    {
        //Which path
        Upgrades upgradeData = path == UpgradePath.PathA ? this.upgradeData.PathA : this.upgradeData.PathB;

        base.applyUpgrade(upgradeData, path);

        if (base.upgradePath == UpgradePath.PathA && base.getCurrentLevel() == 3) {
            base.bulletPrefab = this.laserBullet;
        }

        if (base.upgradePath == UpgradePath.PathB && base.getCurrentLevel() == 3) {
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

        Vector3 targetPosition = currentTarget.position.CloneViaSerialization<Vector3>();

        //Add some Noise in the target position in order to simulate a changing and "firing" laser
        targetPosition.x = targetPosition.x + Random.Range(-0.1f,0.1f);
        targetPosition.y = targetPosition.y + Random.Range(-0.1f,0.1f);
        laserLR.SetPosition(1, targetPosition);
    }
}
