using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;
    
    
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] public float distanceTraveled = 0f;

    private Transform target;
    private int pathIndex = 0;
    private Vector2 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
        target = LevelManager.main.path[pathIndex];
        moveSpeed = Random.Range(1f, 4f);
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        
        rb.linearVelocity =  dir * moveSpeed;
        distanceTraveled += Vector2.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
    }
}
