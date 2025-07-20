using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public GameObject cardPrefab;
    public GridLayoutGroup gridReference;
    public LevelData levelsData;
    public float cardSpawnDelay = 0.1f;
    public float cardFlipDelay = 1.5f;
    public float comboDuration = 5f;

    private List<Card> cards = new List<Card>();
    private Card firstSelected, secondSelected, thirdSelected, fourthSelected;
    private bool isCheckingMatch = false;
    private bool multiClickHappened = false;
    public int currentLevelIndex = 0;
    private int score = 0;
    private Transform grid;
    public int totalPairs;
    private int matchedPairs;
    private int turnsTaken = 0;
    private bool isComboActive = false;
    public int comboCount = 0;
    private float timeLeft = 5f;
    private bool isTimerRunning = true;

    private void Awake()
    {
        Instance = this;
        grid = gridReference.transform;
        isTimerRunning = false;
    }

    private void Start()
    {
       score = SaveSystem.GetSavedScore();
    }
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public void RestartGame()
    {
        ClearBoard();
        StartCoroutine(LevelLoader(currentLevelIndex));
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelsData.levels.Count)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }
        currentLevelIndex = levelIndex;

        StartCoroutine(LevelLoader(currentLevelIndex));
    }
    IEnumerator LevelLoader(int levelIndex)
    {
        yield return new WaitForSeconds(0.1f);
        turnsTaken = 0;
        matchedPairs = 0;
        comboCount = 0;

        ClearBoard();
        totalPairs = levelsData.levels[currentLevelIndex].cardImages.Count;
        UIManager.Instance.turnText.text = $"Turn: {turnsTaken}";
        UIManager.Instance.matchText.text = $"Matched: {matchedPairs}/{totalPairs}";
        UIManager.Instance.comboText.text = $"Combos: {comboCount}";
        grid.GetComponent<DynamicGridScaler>().UpdateGrid(
            levelsData.levels[currentLevelIndex].rows,
            levelsData.levels[currentLevelIndex].columns,
            levelsData.levels[currentLevelIndex].cardSpacing,
            levelsData.levels[currentLevelIndex].padding
        );

        List<int> ids = new List<int>();
        for (int i = 0; i < levelsData.levels[currentLevelIndex].cardImages.Count; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        Shuffle(ids);

        for (int i = 0; i < ids.Count; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, grid);
            Card card = cardGO.GetComponent<Card>();
            card.Init(ids[i], levelsData.levels[currentLevelIndex].cardImages[ids[i]]);
            cards.Add(card);
            yield return new WaitForSeconds(cardSpawnDelay);
        }

        yield return new WaitForSeconds(cardFlipDelay);
        foreach (Card card in cards)
        {
            card.FlipDown();
        }
    }

    void ClearBoard()
    {
        foreach (Transform child in grid)
            Destroy(child.gameObject);
        cards.Clear();
        firstSelected = null;
        secondSelected = null;
        thirdSelected = null;
        fourthSelected = null;
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        
        if (currentLevelIndex < levelsData.levels.Count)
        {
           StartCoroutine(LevelLoader(currentLevelIndex));
        }
        else
        {
            Debug.Log("All levels completed!");
            UIManager.Instance.ActivateUI(UIManager.Instance.gameOverUI.name);
            AudioManager.Instance.Play(SoundType.GameOver);
        }
    }


    public void OnCardSelected(Card selected)
    {
        if (isCheckingMatch)
            multiClickHappened = true;

        selected.FlipUp();
        AudioManager.Instance.Play(SoundType.Flip);
        if (multiClickHappened)
        {
            if (thirdSelected == null)
            {
                thirdSelected = selected;
            }
            else if (fourthSelected == null)
            {
                fourthSelected = selected;
                StartCoroutine(CheckMatch());
            }
        }
        else
        { 
            if (firstSelected == null)
            {
                firstSelected = selected;
            }
            else
            {
                secondSelected = selected;
                StartCoroutine(CheckMatch());
            }
        }
    }

    public void OnMatchSuccess()
    {
        matchedPairs++;
        if(isComboActive)
        {
            comboCount++;
            score += 50; 
            timeLeft = comboDuration; 
        }
        else
        {
            isComboActive = true;
            timeLeft = comboDuration;
            isTimerRunning = true;
            score += 10;
        }
        UIManager.Instance.UpdateScore(score);
        SaveSystem.SaveScore(score);
        UIManager.Instance.matchText.text = $"Matched: {matchedPairs}/{totalPairs}";
        UIManager.Instance.comboText.text = $"Combos: {comboCount}";
        if (matchedPairs >= totalPairs)
        {
            OnAllCardsMatched();
        }
    }

    void OnAllCardsMatched()
    {
        Debug.Log("All cards matched!");
        matchedPairs = 0; // Reset matched pairs for next level
        SaveSystem.UnlockNextLevel(currentLevelIndex);
        isTimerRunning = false;
        isComboActive = false;
        UIManager.Instance.comboCountText.text = "Combo Timer: 0";
        SaveSystem.SaveLevelTurn(currentLevelIndex, turnsTaken);
        SaveSystem.SaveLevelCombo(currentLevelIndex, comboCount);
        // Show win screen or move to next level
        StartCoroutine(LevelComplete());
    }

    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ActivateUI(UIManager.Instance.nextLevelUI.name);
        AudioManager.Instance.Play(SoundType.LevelComplete);
    }
    IEnumerator CheckMatch()
    {
        if (!multiClickHappened)
        { 
            isCheckingMatch = true;
            yield return new WaitForSeconds(0.5f);
            if (firstSelected.cardId == secondSelected.cardId)
            {
                turnsTaken++;
                AudioManager.Instance.Play(SoundType.Match);
                UIManager.Instance.turnText.text = $"Turn: {turnsTaken}";
                firstSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                secondSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                OnMatchSuccess();

            }
            else
            {
                turnsTaken++;
                firstSelected.FlipDown();
                secondSelected.FlipDown();
                UIManager.Instance.turnText.text = $"Turn: {turnsTaken}";
                AudioManager.Instance.Play(SoundType.Fail);
                isTimerRunning = false;
                isComboActive = false;
                UIManager.Instance.comboCountText.text = "Combo Timer: 0";

            }
            isCheckingMatch = false;
            firstSelected = null;
            secondSelected = null;
        }
        else
        {
            multiClickHappened = false;
            yield return new WaitForSeconds(0.5f);
            if (thirdSelected.cardId == fourthSelected.cardId)
            {
                // Matched..score will be updated
                turnsTaken++;
                AudioManager.Instance.Play(SoundType.Match);
                UIManager.Instance.turnText.text = $"Turn: {turnsTaken}";
                thirdSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                fourthSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                OnMatchSuccess();
            }
            else
            {
                turnsTaken++;
                thirdSelected.FlipDown();
                fourthSelected.FlipDown();
                UIManager.Instance.turnText.text = $"Turn: {turnsTaken}";
                AudioManager.Instance.Play(SoundType.Fail);
                isTimerRunning = false;
                isComboActive = false;
                UIManager.Instance.comboCountText.text = "Combo Timer: 0";
            }
            thirdSelected = null;
            fourthSelected = null;
        }
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            isTimerRunning = false;
            Debug.Log("5 seconds over!");
        }

        if (UIManager.Instance.comboCountText)
        {
            UIManager.Instance.comboCountText.text = "Combo Timer: " + Mathf.CeilToInt(timeLeft).ToString();
        }
    }
}
