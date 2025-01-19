using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
[Header("Tower Prefabs")]
    public GameObject[] towerPrefabs;
    private GameObject currentTowerPrefab;
    private GameObject currentTower; 

    [Header("Button References")]
    public Button[] towerButtons;

    private bool isPlacing = false;

    void Start()
    {
        foreach (var button in towerButtons)
        {
            button.onClick.AddListener(() => OnTowerButtonClick(button));
        }
    }

    void Update()
    {
        if (isPlacing && currentTower != null)
        {
            MoveTowerToMousePosition();
        }

        if (Input.GetMouseButtonDown(0) && isPlacing)
        {
            TryPlaceTower();
        }
    }

    void OnTowerButtonClick(Button button)
    {
        int towerIndex = System.Array.IndexOf(towerButtons, button);

        if (!isPlacing && towerIndex >= 0)
        {
            currentTowerPrefab = towerPrefabs[towerIndex];
            StartPlacingTower();
        } else {
            Destroy(currentTower);
            currentTowerPrefab = towerPrefabs[towerIndex];
            StartPlacingTower();
        }
    }

    void StartPlacingTower()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        currentTower = Instantiate(currentTowerPrefab, mousePosition, Quaternion.identity);
        isPlacing = true;
    }

    void MoveTowerToMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        currentTower.transform.position = mousePosition;
        if (!isPlacable())
        {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray; 
        }
        else
        {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    bool isPlacable()
    {   
        //Check if its on a Map Blocker
        Collider2D blockerCollider = Physics2D.OverlapCircle(currentTower.transform.position, 0.5f, LayerMask.GetMask("Map Blocker"));
        bool onBlocker = blockerCollider != null;

        //Check if its on the map 
        Collider2D mapCollider = Physics2D.OverlapCircle(currentTower.transform.position, 0.5f, LayerMask.GetMask("Map"));
        bool onMap = mapCollider != null;

        //Check if its on another Tower
        Collider2D towerCollider = Physics2D.OverlapCircle(currentTower.transform.position, 0.5f, LayerMask.GetMask("Tower"));
        bool onTower = towerCollider != null;

        return onMap && !onBlocker;
    }

    void TryPlaceTower()
    {
        if (isPlacable())
        {
            isPlacing = false;
            currentTower.GetComponent<Tower>().setActive(true);
        }
    }
}