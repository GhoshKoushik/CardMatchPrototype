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

    private List<Card> cards = new List<Card>();
    private Card firstSelected, secondSelected, thirdSelected, fourthSelected;
    private bool isCheckingMatch = false;
    private bool isComboActive = false;
    public int currentLevelIndex = 0;
    private int score = 0;
    private Transform grid;

    private void Awake()
    {
        Instance = this;
        grid = gridReference.transform;
    }

    private void Start()
    {
        StartCoroutine(LoadLevel(currentLevelIndex));
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        ClearBoard();

        grid.GetComponent<DynamicGridScaler>().UpdateGrid(
            levelsData.levels[currentLevelIndex].rows,
            levelsData.levels[currentLevelIndex].columns,
            levelsData.levels[currentLevelIndex].cardSpacing,
            levelsData.levels[currentLevelIndex].padding
        );

        List<int> ids = new();
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
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelsData.levels.Count)
        {
           StartCoroutine(LoadLevel(currentLevelIndex));
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }


    public void OnCardSelected(Card selected)
    {
        if (isCheckingMatch)
            isComboActive = true;

        selected.FlipUp();
        AudioManager.Instance.Play(SoundType.Flip);
        if (isComboActive)
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

    IEnumerator CheckMatch()
    {
        if (!isComboActive)
        { 
            isCheckingMatch = true;
            yield return new WaitForSeconds(0.5f);
            if (firstSelected.cardId == secondSelected.cardId)
            {
                // Matched..score will be updated
                AudioManager.Instance.Play(SoundType.Match);
                firstSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                secondSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);

            }
            else
            {
                firstSelected.FlipDown();
                secondSelected.FlipDown();
                AudioManager.Instance.Play(SoundType.Fail);
            }
            isCheckingMatch = false;
            firstSelected = null;
            secondSelected = null;
        }
        else
        {
            isComboActive = false;
            yield return new WaitForSeconds(0.5f);
            if (thirdSelected.cardId == fourthSelected.cardId)
            {
                // Matched..score will be updated
                AudioManager.Instance.Play(SoundType.Match);
                thirdSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                fourthSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
                score += 10; // Update score for combo match
            }
            else
            {
                thirdSelected.FlipDown();
                fourthSelected.FlipDown();
                AudioManager.Instance.Play(SoundType.Fail);
            }
            thirdSelected = null;
            fourthSelected = null;
        }
    }
}
