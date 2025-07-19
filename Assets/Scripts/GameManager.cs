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
    private Card firstSelected, secondSelected;
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
            // You can show win UI here
        }
    }


public void OnCardSelected(Card selected)
    {
        selected.FlipUp();

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

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstSelected.cardId == secondSelected.cardId)
        {
            // Matched..score will be updated
            firstSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            secondSelected.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
        }
        else
        {
            firstSelected.FlipDown();
            secondSelected.FlipDown();
        }

        firstSelected = null;
        secondSelected = null;
    }
}
