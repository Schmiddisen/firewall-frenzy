using System;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    
    private GameObject towerObject;
    public Turret turret;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (LevelManager.main.GetComponent<UIManager>().IsHoveringUI()) return;
        
        if (towerObject != null)
        {
            turret.OpenUpgradeUI();
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.buildCost > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.buildCost);
        
        towerObject = Instantiate(towerToBuild.towerPrefab, transform.position, Quaternion.identity);
        turret = towerObject.GetComponent<Turret>();
    }
}
