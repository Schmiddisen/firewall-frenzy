using System.Collections.Generic;
using UnityEngine;

public class DDOS : Enemy
{
    [Header("Stagger Effect")]
    [SerializeField] private float staggerRange; // Radius of effect
    [SerializeField] private double staggerValue; // Stagger value applied to towers
    [SerializeField] private LayerMask towerMask; // Mask to filter towers

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
        // Adjust the range based on the local scale
        float adjustedRange = staggerRange * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);

        // Find all potential towers within the effect range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, adjustedRange, towerMask);
        List<Tower> currentTowersInRange = new List<Tower>();

        foreach (Collider2D collider in hitColliders)
        {
            Tower tower = collider.GetComponent<Tower>();
            if (tower != null)
            {
                // Ensure the center of the tower is within the adjusted range
                float distanceToTower = Vector2.Distance(transform.position, tower.transform.position);
                if (distanceToTower <= adjustedRange)
                {
                    currentTowersInRange.Add(tower);
                }
            }
        }

        // Apply effects to new towers and remove effects from towers that left the range
        foreach (Tower tower in currentTowersInRange)
        {
            if (!affectedTowers.Contains(tower))
            {
                affectedTowers.Add(tower);
                tower.addStagger(staggerValue);
            }
        }

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


    public override void onDestroy()
    {
        base.onDestroy();

        // Ensure all affected towers have the effect removed when this enemy is destroyed
        foreach (Tower tower in affectedTowers)
        {
            if (tower != null)
            {
                tower.removeStagger(staggerValue);
            }
        }
        affectedTowers.Clear();

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        // Adjust the range based on the local scale
        float adjustedRange = staggerRange * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);

        // Visualize the adjusted effect radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, adjustedRange);
    }

}
