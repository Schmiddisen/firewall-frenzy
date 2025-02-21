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
        float adjustedRange = staggerRange * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        ParticleSystem.ShapeModule psShape = GetComponentInChildren<ParticleSystem>().shape;
        psShape.radius = adjustedRange;
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

    public override void takeDamage(int dmg)
    {
        base.takeDamage(dmg);
        DestroySpikes();
    }

    // this method calculates how many spikes should be destroyed based on the amount of spikes owned and base hp
    private void DestroySpikes()
    {
        int totalSpikes = 0;
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Spike"))
            {
                totalSpikes++;
            }
        }

        int healthPerSpike = baseHealth / totalSpikes;
        int spikesDestroyed = baseHealth - currentHealth;
        spikesDestroyed /= healthPerSpike;

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Spike") && spikesDestroyed > 0)
            {
                Destroy(child.gameObject);
                spikesDestroyed--;
            }
        }
    }

}
