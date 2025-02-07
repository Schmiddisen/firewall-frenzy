using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

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
    public int pathIndex;

    public int currentHealth;
    public float currentMovementSpeed;
    private bool isStunned = false;
    private bool isSlowed = false;
    private float slowMultiplier = 1f;

    private bool isDestroyed;

    public float distanceTraveled;

    private BurnEffect burnEffect;

    private float lastBurnTime;

    private System.Random rnd;

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

        this.rnd = new();
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
                this.removeplayerHealth();
                this.onDestroy();
            }
            else
            {
                currentPathTarget = path[pathIndex];
            }
        }
    }

    public void knockback(float knockbackStrength) {
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
        transform.position += (Vector3) knockabackDistance * Time.deltaTime;
    }
    
    public void interruptMovement(float duration) {
    StartCoroutine(interruptMovementCoroutine(duration));
    }

    private IEnumerator interruptMovementCoroutine(float duration) {
        isStunned = true;
        this.currentMovementSpeed = 0;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        applyMovementDebuff();
    }

    public void slowMovement(float duration, float slowingRate) {
        StartCoroutine(slowMovementCoroutine(duration, slowingRate));
    }

    private IEnumerator slowMovementCoroutine(float duration, float slowingRate) {
        isSlowed = true;
        slowMultiplier = slowingRate;
        applyMovementDebuff();
        yield return new WaitForSeconds(duration);
        isSlowed = false;
        applyMovementDebuff();
    }  

    private void applyMovementDebuff() {
        if (isStunned) {
            this.currentMovementSpeed = 0;
        } else if (isSlowed) {
            this.currentMovementSpeed = baseMovementSpeed * slowMultiplier; 
        } else {
            this.currentMovementSpeed = baseMovementSpeed;
        }
    }

    public virtual void SetToughnessGrade(int grade)
    {
        // Default implementation does nothing
    }
    public void takeDamage(int dmg)
    {
        // check if the burn effect has bonus damage, generate a random number bewteen 1 and 10 and then double the taken damage
        if (burnEffect != null){
                    if (burnEffect.applyBonusDmg){
            if (rnd.Next(1,10) == 1 ){
                dmg *= 2;
            }
        }
        }

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
    
    public virtual void removeplayerHealth() {
        LevelManager.main.OnEnemyFinishTrack.Invoke(currentHealth);
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
            // TODO: spawn burning particles
        }
    }
}
