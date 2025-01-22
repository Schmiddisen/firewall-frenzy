using System.Collections.Generic;
using UnityEngine;

public class DDOS : Enemy
{
    [SerializeField] float staggerRange;
    [SerializeField] double staggerValue;
    [SerializeField] public LayerMask towerMask;

    List<Transform> towerTargets;
    private List<Tower> affectedTowers = new List<Tower>();
    
    
    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }

    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateEffectedTowers();
    }

    private void UpdateEffectedTowers()
    {
        // Find all towers within the effect radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, staggerRange, towerMask);
        List<Tower> currentTowersInRange = new List<Tower>();

        foreach (Collider2D collider in hitColliders)
    {
            Tower tower = collider.GetComponent<Tower>();
            if (tower != null)
            {
                currentTowersInRange.Add(tower);
                Debug.Log(currentTowersInRange);
            }
        }

        // Apply the slowing effect to newly entered towers
        foreach (Tower tower in currentTowersInRange)
        {
            if (!affectedTowers.Contains(tower))
        {
            affectedTowers.Add(tower);
            tower.addStagger(staggerValue);
        }
    }

        // Remove the slowing effect from towers that left the range
        for (int i = affectedTowers.Count - 1; i >= 0; i--)
    {
            Tower tower = affectedTowers[i];
            if (!currentTowersInRange.Contains(tower))
        {
            affectedTowers.Remove(tower);
            tower.removeStagger(staggerValue);
        }
    }
    }

    public override void onDestroy() {
        base.onDestroy();

        List<Transform> targetsCopy = new List<Transform>(towerTargets);
        foreach (Transform target in targetsCopy)
        {
            if (target == null) continue;
            target.gameObject.GetComponent<Tower>().removeStagger(staggerValue);
            }

        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        // Visualize the effect radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, staggerRange);
    }

}
