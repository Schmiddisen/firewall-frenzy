using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField]
    private Tower[] tower;

    private int selectedTower = 0;

    public void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return tower[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
}