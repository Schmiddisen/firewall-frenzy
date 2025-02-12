using UnityEngine;
using TMPro; // Falls du TextMeshPro benutzt

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float fadeDuration = 1.5f;

    [SerializeField]
    public TextMeshProUGUI text;
    [SerializeField]
    public RectTransform rectTransform;
    private float timer = 0f;

    public void SetText(string message)
    {
        text.text = message;
    }

    void Update()
    {
        timer += Time.deltaTime;

        rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);

        text.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
