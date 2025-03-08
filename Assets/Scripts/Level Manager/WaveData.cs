using System.Collections.Generic;

// this document as orientation for the wave config: https://topper64.co.uk/nk/btd6/rounds/regular

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
    public float firstSpawnDelay;
    public float spawnDelay;
    public int toughnessGrade;
}

