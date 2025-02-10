using UnityEngine;

public class Trojan_horse : Enemy
{
    [SerializeField] private GameObject virusPrefab; // The Virus prefab to spawn on death
    [SerializeField] private int spawnCount; // Number of viruses to spawn

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }

    public override void onDestroy()
    {
        if (pathIndex >= path.Length)
        {
            base.onDestroy();
            return;
        }

        // Determine movement direction
        Vector2 moveDirection = (pathIndex < path.Length) 
            ? (path[pathIndex].position - transform.position).normalized 
            : rb.linearVelocity.normalized;

        float spacing = 0.4f; // Distance between spawned viruses

        // Spawn three Virus enemies, starting from the same position and moving backwards
        for (int i = 0; i < spawnCount; i++)
        {
            // Adjust position to spawn viruses behind the Trojan Horse
            Vector3 spawnPosition = transform.position - (Vector3)(moveDirection * i * spacing); // i * spacing (0, 0.4, 0.8)

            GameObject virus = Instantiate(virusPrefab, spawnPosition, Quaternion.identity);
            Virus Virus = virus.GetComponent<Virus>();

            if (Virus != null)
            {
                Virus.SetToughnessGrade(5);
                Virus.setupEnemy(Virus.baseMovementSpeed, Virus.baseHealth, Virus.currencyWorth);
                Virus.distanceTraveled = this.getDistanceTraveled(); // Copy parent's distance, relevant for tower targeting
                Virus.UpdateColor();
                Virus.currentMovementSpeed = Virus.GetMovementSpeedByToughness(Virus.toughnessGrade);

                // Inherit path progress
                Virus.path = this.path;
                Virus.pathIndex = this.pathIndex; // Continue from the same path index
                Virus.currentPathTarget = this.currentPathTarget;
            }
        }

        base.onDestroy(); // Destroy the Trojan Horse itself
    }

    public override void removeplayerHealth()
    {
        // Calculate the total health left at death
        int totalHealthAtDeath = currentHealth + (spawnCount * 5 * virusPrefab.GetComponent<Virus>().baseHealth); // the 5 represents the toughness grade of the spawned viruses
        
        // Deduct the total calculated damage from the player's health
        LevelManager.main.OnEnemyFinishTrack.Invoke(totalHealthAtDeath);
    }
}
