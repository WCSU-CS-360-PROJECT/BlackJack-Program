using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Hand
{
    [field: SerializeField] public List<Card> Cards { get; private set; } = new List<Card>();

    public void Clear()
    {
        Cards.Clear();
    }

    public void AddCard(Card card)
    {
        if (card != null)
        {
            Cards.Add(card);
        }
    }
    public int GetValue()
    {
        int total = 0;
        int aces = 0;

        foreach (var card in Cards)
        {
            total += card.GetBaseValue();
            if (card.Rank == Rank.Ace) aces++;
        }

        // Convert Aces from 11 to 1 as needed
        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }

        return total;
    }

    public bool IsBlackjack()
    {
        return Cards.Count == 2 && GetValue() == 21;
    }

    public bool IsBust()
    {
        return GetValue() > 21;
    }
}
