using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI currencyUI;

    private bool isMenuOpen = true;


    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
    }
    private void OnGUI()
    {
        //currencyUI.text = LevelManager.main.currency.ToString();
    }
    
}
