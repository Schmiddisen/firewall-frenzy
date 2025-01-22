using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    
    
    [Header("Attributes")]
    [SerializeField] public float baseMovementSpeed;
    [SerializeField] public int baseHealth;
    [SerializeField] public int currencyWorth;

    

    public Transform[] path;
    public Transform currentPathTarget;
    private int pathIndex;

    private int currentHealth;
    private float currentMovementSpeed;

    private bool isDestroyed;

    private float distanceTraveled;


    public void setupEnemy(float moveSpeed, int health, int currencyWorth){
        path = LevelManager.main.path;
        currentPathTarget = path[0];

        this.baseMovementSpeed = moveSpeed;
        this.baseHealth = health;
        this.currencyWorth = currencyWorth;

        currentHealth = baseHealth;
        currentMovementSpeed = baseMovementSpeed;

        this.pathIndex = 0;
        distanceTraveled = 0;

        this.isDestroyed = false;
    }

    public void FixedUpdate() {
        move();
    }

    private void move() {

        Vector2 dir = (currentPathTarget.position - transform.position).normalized;
        rb.linearVelocity =  dir * currentMovementSpeed;

        distanceTraveled += currentMovementSpeed * Time.deltaTime;
        
        if (Vector2.Distance(currentPathTarget.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == path.Length) // Enemy has crossed the end line
            {
                this.onDestroy();
                // call the event to reduce the player's health by the current health of the enemy 
                LevelManager.main.OnEnemyFinishTrack.Invoke(currentHealth);
            }
            else
            {
                currentPathTarget = path[pathIndex];
            }
        }
    }

    
    public void interruptMovement(bool shouldStop) {
        if (shouldStop) {
            this.currentMovementSpeed = 0;
        }
        else {
            this.currentMovementSpeed = baseMovementSpeed;
        }
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

    public float getDistanceTraveled() {
        return this.distanceTraveled;
    }
}
