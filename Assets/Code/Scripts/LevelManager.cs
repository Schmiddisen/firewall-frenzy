using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform startPoint;
    public Transform[] path;

    public int monero;

    private void Awake(){
        main=this;
    }

    private void Start(){
        monero = 100;
    }

    public void IncreaseMonero(int amount){
        monero += amount;
    }

    public bool SpendMonero(int amount){
        if (amount <= monero){
            // buy
            monero -= amount;
            return true;
        } else {
            Debug.Log("Broke!");
            return false;
        }
    }

}
