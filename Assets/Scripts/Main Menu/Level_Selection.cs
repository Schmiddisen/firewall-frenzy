using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelection : MonoBehaviour
{
    [Header("UIDocument")]
    public UIDocument uIDocument;

    private void Start()
    {
        RegisterButtons();
    }

    private void RegisterButtons()
    {
        if (uIDocument == null)
        {
            Debug.LogError("UIDocument is not loaded!");
            return;
        }

        Button level1Button = uIDocument.rootVisualElement.Q<Button>("Level1");
        Button level2Button = uIDocument.rootVisualElement.Q<Button>("Level2");
        Button level3Button = uIDocument.rootVisualElement.Q<Button>("Level3");

        if (level1Button == null || level2Button == null || level3Button == null)
        {
            Debug.LogError("One or more buttons are not found!");
            return;
        }

        level1Button.clicked += () => LoadGameScene("First_Level");
        level2Button.clicked += () => LoadGameScene("Second_Level");
        level3Button.clicked += () => LoadGameScene("Third_Level");
    }

    private void LoadGameScene(string levelName)
    {
        AudioManager.main.playButtonClick();
        SceneManager.LoadScene(levelName);
        AudioManager.main.isLevelMusic = true;
        AudioManager.main.playLevelMusic();
    }
}