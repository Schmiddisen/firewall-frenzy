using System.Collections;
using UnityEngine;

public class Worm : Enemy
{
    [SerializeField] private float stopDuration = 3f; // Time the worm stays untargetable
    [SerializeField] private int regenAmount = 50; // HP regained during stop
    [SerializeField] private GameObject visualIndicatorPrefab; // Visual indicator for untargetable state
    [SerializeField] private float rotationSpeed = 250f;

    private GameObject visualIndicatorInstance;
    private bool isUntargetable = false;
    private bool hasSpikes = true;

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth, isCamouflaged, hasSpikes);
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
        gameObject.layer = LayerMask.NameToLayer("Untargetable");

        // Stop movement
        currentMovementSpeed = 0;
        rb.linearVelocity = Vector2.zero;

        regenHP();

        // Show visual indicator using CURRENT rotation
        if (visualIndicatorPrefab != null)
        {
            Quaternion currentRotation = transform.rotation;

            float offsetAngle = 0f;
            visualIndicatorInstance = Instantiate(
                visualIndicatorPrefab,
                transform.position,
                currentRotation * Quaternion.Euler(0, 0, offsetAngle),
                transform
            );
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

    // override move funcion, as the sprite is longer than all other sprites
    public override void move()
    {
        Vector2 dir = (currentPathTarget.position - transform.position).normalized;
        rb.linearVelocity = dir * currentMovementSpeed;

        // Calculate target rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float offsetAngle = 0f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle + offsetAngle);

        // rotate towards target direction
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        distanceTraveled += currentMovementSpeed * Time.deltaTime;

        if (Vector2.Distance(currentPathTarget.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex == path.Length)
            {
                this.removeplayerHealth();
                this.onDestroy();
            }
            else
            {
                currentPathTarget = path[pathIndex];
            }
        }
    }
}

