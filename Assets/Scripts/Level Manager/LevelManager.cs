using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [SerializeField] public MenuTowerDetails towerDetailsUI;
    [SerializeField] public HealthUI healthUI;

    [Header("JSON")]
    public TextAsset towerInfos;

    [Header("ShopMenuUIDocument")]
    public UIDocument shopMenuUIDocument;
    [Header("PauseButton")]
    public UIDocument pauseButtonUIDocument;
    [Header("PauseMenu")]
    public UIDocument pauseMenuUIDocument;
    [Header("HealthUIDocuments")]
    public UIDocument healthUIDocument;

    public Transform startPoint;
    public Transform[] path;

    public Tower selectedTower;

    public int currency = 100;

    public int playerHealth = 5000; // -> 1000 health for each node

    public class EnemyFinishTrackEvent : UnityEvent<int> { }
    public EnemyFinishTrackEvent OnEnemyFinishTrack;

    private bool isPaused = false;

    public void Awake()
    {
        main = this;
        towerDetailsUI.updateCurrency(shop_uIDocument, currency);
        OnEnemyFinishTrack ??= new EnemyFinishTrackEvent();
        OnEnemyFinishTrack.AddListener(EnemyFinishTrack);
    }

    public void pauseGame(bool pause) {
        isPaused = pause;
        Time.timeScale = pause ? 0: 1;
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
        towerDetailsUI.showTowerInfos(shopMenuUIDocument, towerInfos);
    }
    public void deselectTower()
    {
        if (selectedTower == null) return;

        selectedTower.showRangeIndicator(false);
        this.selectedTower = null;
        towerDetailsUI.showTowerInfos(shopMenuUIDocument, towerInfos);
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        towerDetailsUI.updateCurrency(shop_uIDocument, currency);
    }

    public void EnemyFinishTrack(int enemyHealth)
    {
        this.playerHealth -= enemyHealth;
        Debug.Log("playerHealth: " + playerHealth);
        healthUI.UpdateHealthBar(healthUIDocument, player
        );
        if (healthUI.CheckIfDefeat(playerHealth))
        {
            Debug.Log("Game Over");
        }
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
