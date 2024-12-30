using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attribute")] 
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; //Bullets per Second
    [SerializeField] private int baseUpgradeCost = 100;

    private float bpsBase;
    public float targetingRangeBase;

    private int level = 1;


    private Transform target;
    private float timeUntilFire;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;
        
        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.setTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
             (Vector2) transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            float distance = 0;
            RaycastHit2D furthestHit = hits[0];
            for (int i = 0; i < hits.Length; i++)
            {
                float i_distance = hits[i].transform.gameObject.GetComponent<EnemyMovement>().distanceTraveled;

                if (i_distance > distance)
                {
                    distance = i_distance;
                    furthestHit = hits[i];
                }
            }

            target = furthestHit.transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
            target.position.x - transform.position.x) * Mathf.Rad2Deg;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }
    

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        LevelManager.main.GetComponent<UIManager>().SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (baseUpgradeCost > LevelManager.main.currency) return;
        
        LevelManager.main.SpendCurrency(CalculateUpgradeCost());

        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();
        
        CloseUpgradeUI();
    }

    private int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }
    
    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

}
