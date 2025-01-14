using System.Collections;
using UnityEngine;
using UnityEditor;

public class TurretSlowMo : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private LayerMask enemyMask;

    [Header("Attribute")] 
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 4f; //Attacks per Second
    [SerializeField] private float freezeTime = 1f;

    private float timeUntilFire;

    // Update is called once per frame
    void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {

    }

    private IEnumerator ResetEnemySpeed(Enemy em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.interruptMovement(0);
    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
