using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Gamewon : MonoBehaviour
{
    [Header("GamewonUIDocument")]
    public UIDocument GamewonUIDocument;

    void OnEnable()
    {
        Button restart_game = GamewonUIDocument.rootVisualElement.Q<Button>("restart_game");
        Button exit_game = GamewonUIDocument.rootVisualElement.Q<Button>("exit_game");

        restart_game.clicked += () => restartGame();
        exit_game.clicked += () => exitGame();
    }

    void restartGame()
    {
        AudioManager.main.playButtonClick();
        AudioManager.main.stopSFX("game_win");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void exitGame()
    {
        AudioManager.main.playButtonClick();
        AudioManager.main.stopSFX("game_win");
        SceneManager.LoadScene("Main_Menu");
        AudioManager.main.isLevelMusic = false;
        AudioManager.main.playMainTheme();
    }
}
