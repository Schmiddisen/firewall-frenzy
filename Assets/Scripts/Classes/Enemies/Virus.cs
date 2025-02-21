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
        // Calculate the total health left at death
        int totalHealthAtDeath = currentHealth + ((toughnessGrade - 1) * baseHealth);
        
        // Deduct the total calculated damage from the player's health
        LevelManager.main.OnEnemyFinishTrack.Invoke(totalHealthAtDeath);
    }

    private void SpawnWeakerVirus()
    {
        Virus weakerVirus = Instantiate(this, transform.position, Quaternion.identity);
        weakerVirus.toughnessGrade = this.toughnessGrade - 1;
        weakerVirus.baseHealth = this.baseHealth; 
        weakerVirus.currencyWorth = this.currencyWorth; // Adjust currency reward
        weakerVirus.distanceTraveled = this.getDistanceTraveled(); // Copy parent's distance
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

    public float GetMovementSpeedByToughness(int grade)
    {
        switch (grade)
        {
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
