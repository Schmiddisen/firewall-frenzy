using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    [Header("GameoverUIDocument")]
    public UIDocument GameoverUIDocument;

    void OnEnable()
    {
        Button restart_game = GameoverUIDocument.rootVisualElement.Q<Button>("restart_game");
        Button exit_game = GameoverUIDocument.rootVisualElement.Q<Button>("exit_game");

        restart_game.clicked += () => restartGame();
        exit_game.clicked += () => exitGame();
    }

    void restartGame()
    {
        LevelManager.main.playerHealth = 5000;
        LevelManager.main.currency = 100;
        SceneManager.LoadScene("First_Level");
    }

    void exitGame()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
