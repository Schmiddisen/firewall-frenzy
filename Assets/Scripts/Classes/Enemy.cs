using UnityEngine;
using UnityEngine.Playables;

public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    public ParticleSystem flameParticles;


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

    private BurnEffect burnEffect;

    private float lastBurnTime;

    public void setupEnemy(float moveSpeed, int health, int currencyWorth)
    {
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
        this.burnEffect = null;
        this.lastBurnTime = Time.time;
    }

    public virtual void FixedUpdate()
    {
        move();
    }

    public virtual void Update(){
        HandleBurnEffect();
    }

    private void move()
    {

        Vector2 dir = (currentPathTarget.position - transform.position).normalized;
        rb.linearVelocity = dir * currentMovementSpeed;

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

    public void knockback(float knockbackStrength)
    {
        Debug.Log("KNOWCKBACK " + this.gameObject);
        int prevWaypointIndex = pathIndex - 1;

        Transform prevWaypoint;

        if (prevWaypointIndex < 0)
        {
            prevWaypoint = LevelManager.main.startPoint;
        }
        else
        {
            prevWaypoint = path[prevWaypointIndex];
        }


        Vector2 dir = (prevWaypoint.position - transform.position).normalized;

        Vector2 knockabackDistance = dir * knockbackStrength;

        Vector2 distance = knockabackDistance * Time.deltaTime;

        distanceTraveled -= distance.magnitude;

        transform.position += (Vector3)knockabackDistance * Time.deltaTime;
    }

    public void interruptMovement(bool shouldStop)
    {
        if (shouldStop)
        {
            this.currentMovementSpeed = 0;
        }
        else
        {
            this.currentMovementSpeed = baseMovementSpeed;
        }
    }

    public void takeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0 && !isDestroyed)
        {
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            onDestroy();
        }
    }

    public virtual void onDestroy()
    {
        Destroy(gameObject);
    }

    public float getDistanceTraveled()
    {
        return this.distanceTraveled;
    }

    public void ApplyBurnEffect(BurnEffect effect)
    {
        burnEffect = effect;
    }

    public void HandleBurnEffect()
    {
        if (burnEffect == null) return;

        burnEffect.duration -= Time.deltaTime;

        // remove burnEffect upon expiration
        if (burnEffect.duration <= 0)
        {
            burnEffect = null;
            return;
        }


        // Check if enough time has passed since last burn
        if (Time.time - lastBurnTime >= burnEffect.timeBetweenBurns)
        {
            // Apply burn effect damage
            takeDamage(burnEffect.damage);
            // Update last burn time
            lastBurnTime = Time.time;
            Instantiate(flameParticles, this.transform.position, this.transform.rotation);
        }
        
    }
}
