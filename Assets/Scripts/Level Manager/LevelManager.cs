using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public GameObject selectedTower;

    public int currency = 100;

    public void Awake()
    {
        main = this;
    }


    public void setSelectedTower(GameObject tower) {
        deselectTower();
        
        this.selectedTower = tower;
        
        selectedTower.GetComponent<Tower>().showRangeIndicator(true);
    }
    public void deselectTower() {
        if (selectedTower == null) return;
        
        selectedTower.GetComponent<Tower>().showRangeIndicator(false);
        this.selectedTower = null;
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
