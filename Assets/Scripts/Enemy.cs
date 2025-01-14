using UnityEngine;

public class Enemy: MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    
    
    [Header("Attributes")]
    [SerializeField] public float baseMovementSpeed;
    [SerializeField] public int baseHealth;
    [SerializeField] public int currencyWorth;

    

    private Transform[] path;
    private Transform currentPathTarget;
    private int pathIndex = 0;

    private int currentHealth;
    private float currentMovementSpeed;

    private bool isDestroyed = false;



    public void Start() {
        path = LevelManager.main.path;
        currentPathTarget = path[0];

        currentHealth = baseHealth;
        currentMovementSpeed = baseMovementSpeed;
    
    }

    public void FixedUpdate() {
        move();
    }

    private void move() {

        Vector2 dir = (currentPathTarget.position - transform.position).normalized;
        rb.linearVelocity =  dir * currentMovementSpeed;
        
        if (Vector2.Distance(currentPathTarget.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == path.Length)
            {
                this.onDestroy();
            }
            else
            {
                currentPathTarget = path[pathIndex];
            }
        }
    }

    

    public void interruptMovement(float duration) {
        
    }

    public void takeDamage(int dmg) {
        currentHealth -= dmg;

        if (currentHealth <= 0 && !isDestroyed)
        {
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            onDestroy();
        }
    }

    public void onDestroy() {
        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }
}
