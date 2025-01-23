using UnityEngine;

public class Virus : Enemy
{

    void Awake() {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }

}
