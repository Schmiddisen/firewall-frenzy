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
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
             transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
