using UnityEngine;
using System.Collections.Generic;


public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    private List<Transform> playerCards = new List<Transform>();

    [System.Serializable]
    public class CardType
    {
        public GameObject prefab;
        public int count;
    }
    
    public List<CardType> deck;

    void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            DrawCard();
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
            cards[i].localRotation = Quaternion.Euler(-120, 0, -x * 5f);
        }
    } 

    public void PlaceCard(Transform card)
    {
        playerCards.Remove(card);
        DrawCard();
    }

    public void ReturnCard(Transform card)
    {
        if (!playerCards.Contains(card))
        {
            card.SetParent(transform);
            playerCards.Add(card);
        }
    }

    void DrawCard()
    {
        int total = 0;
        foreach (var c in deck) total += c.count;

        if (total == 0) return;

        int roll = Random.Range(0, total);

        for (int i = 0; i < deck.Count; i++)
        {
            if (roll < deck[i].count)
            {
                GameObject newCard = Instantiate(deck[i].prefab, transform);
                playerCards.Add(newCard.transform);
                newCard.transform.SetParent(transform);
                newCard.transform.localScale = new Vector3(1f, 0.2f, 1f);

                CardType cardType = deck[i];
                cardType.count--;
                deck[i] = cardType;
                if (deck[i].count <= 0)
                {
                    deck.RemoveAt(i);
                }
                return;
            }
            roll -= deck[i].count;
        }
    }
}
