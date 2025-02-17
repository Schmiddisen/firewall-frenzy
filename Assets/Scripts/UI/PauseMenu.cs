using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("PauseButton")]
    public UIDocument pauseButtonUIDocument;
    [Header("PauseMenu")]
    public UIDocument pauseMenuUIDocument;

    private VisualElement pauseMenu;

    void Awake()
    {
        var root = pauseMenuUIDocument.rootVisualElement;
        pauseMenu = root.Q<VisualElement>("Pause_Menu_initial");
        pauseMenu.SetEnabled(false);

        Button pauseButton = pauseButtonUIDocument.rootVisualElement.Q<Button>("pause_button");
        pauseButton.clicked += () => openPauseMenu();

        Button btnContinue = root.Q<Button>("Continue_Game_Button");
        Button btnRestartGame = root.Q<Button>("Restart_Game_Button");
        Button btnExitGame = root.Q<Button>("Exit_Game_Button");

        btnContinue.clicked += () => closePauseMenu();
        btnRestartGame.clicked += () => restarGame();
        btnExitGame.clicked += () => exitGame();

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("ESCAPE");
            if (LevelManager.main.isPaused) {
                closePauseMenu();
            } else {
                openPauseMenu();
            }
        }
    }

    public void openPauseMenu() {
        LevelManager.main.pauseGame(true);
        pauseMenu.SetEnabled(true);
        pauseMenu.RemoveFromClassList("hidden");
    }
    
    public void closePauseMenu() {
        LevelManager.main.pauseGame(false);
        pauseMenu.SetEnabled(false);
        pauseMenu.AddToClassList("hidden");
    }

    private void restarGame() {
        Debug.Log("Restart Game");
    }

    private void exitGame() {
        SceneManager.LoadScene("Main_Menu");
    }

}
