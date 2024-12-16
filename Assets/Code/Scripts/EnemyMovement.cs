using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // check distance, and if Enemy is on target, switch to the next one
        if (Vector2.Distance(target.position, transform.position) <= 0.1)
        {
            pathIndex++;

            // remove enemy at EndPoint
            if (pathIndex >= LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }

        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; //normalized -> value is between 0 and 1

        rb.linearVelocity = direction * moveSpeed;
    }
}
