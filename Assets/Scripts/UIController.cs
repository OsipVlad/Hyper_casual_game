using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameController gameController;
    public GameObject startMenu, gameTitle;
    public GameObject gameOverObj, scoreObj, levelObj;
    public Protection protection;
    public Text scoreText, levelText, gameOverScoreText;
    void Start()
    {
        gameController.OnStateChanged += UpdateUI;
        gameController.OnScoreChanged += (int value) => { scoreText.text = value.ToString(); };
        gameController.OnCurrentLevelChanged += (int value) => { levelText.text = value.ToString(); };
    }

    void UpdateUI(GameController.GameState state)
    {
        startMenu.SetActive(state == GameController.GameState.START);
        gameTitle.SetActive(state == GameController.GameState.START);
        gameOverObj.SetActive(state == GameController.GameState.GAME_OVER);
        ShowLevelAndScore(state == GameController.GameState.PLAY || state == GameController.GameState.GAME_OVER);
        protection.gameObject.SetActive(state == GameController.GameState.PLAY);

    }

    private void ShowLevelAndScore(bool show)
    {
        scoreObj.SetActive(show);
        levelObj.SetActive(show);
    }
}
