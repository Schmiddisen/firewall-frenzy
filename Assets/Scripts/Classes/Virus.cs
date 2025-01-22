using UnityEngine;

public class BasicEnemy : Enemy
{

    public void Initialize(int moveSpeed, int health, int currencyWorth) {
        base.setupEnemy(moveSpeed, health, currencyWorth);
    }

}
