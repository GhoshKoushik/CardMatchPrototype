using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text scoreText;
    public TMP_Text matchText;
    public TMP_Text comboText;
    public TMP_Text comboCountText;
    public TMP_Text turnText;
    public GameObject scoreGRP;
    public GameObject startGameUI;
    public GameObject nextLevelUI;
    public GameObject levelLoadUI;
    public GameObject gameOverUI;
    public GameObject gameUI;


    private void Awake()
    {
        Instance = this;
        UpdateScore(SaveSystem.GetSavedScore());
        ActivateUI(startGameUI.name);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void OnNextLevelButton()
    {
        ActivateUI(gameUI.name);
        GameManager.Instance.NextLevel();
    }

    public void OnRestartButton()
    {
        ActivateUI(gameUI.name);
        GameManager.Instance.RestartGame();
    }

    public void OnGameOverButton()
    {
        ActivateUI(levelLoadUI.name);
        ActivateScoreStatus(false);
        levelLoadUI.GetComponent<LevelDetailsUI>().LoadLevelButttons();
    }

    public void OnStartGameButton()
    {
       ActivateUI(levelLoadUI.name);
       levelLoadUI.GetComponent<LevelDetailsUI>().LoadLevelButttons();
    }

    public void OnLevelLoadUIButton()
    {
        ActivateUI(levelLoadUI.name);
        ActivateScoreStatus(false);
        levelLoadUI.GetComponent<LevelDetailsUI>().LoadLevelButttons();
    }

    public void ActivateUI(string uiToBeActivated)
    {

        gameUI.SetActive(uiToBeActivated.Equals(gameUI.name));
        startGameUI.SetActive(uiToBeActivated.Equals(startGameUI.name));
        nextLevelUI.SetActive(uiToBeActivated.Equals(nextLevelUI.name));
        levelLoadUI.SetActive(uiToBeActivated.Equals(levelLoadUI.name));
        gameOverUI.SetActive(uiToBeActivated.Equals(gameOverUI.name));

    }

    public void ActivateScoreStatus(bool isActive)
    {
        scoreGRP.SetActive(isActive);
    }

    public void ResetLevels()
    {
       SaveSystem.ResetProgress();
       UpdateScore(SaveSystem.GetSavedScore());
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
