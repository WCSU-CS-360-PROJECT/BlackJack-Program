using System;

public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public enum Rank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

[System.Serializable]
public class Card
{
    public Suit Suit;
    public Rank Rank;

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int GetBaseValue()
    {
        if (Rank == Rank.Ace) return 11;
        if (Rank >= Rank.Two && Rank <= Rank.Ten) return (int)Rank;
        return 10; // J, Q, K
    }

    public string GetSpriteKey()
    {
        string suitLetter = Suit switch
        {
            Suit.Hearts => "H",
            Suit.Diamonds => "D",
            Suit.Clubs => "C",
            Suit.Spades => "S",
            _ => "?"
        };

        string rankLetter = Rank switch
        {
            Rank.Ace => "A",
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            _ => ((int)Rank).ToString()   // 2–10
        };

        return suitLetter + rankLetter;   // ex: "HA", "D4", "C10"
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
