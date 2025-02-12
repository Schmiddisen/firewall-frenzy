using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour
{
    private Label Node_1_broken;
    private Label Node_2_broken;
    private Label Node_3_broken;
    private Label Node_4_broken;
    private Label Node_5_broken;

    public void UpdateHealthBar(UIDocument health_uIDocument, int health)
    {
        var root = health_uIDocument.rootVisualElement;
        Node_1_broken = root.Q<Label>("Node_1_broken");
        Node_2_broken = root.Q<Label>("Node_2_broken");
        Node_3_broken = root.Q<Label>("Node_3_broken");
        Node_4_broken = root.Q<Label>("Node_4_broken");
        Node_5_broken = root.Q<Label>("Node_5_broken");

        checkHealth(health);
    }

    private void checkHealth(int health)
    {
        if (Node_1_broken == null || Node_2_broken == null || Node_3_broken == null || Node_4_broken == null || Node_5_broken == null)
        {
            Debug.LogError("Eines der Labels ist null! Stelle sicher, dass die Namen in UI Toolkit stimmen.");
            return;
        }

        if (health <= 4000) Node_1_broken.RemoveFromClassList("hidden");
        if (health <= 3000) Node_2_broken.RemoveFromClassList("hidden");
        if (health <= 2000) Node_3_broken.RemoveFromClassList("hidden");
        if (health <= 1000) Node_4_broken.RemoveFromClassList("hidden");
        if (health <= 0) Node_5_broken.RemoveFromClassList("hidden");
    }

    public bool CheckIfDefeat(int health)
    {
        return health <= 0;
    }
}