using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        LevelManager.main.GetComponent<UIManager>().SetHoveringState(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        LevelManager.main.GetComponent<UIManager>().SetHoveringState(false);
        gameObject.SetActive(false);
    }
}
