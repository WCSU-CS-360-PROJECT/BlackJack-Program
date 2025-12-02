using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel Model { get; private set; }

    public List<Hand> Hands { get; private set; } = new List<Hand>();
    public List<int> HandBets { get; private set; } = new List<int>();

    public int ActiveHandIndex { get; private set; } = 0;

    public int CurrentBet { get; private set; } = 0;

    /// Maximum number of hands allowed after splits
    [SerializeField] private int maxHands = 4;

    public int InsuranceBet { get; private set; } = 0;

    public Hand CurrentHand
    {
        get
        {
            if (Hands == null || Hands.Count == 0) return null;
            if (ActiveHandIndex < 0 || ActiveHandIndex >= Hands.Count) return null;
            return Hands[ActiveHandIndex];
        }
    }

    public void Initialize(int startingBBucks)
    {
        if (Model == null)
        {
            Model = new PlayerModel(startingBBucks);
        }
        else
        {
            Model.ResetForNewSession(startingBBucks);
        }

        ClearRoundState();
    }

    private void ClearRoundState()
    {
        Hands.Clear();
        HandBets.Clear();
        ActiveHandIndex = 0;
        CurrentBet = 0;
        ClearInsurance();

        // Start with a empty hand
        Hands.Add(new Hand());
        HandBets.Add(0);
    }

    public void ClearHands()
    {
        ClearRoundState();
    }

    // Sets up a single hand with that base bet.
    public void BeginRound(int betAmount)
    {
        Hands.Clear();
        HandBets.Clear();
        ActiveHandIndex = 0;
        CurrentBet = betAmount;

        Hands.Add(new Hand());
        HandBets.Add(betAmount);
    }

    public void ClearInsurance()
    {
        InsuranceBet = 0;
    }

    public void SetInsurance(int amount)
    {
        InsuranceBet = amount;
    }

    // Returns the bet for the active hand.
    public int GetCurrentHandBet()
    {
        if (HandBets == null || HandBets.Count == 0) return 0;
        if (ActiveHandIndex < 0 || ActiveHandIndex >= HandBets.Count) return 0;
        return HandBets[ActiveHandIndex];
    }

    public int GetHandBet(int index)
    {
        if (HandBets == null || index < 0 || index >= HandBets.Count) return 0;
        return HandBets[index];
    }

    // Attempts to double the bet on the current hand.
    public bool TryDoubleDownOnCurrentHand()
    {
        int current = GetCurrentHandBet();
        if (current == 0) return false;
        if (Model == null) return false;

        int totalExposure = 0;

        for (int i = 0; i < HandBets.Count; i++)
        {
            totalExposure += HandBets[i];

        }

        int newExposure = totalExposure - current + (current * 2);

        if (newExposure > Model.BBucks)
        {
            return false;
        }

        HandBets[ActiveHandIndex] = current * 2;
        return true;
    }

    /// Can the current hand be split?
    public bool CanSplitCurrentHand()
    {
        Hand hand = CurrentHand;
        if (hand == null) return false;

        // Need exactly 2 cards of the same rank.
        if (hand.Cards.Count != 2) return false;
        Card c1 = hand.Cards[0];
        Card c2 = hand.Cards[1];
        if (c1.Rank != c2.Rank) return false;

        // Do not exceed max hands.
        if (Hands.Count >= maxHands) return false;

        // Require enough B-Bucks to conceptually cover another bet.
        // For N hands with base bet B, total potential loss is N * B.
        int totalHandsAfterSplit = Hands.Count + 1;
        int required = CurrentBet * totalHandsAfterSplit;

        if (Model == null) return false;
        if (Model.BBucks < required) return false;

        return true;
    }

    //Splits hand and adds new card
    public void ApplySplitCurrentHand()
    {
        Hand hand = CurrentHand;
        if (hand == null || hand.Cards.Count < 2) return;

        // Move the second card into a new hand.
        Card second = hand.Cards[1];
        hand.Cards.RemoveAt(1);

        Hand newHand = new Hand();
        newHand.AddCard(second);

        int insertIndex = ActiveHandIndex + 1;
        Hands.Insert(insertIndex, newHand);
        HandBets.Insert(insertIndex, CurrentBet);
    }

    public void MoveToNextHand()
    {
        if (ActiveHandIndex < Hands.Count - 1)
            ActiveHandIndex++;
    }

    public bool AllHandsAreBust()
    {
        if (Hands == null || Hands.Count == 0) return false;

        foreach (var h in Hands)
        {
            if (!h.IsBust())
                return false;
        }

        return true;
    }
}
