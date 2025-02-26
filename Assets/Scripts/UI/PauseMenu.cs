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

        Slider musicSlider = pauseMenuUIDocument.rootVisualElement.Q<Slider>("volume_slider");
        Slider sfxSlider = pauseMenuUIDocument.rootVisualElement.Q<Slider>("sfx_slider");

        musicSlider.value = AudioManager.main.getMusicVolume();
        sfxSlider.value = AudioManager.main.getSFXVolume();
        
        musicSlider.RegisterValueChangedCallback(evt =>
        {
            AudioManager.main.setMusicVolume(evt.newValue);
        });

        sfxSlider.RegisterValueChangedCallback(evt =>
        {
            AudioManager.main.setSFXVolume(evt.newValue);
        });


    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // If Game over, then dont open the menu and pause / unpause game
            if (LevelManager.main.gameOver) return;
            if (LevelManager.main.isPaused) {
                closePauseMenu();
            } else {
                openPauseMenu();
            }
        }
    }

    public void openPauseMenu() {
        AudioManager.main.playButtonClick();
        LevelManager.main.pauseGame(true);
        pauseMenu.SetEnabled(true);
        pauseMenu.RemoveFromClassList("hidden");
    }
    
    public void closePauseMenu() {
        AudioManager.main.playButtonClick();
        LevelManager.main.pauseGame(false);
        pauseMenu.SetEnabled(false);
        pauseMenu.AddToClassList("hidden");
    }

    private void restarGame() {
        AudioManager.main.playButtonClick();
        //Function for restarting the game
    }

    private void exitGame() {
        AudioManager.main.playButtonClick();
        SceneManager.LoadScene("Main_Menu");
        AudioManager.main.isLevelMusic = false;
        AudioManager.main.playMainTheme();
    }

}
