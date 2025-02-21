using System.Collections;
using UnityEngine;

public class Worm : Enemy
{
    [SerializeField] private float stopDuration = 3f; // Time the worm stays untargetable
    [SerializeField] private int regenAmount = 50; // HP regained during stop
    [SerializeField] private GameObject visualIndicatorPrefab; // Visual indicator for untargetable state

    private GameObject visualIndicatorInstance;
    private bool isUntargetable = false;

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth, isCamouflaged);
    }

    public override void FixedUpdate()
    {
        if (!isUntargetable)
        {
            base.FixedUpdate();
        }
    }

    private void Start()
    {
        StartCoroutine(WormBehaviorRoutine());
    }

    private IEnumerator WormBehaviorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f)); // Random delay before stopping

            StopAndRegenerate();
            yield return new WaitForSeconds(stopDuration);

            ResumeMovement();
        }
    }

    private void StopAndRegenerate()
    {
        isUntargetable = true;
        gameObject.layer = LayerMask.NameToLayer("Untargetable"); // Change layer to avoid being targeted

        // Stop movement
        currentMovementSpeed = 0; 
        rb.linearVelocity = Vector2.zero;

        regenHP();

        // Show visual indicator
        if (visualIndicatorPrefab != null)
        {
            visualIndicatorInstance = Instantiate(visualIndicatorPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private void ResumeMovement()
    {
        isUntargetable = false;

        // Restore original layer
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        // Resume movement
        currentMovementSpeed = baseMovementSpeed;

        // Remove visual indicator
        if (visualIndicatorInstance != null)
        {
            Destroy(visualIndicatorInstance);
        }

        ForceReDetection();
    }

    private void ForceReDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            Tower tower = collider.GetComponent<Tower>();
            if (tower != null)
            {
                tower.OnTriggerEnter2D(GetComponent<Collider2D>());
            }
        }
    }

    private void regenHP()
    {
        currentHealth = Mathf.Min(baseHealth, currentHealth + regenAmount);
    }
}
