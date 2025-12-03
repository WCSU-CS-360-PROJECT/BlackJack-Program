using UnityEngine;

public class DealerController : MonoBehaviour
{
    [field: SerializeField] public Hand DealerHand { get; private set; } = new Hand();

    public void ClearHand()
    {
        DealerHand.Clear();
    }

    public void AddCard(Card card)
    {
        DealerHand.AddCard(card);
    }

    public void PlayDealerTurn(Deck deck)
    {
        while (DealerHand.GetValue() < 17)
        {
            DealerHand.AddCard(deck.DrawCard());
        }
    }
}
