using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public GameObject cardPrefab;
    public Transform gridReference;
    public List<Sprite> cardSprites;

    private List<Card> cards = new List<Card>();
    private Card firstSelected, secondSelected;
    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateCards();
    }

    void CreateCards()
    {
        cards.Clear();

        List<int> ids = new List<int>();
        for (int i = 0; i < cardSprites.Count; i++)
        {
            ids.Add(i);
            ids.Add(i); // Add pair
        }
        Shuffle(ids);

        for (int i = 0; i < ids.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, gridReference);
            Card card = newCard.GetComponent<Card>();
            card.Init(ids[i], cardSprites[ids[i]]);
            cards.Add(card);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
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
