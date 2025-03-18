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

        newGameButton.clicked += LoadLevelSelectScene;
        exitGameButton.clicked += ExitGame;

        Slider musicSlider = uIDocument.rootVisualElement.Q<Slider>("volume_slider");
        Slider sfxSlider = uIDocument.rootVisualElement.Q<Slider>("sfx_slider");

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

    private void LoadLevelSelectScene()
    {
        AudioManager.main.playButtonClick();
        SceneManager.LoadScene("Level_Selection");
    }

    private void ExitGame()
    {
        AudioManager.main.playButtonClick();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}