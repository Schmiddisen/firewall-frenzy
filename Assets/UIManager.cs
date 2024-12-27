using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    private bool isHoveringUI;

    private void Awake()
    {
        
    }

    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
