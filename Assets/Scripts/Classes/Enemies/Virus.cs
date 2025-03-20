using UnityEngine;

public class Virus : Enemy
{
    [SerializeField] public int toughnessGrade = 1; // Will be overwritten by the WaveSpawner	
    [SerializeField] private Color[] virusColors;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth, isCamouflaged);
        
        // Ensure we get the SpriteRenderer from the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is missing on " + gameObject.name);
        }
        
        UpdateColor();
    }

    public override void SetToughnessGrade(int grade)
    {
        toughnessGrade = grade;

        // Adjust baseHealth for toughness grade 9 (multiply by 10)
        if (toughnessGrade == 9)
        {
            baseHealth *= 10; // Multiply baseHealth by 10 for grade 9
            currentHealth = baseHealth;
        }

        UpdateColor(); // Update the color when the toughness grade changes
        currentMovementSpeed = GetMovementSpeedByToughness(toughnessGrade); // Apply the movement speed based on the toughness grade
    }

    public override void onDestroy()
    {
        if (pathIndex >= path.Length)
        {
            base.onDestroy();
            return;
        }

        if (toughnessGrade > 1) // If not at the weakest form, spawn the next level
        {
            SpawnWeakerVirus();
        }
        base.onDestroy();
    }

    public override void removeplayerHealth()
    {
        int totalHealthAtDeath = currentHealth;
        int toughness = toughnessGrade;

        // Total health array for each grade
        int[] healthPerGrade = new int[] { 20, 40, 60, 80, 100, 220, 460, 940, 2080 }; // has to be modified if the base health of the virus changes

        // Prevent out-of-bounds exception for toughness == 1
        if (toughness == 1)
        {
            LevelManager.main.OnEnemyFinishTrack.Invoke(totalHealthAtDeath);
            return;
        }

        // Calculate the total health left at death
        if (toughness >= 6)
        {
            // For Grade 6 and above, the enemy spawns 2 weaker enemies
            totalHealthAtDeath += 2 * healthPerGrade[toughness - 2];
        }
        else
        {
            // Standard equation for grades 5 and below
            totalHealthAtDeath += healthPerGrade[toughness - 2];
        }
        
        // Deduct the total calculated damage from the player's health
        LevelManager.main.OnEnemyFinishTrack.Invoke(totalHealthAtDeath);
    }

    private void SpawnWeakerVirus()
    {
        int spawnCount = (toughnessGrade >= 6) ? 2 : 1; // Spawn two viruses if grade is 6 or higher
        float spacing = 0.4f; // Distance between spawned viruses

        // Determine movement direction to spawn them slightly apart
        Vector2 moveDirection = (pathIndex < path.Length) 
        ? (path[pathIndex].position - transform.position).normalized 
        : rb.linearVelocity.normalized;

        for (int i = 0; i < spawnCount; i++)
        {
            // Offset each spawned virus slightly to prevent overlap
            Vector3 spawnPosition = transform.position - (Vector3)(moveDirection * i * spacing);

            Virus weakerVirus = Instantiate(this, spawnPosition, Quaternion.identity);
            weakerVirus.toughnessGrade = this.toughnessGrade - 1;
            if (this.toughnessGrade == 9)
            {
                weakerVirus.baseHealth = this.baseHealth / 10; // No adjustment needed for grade 9 parents
                weakerVirus.currentHealth = this.currentHealth / 10;
            }
            else
            {
                weakerVirus.baseHealth = this.baseHealth; // Normal baseHealth for other grades
            } 
            weakerVirus.currencyWorth = this.currencyWorth; // Adjust currency reward
            weakerVirus.distanceTraveled = this.getDistanceTraveled() - (i * spacing);; // Copy parent's distance
            weakerVirus.UpdateColor();

            // Set the movement speed based on the toughness grade
            weakerVirus.currentMovementSpeed = GetMovementSpeedByToughness(weakerVirus.toughnessGrade);
            
            // Inherit Path Progress
            weakerVirus.path = this.path; // Copy path array
            weakerVirus.pathIndex = this.pathIndex; // Continue from current path index
            weakerVirus.currentPathTarget = this.currentPathTarget; // Set correct next target

            // make the children burn too
            // this might have to be removed if too strong
            weakerVirus.ApplyBurnEffect(GetBurnEffect());
            weakerVirus.lastBurnTime = lastBurnTime;
        }
    }

    public float GetMovementSpeedByToughness(int grade)
    {
        switch (grade)
        {
            case 9:
                return 2.5f;
            case 8:
                return 2.2f;
            case 7:
                return 2.0f;
            case 6:
                return 1.8f;
            case 5:
                return 3.5f;
            case 4:
                return 3.2f;
            case 3:
                return 1.8f;
            case 2:
                return 1.4f;
            case 1:
            default:
                return baseMovementSpeed; // Standard movement speed for grade 1
        }
    }


    public void UpdateColor()
    {
        if (spriteRenderer != null && virusColors.Length >= toughnessGrade)
        {
            Color newColor = virusColors[toughnessGrade - 1];
            newColor.a = 1f; // Ensure full opacity, makes the enemy visible
            spriteRenderer.color = newColor;
        }
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
