using System;
using System.Collections.Generic;


[Serializable]
public class UpgradeMetrics
{
    public int level;
    public float aps;
    public int damage;
    public float range;
}

[Serializable]
public class Upgrades
{
    public List<UpgradeMetrics> upgrades;
}

[Serializable]
public class TowerPathUpgrades
{
    public Upgrades PathA;
    public Upgrades PathB;
}
