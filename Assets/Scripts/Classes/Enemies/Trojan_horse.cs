using UnityEngine;

public class Trojan_horse : Enemy
{
    [SerializeField] private GameObject virusPrefab; // The Virus prefab to spawn on death

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }

    public override void onDestroy()
    {
        // Determine movement direction
        Vector2 moveDirection = (pathIndex < path.Length) 
            ? (path[pathIndex].position - transform.position).normalized 
            : rb.linearVelocity.normalized;

        float spacing = 0.4f; // Distance between spawned viruses

        // Spawn three Virus enemies at the same path progress
        for (int i = 0; i < 3; i++)
        {
            GameObject virus = Instantiate(virusPrefab, transform.position + (Vector3)(moveDirection * (i - 1) * spacing), Quaternion.identity);
            Virus virusScript = virus.GetComponent<Virus>();

            if (virusScript != null)
            {
                virusScript.SetToughnessGrade(5);
                virusScript.setupEnemy(virusScript.baseMovementSpeed, virusScript.baseHealth, virusScript.currencyWorth);
                
                // Inherit path progress
                virusScript.path = this.path;
                virusScript.pathIndex = this.pathIndex; // Continue from the same path index
                virusScript.currentPathTarget = this.currentPathTarget;
            }
        }

        base.onDestroy();
    }
}
