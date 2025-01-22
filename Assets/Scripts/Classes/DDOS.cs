using System.Collections.Generic;
using UnityEngine;

public class DDOS : Enemy
{
    [SerializeField] int staggerRange;
    [SerializeField] double staggerValue;

    List<Transform> towerTargets;
    List<Transform> staggeredTargets;
    

    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }


    // this function adds towers in range to a list
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //Return if the other object is not in the layer of the affected towers
        // TODO: implement this
        // if (((1 << other.gameObject.layer) & enemyMask) == 0) return;

        Transform towerTransform = other.transform;
        if (!towerTargets.Contains(towerTransform)) // Only if the tower is not in the List
        {
            towerTargets.Add(towerTransform);
        }
    }

    // this function removes the towers once they exit the range of the ddos enemy
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        //Return if the other object is not in the layer of the affected towers
        // TODO: implement this
        //if (((1 << other.gameObject.layer) & enemyMask) == 0) return;

        Transform towerTransform = other.transform;
        if (towerTargets.Contains(towerTransform)) // Only if the tower is in the List
        {
            towerTransform.gameObject.GetComponent<Tower>().removeStagger(staggerValue);
            towerTargets.Remove(towerTransform);
            staggeredTargets.Remove(towerTransform);
        }
    }


    public void staggerTurrets()
    {
        if (towerTargets == null || towerTargets.Count == 0) return;
        //Make a Copy because maybe an Tower will exit the range right in the time when we iterate the list
        List<Transform> targetsCopy = new List<Transform>(towerTargets);
        foreach (Transform target in targetsCopy)
        {
            if (target == null) continue;
            if (staggeredTargets.Contains(target)) continue;
            target.gameObject.GetComponent<Tower>().addStagger(staggerValue);
            staggeredTargets.Add(target);
        }
    }


    public void onDestroy() {
        List<Transform> targetsCopy = new List<Transform>(towerTargets);
        foreach (Transform target in targetsCopy)
        {
            if (target == null) continue;
            target.gameObject.GetComponent<Tower>().removeStagger(staggerValue);
        }

        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }

}
