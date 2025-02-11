using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Game_Builder : MonoBehaviour
{
    [Header("UIDocument")]
    public UIDocument uIDocument;

    private void Start()
    {
        RegisterButton();
    }

    private void RegisterButton()
    {
        if (uIDocument == null)
        {
            Debug.LogError("UIDocument ist nicht zugewiesen!");
            return;
        }

        Button newGameButton = uIDocument.rootVisualElement.Q<Button>("New_Game_Button");
        Button exitGameButton = uIDocument.rootVisualElement.Q<Button>("Exit_Game_Button");

        if (newGameButton == null)
        {
            Debug.LogError("Button mit dem Namen 'New_Game_Button' wurde nicht gefunden!");
            return;
        }

        if (exitGameButton == null)
        {
            Debug.LogError("Button mit dem Namen 'Exit_Game_Button' wurde nicht gefunden!");
            return;
        }

        newGameButton.clicked += LoadGameScene;
        exitGameButton.clicked += ExitGame;
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("First_Level");
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}