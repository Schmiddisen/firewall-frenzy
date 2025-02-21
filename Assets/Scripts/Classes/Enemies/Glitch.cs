using UnityEngine;

public class Glitch : Enemy
{
    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth, isCamouflaged = true);
    }
}