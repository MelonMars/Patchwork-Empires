using UnityEngine;
using System.Collections.Generic;


public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    private List<Transform> playerCards = new List<Transform>();

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            card.name = "Card " + (i + 1);
            playerCards.Add(card.transform);
        }
    }
    void Update()
    {
        LayoutHand(playerCards);
    }

    void LayoutHand(List<Transform> cards)
    {
        float spacing = 0.8f;
        float start = -spacing * (cards.Count - 1) / 2f;

        for (int i = 0; i < cards.Count; i++)
        {
            float x = start + i * spacing;
            cards[i].localPosition = new Vector3(x, 0, 0);
            cards[i].localRotation = Quaternion.Euler(60, 0, -x * 5f);
        }
    }

    public void PlaceCard(Transform card)
    {
        playerCards.Remove(card);
    }
    
    public void ReturnCard(Transform card)
    {
        if (!playerCards.Contains(card))
        {
            card.SetParent(transform);
            playerCards.Add(card);
        }
    }
}
