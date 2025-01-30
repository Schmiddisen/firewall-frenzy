using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [SerializeField] public MenuTowerDetails towerDetailsUI;

    [Header("JSON")]
    public TextAsset towerInfos;

    [Header("UIDocument")]
    public UIDocument uIDocument;

    public Transform startPoint;
    public Transform[] path;

    public Tower selectedTower;

    public int currency = 100;

    public int playerHealth = 300; // -> 100 health for each node

    public class EnemyFinishTrackEvent : UnityEvent<int> { }
    public EnemyFinishTrackEvent OnEnemyFinishTrack;

    public void Awake()
    {
        main = this;

        OnEnemyFinishTrack ??= new EnemyFinishTrackEvent();
        OnEnemyFinishTrack.AddListener(EnemyFinishTrack);
    }


    public void setSelectedTower(Tower tower)
    {

        // If its the same tower, just return, so that you can toggle selection
        if (tower && selectedTower && tower.gameObject == selectedTower.gameObject)
        {
            deselectTower();
            return;
        }
        deselectTower();

        this.selectedTower = tower;
        selectedTower.showRangeIndicator(true);
        towerDetailsUI.showTowerInfos(uIDocument, towerInfos);
    }
    public void deselectTower()
    {
        if (selectedTower == null) return;

        selectedTower.showRangeIndicator(false);
        this.selectedTower = null;
        towerDetailsUI.showTowerInfos(uIDocument, towerInfos);
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public void EnemyFinishTrack(int enemyHealth)
    {
        this.playerHealth -= enemyHealth;
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
