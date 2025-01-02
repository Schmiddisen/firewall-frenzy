using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTower = 0;

    private void Awake(){
        main = this;
    }

    public GameObject getSelectedTower(){
        return towerPrefabs[selectedTower];
    }

}
