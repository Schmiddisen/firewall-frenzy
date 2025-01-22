using UnityEngine;

public class BasicEnemy : Enemy
{

    void Awake() {
        setupEnemy(baseMovementSpeed, baseHealth, currencyWorth);
    }

}
