using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [SerializeField] public MenuTowerDetails towerDetailsUI;

    [Header("UIDocument")]
    public UIDocument uIDocument;

    public Transform startPoint;
    public Transform[] path;

    public Tower selectedTower;

    public int currency = 100;

    public void Awake()
    {
        main = this;
    }


    public void setSelectedTower(Tower tower) {
        

        // If its the same tower, just return, so that you can toggle selection
        if(tower && selectedTower && tower.gameObject == selectedTower.gameObject) {
            deselectTower();
            return;
        }
        deselectTower();

        this.selectedTower = tower;
        
        selectedTower.showRangeIndicator(true);
        towerDetailsUI.showTowerInfos(uIDocument);
    }
    public void deselectTower() {
        if (selectedTower == null) return;
        
        selectedTower.showRangeIndicator(false);
        this.selectedTower = null;
        towerDetailsUI.showTowerInfos(uIDocument);
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

}
