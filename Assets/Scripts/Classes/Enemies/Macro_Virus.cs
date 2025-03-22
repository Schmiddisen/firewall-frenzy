using UnityEngine;

public class Macro_Virus : Enemy
{
    private bool hasSpikes = true;
    void Awake()
    {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth, isCamouflaged, hasSpikes);
    }

}
