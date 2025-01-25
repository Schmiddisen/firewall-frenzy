using UnityEngine;

public class PacketDefender : TargetingTower
{
    [Header("Upgrade JSON")]
    [SerializeField] TextAsset upgradeJson;

    TowerPathUpgrades upgradeData;

    
    public void Start() {
        this.upgradeData = JsonUtility.FromJson<TowerPathUpgrades>(upgradeJson.text);
    }
    
    public override void Shoot()
    {
        base.Shoot();
        Debug.Log("Override Shoot");
    }

    public override void upgrade(UpgradePath path)
    {
        //Which path
        Upgrades upgradeData = path == UpgradePath.PathA ? this.upgradeData.PathA : this.upgradeData.PathB;

        base.applyUpgrade(upgradeData, path);
        
    }

}
