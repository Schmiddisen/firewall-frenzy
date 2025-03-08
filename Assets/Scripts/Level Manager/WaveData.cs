using System.Collections.Generic;

[System.Serializable]
public class WaveCollection
{
    public List<WaveData> waves;
}

[System.Serializable]
public class WaveData
{
    public string waveName;
    public List<EnemySpawnInfo> enemies;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public string enemyType;
    public int count;
    public float spawnDelay;
    public int toughnessGrade;
}

