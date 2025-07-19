using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public Sprite frontImage;
    public int cardId;
    public Sprite backImage;
    private bool isFlipped = false;
    public Image cardImage;

  
    public void Init(int id, Sprite front)
    {
        cardId = id;
        frontImage = front;
        FlipDown();
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

