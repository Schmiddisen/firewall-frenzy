using UnityEngine;

public class Trojan_horse : Enemy
{
    [SerializeField] private GameObject virusPrefab; // The Virus prefab to spawn on death

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }

    /*public override void onDestroy()
    {
        // Spawn three Virus enemies at the same path progress
        for (int i = 0; i < 3; i++)
        {
            GameObject virus = Instantiate(virusPrefab, transform.position, Quaternion.identity);
            Virus virusScript = virus.GetComponent<Virus>();

            if (virusScript != null)
            {
                virusScript.SetToughnessGrade(5);
                virusScript.setupEnemy(virusScript.baseMovementSpeed, virusScript.baseHealth, virusScript.currencyWorth);
                
                // Inherit path progress
                virusScript.path = this.path;
                virusScript.pathIndex = this.pathIndex; // Continue from the same path index
                virusScript.currentPathTarget = this.currentPathTarget;
                
                // Small random offset to avoid stacking
                Vector2 offset = Random.insideUnitCircle * 0.2f;
                virus.transform.position += (Vector3)offset;
            }
        }

        base.onDestroy(); // Destroy the TankEnemy itself
    }*/
}
