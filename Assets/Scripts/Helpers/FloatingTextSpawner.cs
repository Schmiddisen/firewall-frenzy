using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject floatingTextPrefab;
    [SerializeField]
    public Canvas canvas;

    public static FloatingTextSpawner main;

    public void Awake()
    {
        main = this;
    }

    public void spawnFloatingText(string message, Vector2 position)
    {
        //Debug.Log("spawn Floating text");
        GameObject instance = Instantiate(floatingTextPrefab, canvas.transform);
        
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.position = position;

        FloatingText floatingText = instance.GetComponent<FloatingText>();
        floatingText.SetText(message);
    }
}
