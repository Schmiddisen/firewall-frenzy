using UnityEngine;
using System.Collections.Generic;

public static class EnemyFactory
{
    private static Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    public static void Initialize()
    {
        if (enemyPrefabs.Count == 0)
        {
            enemyPrefabs["Virus"] = Resources.Load<GameObject>("Prefabs/Virus");
            enemyPrefabs["Worm"] = Resources.Load<GameObject>("Prefabs/Worm");
            enemyPrefabs["Glitch"] = Resources.Load<GameObject>("Prefabs/Glitch");
        }
    }

    public static GameObject GetEnemyPrefab(string enemyType)
    {
        if (enemyPrefabs.Count == 0) Initialize();
        return enemyPrefabs.ContainsKey(enemyType) ? enemyPrefabs[enemyType] : null;
    }
}
