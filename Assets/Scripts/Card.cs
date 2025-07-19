using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public int cardId;
    public Sprite backImage;
    public Image cardImage;

    private Sprite frontImage;
    private bool isFlipped = false;


    public void Init(int id, Sprite front)
    {
        cardId = id;
        frontImage = front;
        cardImage.sprite = frontImage;
    }

    public void FlipUp()
    {
        isFlipped = true;
        cardImage.sprite = frontImage;
    }

    public void FlipDown()
    {
        isFlipped = false;
        cardImage.sprite = backImage;
    }

    public void OnClick()
    {
        if (!isFlipped)
            GameManager.Instance.OnCardSelected(this);
    }
}

