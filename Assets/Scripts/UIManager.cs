using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text scoreText;
    public GameObject startGameUI;
    public GameObject nextLevelUI;
    public GameObject levelLoadUI;
    public GameObject gameOverUI;
    public GameObject gameUI;


    private void Awake()
    {
        Instance = this;
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

    public void OnStartGameButton()
    {
       ActivateUI(levelLoadUI.name);
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
}
