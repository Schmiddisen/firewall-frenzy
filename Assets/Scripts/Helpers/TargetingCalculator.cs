using System.Collections.Generic;
using UnityEngine;

public static class TargetingCalculator {
    public static Transform getTargetAfterPriority(TargetingPriority prio, List<Transform> enemies) {
        if (enemies == null || enemies.Count == 0) return null; 
        switch (prio)
        {
            case TargetingPriority.LeastDistanceTraveled:
                int min_index = 0;
                float min_distance = float.MaxValue;
                for (int i = 0; i < enemies.Count; i++)
                {
                    float d = enemies[i].GetComponent<Enemy>().getDistanceTraveled();
                    if (d < min_distance) {
                        min_index = i;
                        min_distance = d;
                    }
                }
                return enemies[min_index];

            case TargetingPriority.MostDistanceTraveled:
                int max_index = 0;
                float max_distance = 0;
                for (int i = 0; i < enemies.Count; i++)
                {
                    float d = enemies[i].GetComponent<Enemy>().getDistanceTraveled();
                    if (d > max_distance) {
                        max_index = i;
                        max_distance = d;
                    }
                }
                return enemies[max_index];

            case TargetingPriority.EnemyPriority:
                return null;

            default:
                return null;
        }
    }
}