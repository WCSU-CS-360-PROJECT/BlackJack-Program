using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private readonly List<Card> cards = new List<Card>();
    private System.Random rng = new System.Random();

    public Deck()
    {
        BuildStandardDeck();
        Shuffle();
    }

    private void BuildStandardDeck()
    {
        cards.Clear();
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        // Fisher–Yates shuffle
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            BuildStandardDeck();
            Shuffle();
        }

        int lastIndex = cards.Count - 1;
        Card c = cards[lastIndex];
        cards.RemoveAt(lastIndex);
        return c;
    }
}
