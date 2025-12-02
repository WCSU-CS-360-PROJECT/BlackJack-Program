using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackGameController : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Betting,
        Dealing,
        PlayerTurn,
        DealerTurn,
        RoundResult,
        GameOver
    }

    [Header("References")]
    [SerializeField] private BlackjackUIController ui;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DealerController dealerController;

    [Header("Config")]
    [SerializeField] private int startingBBucks = 1000;
    [SerializeField] private int minBet = 10;
    [SerializeField] private int maxBet = 500;  // max is current balance
    [SerializeField] private float decisionTimeLimitSeconds = 60f;
    [SerializeField] private float roundResultDelaySeconds = 6f;

    private Deck deck;
    private GameState currentState;

    private bool dealerHoleHidden = true;

    // Session timer
    private float sessionTimer = 0f;
    private bool sessionTimerRunning = false;

    // Per-hand decision timer
    private float decisionTimer = 0f;
    private bool decisionTimerRunning = false;

    private void Start()
    {
        deck = new Deck();

        playerController.Initialize(startingBBucks);
        dealerController.ClearHand();

        if (ui != null)
        {
            ui.Initialize(this);
            ui.UpdateBBucks(playerController.Model.BBucks);
            ui.UpdateHighScore(playerController.Model.HighScore);
            ui.UpdateWinStreak(playerController.Model.WinStreak);

            int maxAllowed = playerController.Model.BBucks;
            ui.SetBetSliderRange(minBet, maxAllowed);
            ui.UpdateBet(minBet);

            ui.SetGameButtonsVisible(false);
            ui.SetBettingUIVisible(false);
            ui.SetStartButtonVisible(true);
            ui.UpdateTimer(0f);
            ui.UpdateSessionTime(0f);
            ui.ShowMessage("Welcome to Blackjack! Press Start!");
        }

        currentState = GameState.MainMenu;
    }

    private void Update()
    {
        if (sessionTimerRunning)
        {
            sessionTimer += Time.deltaTime;
            ui.UpdateSessionTime(sessionTimer);
        }

        if (decisionTimerRunning)
        {
            decisionTimer -= Time.deltaTime;
            if (decisionTimer < 0f)
            {
                decisionTimer = 0f;
                decisionTimerRunning = false;
                ui.UpdateTimer(decisionTimer);

                if (currentState == GameState.PlayerTurn)
                {
                    ui.ShowMessage("Time's up! You stand on this hand.");
                    OnHandCompleted(false);
                }
            }
            else
            {
                ui.UpdateTimer(decisionTimer);
            }
        }
    }

    private void RefreshUIHands(bool hideDealer = true)
    {
        if (ui == null) return;

        ui.RenderHands(
            playerController.Hands,
            playerController.ActiveHandIndex,
            dealerController.DealerHand,
            hideDealer
        );

        ui.UpdateBBucks(playerController.Model.BBucks);
        ui.UpdateHighScore(playerController.Model.HighScore);
        ui.UpdateWinStreak(playerController.Model.WinStreak);
    }

    #region UI Callbacks

    public void OnStartGamePressed()
    {
        if (currentState == GameState.MainMenu)
        {
            // First run already has model initialized in Start().
        }
        else if (currentState == GameState.GameOver)
        {
            // New run after Game Over or cashout: reset bankroll, keep highscore.
            playerController.Initialize(startingBBucks);

            ui.UpdateBBucks(playerController.Model.BBucks);
            ui.UpdateWinStreak(playerController.Model.WinStreak);
            ui.UpdateHighScore(playerController.Model.HighScore);

            sessionTimer = 0f;
            ui.UpdateSessionTime(sessionTimer);
        }
        else
        {
            return;
        }

        if (playerController.Model.IsBroke())
        {
            ui.ShowMessage("You have 0 B-Bucks. Cannot start a new game.");
            return;
        }

        sessionTimerRunning = true;
        ui.SetStartButtonVisible(false);

        GoToBettingState();
    }

    public void OnDealPressed()
    {
        if (currentState != GameState.Betting) return;

        int sliderValue = ui.GetBetSliderValue();
        int maxAllowed = playerController.Model.BBucks;

        int bet = Mathf.Clamp(sliderValue, minBet, maxAllowed);

        if (bet < minBet || bet > maxAllowed)
        {
            ui.ShowMessage($"Invalid bet amount. Min: {minBet}, Max: {maxAllowed}.");
            return;
        }

        // Initialize player hands & bets for this round.
        playerController.BeginRound(bet);

        StartRound();
    }

    public void OnHitPressed()
    {
        if (currentState != GameState.PlayerTurn) return;

        Hand hand = playerController.CurrentHand;
        if (hand == null) return;

        hand.AddCard(deck.DrawCard());
        RefreshUIHands(true);

        if (hand.IsBust())
        {
            ui.ShowMessage($"Hand #{playerController.ActiveHandIndex + 1} busts.");
            OnHandCompleted(true);
        }
    }

    public void OnStandPressed()
    {
        if (currentState != GameState.PlayerTurn) return;

        ui.ShowMessage($"Hand #{playerController.ActiveHandIndex + 1} stands.");
        OnHandCompleted(false);
    }

    public void OnDoubleDownPressed()
    {
        if (currentState != GameState.PlayerTurn) return;

        Hand hand = playerController.CurrentHand;
        if (hand == null) return;

        if (hand.Cards.Count != 2)
        {
            ui.ShowMessage("You can only double down on your first move for this hand.");
            return;
        }

        if (!playerController.TryDoubleDownOnCurrentHand())
        {
            ui.ShowMessage("Not enough B-Bucks to double down on this hand.");
            return;
        }

        // One more card, then automatically stand on this hand.
        hand.AddCard(deck.DrawCard());
        RefreshUIHands(true);

        if (hand.IsBust())
        {
            ui.ShowMessage($"Hand #{playerController.ActiveHandIndex + 1} busts on double down.");
            OnHandCompleted(true);
        }
        else
        {
            ui.ShowMessage($"Hand #{playerController.ActiveHandIndex + 1} double downs and stands.");
            OnHandCompleted(false);
        }
    }

    public void OnSplitPressed()
    {
        if (currentState != GameState.PlayerTurn) return;

        if (!playerController.CanSplitCurrentHand())
        {
            ui.ShowMessage("Cannot split this hand (need a pair, enough B-Bucks, and not at max hands).");
            return;
        }

        // Perform split of current hand into two hands with same bet.
        playerController.ApplySplitCurrentHand();

        // Deal an extra card to each of the two split hands.
        int idx = playerController.ActiveHandIndex;
        Hand firstHand = playerController.Hands[idx];
        Hand secondHand = playerController.Hands[idx + 1];

        firstHand.AddCard(deck.DrawCard());
        secondHand.AddCard(deck.DrawCard());

        // DEBUG: Force next dealt card to always be a 3------------------------------------------------------------------------------------------------------
         //firstHand.AddCard(new Card(Suit.Spades, Rank.Three));
         //secondHand.AddCard(new Card(Suit.Clubs, Rank.Three));
        // above code only to show split areas.---------------------------------------------------------------------------------------------------------------

        ui.ShowMessage($"Hand #{idx + 1} split. Now playing hand #{idx + 1}.");
        RefreshUIHands(true);
    }

    public void OnInsurancePressed()
    {
        if (currentState != GameState.PlayerTurn) return;

        if (dealerController.DealerHand.Cards.Count == 0 ||
            dealerController.DealerHand.Cards[0].Rank != Rank.Ace)
        {
            ui.ShowMessage("Insurance only available when dealer shows an Ace.");
            return;
        }

        int baseBet = playerController.CurrentBet;
        int insurance = baseBet / 2;
        if (insurance <= 0)
        {
            ui.ShowMessage("Bet too small for insurance.");
            return;
        }

        if (playerController.Model.BBucks < insurance)
        {
            ui.ShowMessage("Not enough B-Bucks for insurance.");
            return;
        }

        playerController.SetInsurance(insurance);
        ui.ShowMessage($"Insurance placed: {insurance} B-Bucks.");
    }

    public void OnCashOutPressed()
    {
        if (currentState == GameState.PlayerTurn || currentState == GameState.DealerTurn)
        {
            ui.ShowMessage("Cannot cash out in the middle of a hand.");
            return;
        }

        playerController.Model.CashOut();
        ui.UpdateHighScore(playerController.Model.HighScore);
        ui.ShowMessage($"You cashed out with {playerController.Model.BBucks} B-Bucks.");

        decisionTimerRunning = false;
        ui.UpdateTimer(0f);

        sessionTimerRunning = false;

        currentState = GameState.GameOver;
        ui.SetBettingUIVisible(false);
        ui.SetGameButtonsVisible(false);
        ui.SetCashOutEnabled(false);
        ui.SetStartButtonVisible(true);

        RefreshUIHands(false);
    }

    public void OnExitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    #region State Logic

    private void GoToBettingState()
    {
        currentState = GameState.Betting;

        playerController.ClearHands();
        dealerController.ClearHand();
        playerController.ClearInsurance();
        dealerHoleHidden = true;

        decisionTimerRunning = false;
        ui.UpdateTimer(0f);

        if (playerController.Model.BBucks < minBet)
        {
            currentState = GameState.GameOver;
            sessionTimerRunning = false;
            ui.ShowMessage("Not enough B-Bucks to meet the minimum bet. Game Over.");
            ui.SetBettingUIVisible(false);
            ui.SetGameButtonsVisible(false);
            ui.SetCashOutEnabled(false);
            ui.SetStartButtonVisible(true);
            RefreshUIHands(false);
            return;
        }

        ui.SetBettingUIVisible(true);
        ui.SetGameButtonsVisible(false);
        ui.SetCashOutEnabled(true);

        int maxAllowed = playerController.Model.BBucks;
        if (maxAllowed < minBet) maxAllowed = minBet;
        ui.SetBetSliderRange(minBet, maxAllowed);
        ui.UpdateBet(minBet);

        ui.ShowMessage("Place your bet to start a new hand.");
        RefreshUIHands(true);
    }

    private void StartRound()
    {
        currentState = GameState.Dealing;

        dealerController.ClearHand();
        dealerHoleHidden = true;

        // Initial deal to first hand and dealer
        Hand firstHand = playerController.CurrentHand;
        firstHand.AddCard(deck.DrawCard());
        dealerController.AddCard(deck.DrawCard()); // face-up
        firstHand.AddCard(deck.DrawCard());
        dealerController.AddCard(deck.DrawCard()); // face-down

        // ---------------- DEBUG: FORCE INITIAL PAIR FOR SPLIT TESTING ----------------------------------------------------------------
       // firstHand.Cards.Clear();
       // firstHand.AddCard(new Card(Suit.Hearts, Rank.Three));
       // firstHand.AddCard(new Card(Suit.Spades, Rank.Three));
        // ------------------------------------------------------------------------------------------------------------------------------

        ui.SetBettingUIVisible(false);
        ui.SetGameButtonsVisible(true);
        ui.SetCashOutEnabled(false);

        ui.ShowMessage($"Bet locked in: {playerController.CurrentBet} B-Bucks per hand.");

        RefreshUIHands(true);

        currentState = GameState.PlayerTurn;
        StartDecisionTimer();
    }

    private void StartDecisionTimer()
    {
        decisionTimer = decisionTimeLimitSeconds;
        decisionTimerRunning = true;
        ui.UpdateTimer(decisionTimer);
    }

    private void StopDecisionTimer()
    {
        decisionTimerRunning = false;
        ui.UpdateTimer(0f);
    }

    // Action after hand is completed
    private void OnHandCompleted(bool busted)
    {
        StopDecisionTimer();

        // If there is another hand to play, move to it.
        if (playerController.ActiveHandIndex < playerController.Hands.Count - 1)
        {
            playerController.MoveToNextHand();
            ui.ShowMessage($"Now playing hand #{playerController.ActiveHandIndex + 1}.");
            RefreshUIHands(true);
            StartDecisionTimer();
            return;
        }

        // All hands are done; dealer plays
        StartCoroutine(DealerTurnSequence());
    }

    private IEnumerator DealerTurnSequence()
    {
        if (currentState != GameState.PlayerTurn) yield break;

        currentState = GameState.DealerTurn;

        dealerHoleHidden = false;
        RefreshUIHands(false);
        ui.ShowMessage("Dealer reveals hand...");
        yield return new WaitForSeconds(1.0f);

        dealerController.PlayDealerTurn(deck);
        RefreshUIHands(false);

        string resultMsg = ResolveOutcomeMultiHand();
        ui.ShowMessage(resultMsg);

        yield return new WaitForSeconds(roundResultDelaySeconds);

        AfterRoundCheck();
    }

    /// Computes the net outcome for each player hand against the dealer and updates BBucks.
    // Blackjack logic (3:2 payout) and multi-hand support.
    
    private string ResolveOutcomeMultiHand()
    {
        Hand dealerHand = dealerController.DealerHand;
        int dealerTotal = dealerHand.GetValue();
        bool dealerBust = dealerTotal > 21;
        bool dealerBJ = dealerHand.IsBlackjack();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // Insurance if dealer has blackjack
        if (dealerBJ && playerController.InsuranceBet > 0)
        {
            int insurancePayout = playerController.InsuranceBet * 2;
            playerController.Model.RecordWin(insurancePayout);
            sb.AppendLine($"Dealer blackjack, insurance pays {insurancePayout} B-Bucks.");
        }

        // Action for hands
        List<Hand> hands = playerController.Hands;
        for (int i = 0; i < hands.Count; i++)
        {
            Hand hand = hands[i];
            int bet = playerController.GetHandBet(i);

            if (hand == null || bet <= 0)
            {
                sb.AppendLine($"Hand {i + 1}: No bet.");
                continue;
            }

            int playerTotal = hand.GetValue();
            bool playerBJ = hand.IsBlackjack();

            string handLabel = (hands.Count > 1) ? $"Hand {i + 1}: " : "";

            // logic for blackjack
            if (playerBJ || dealerBJ)
            {
                if (playerBJ && dealerBJ)
                {
                    // Push: both have blackjack
                    playerController.Model.RecordPush();
                    sb.AppendLine($"{handLabel}Push. Both you and the dealer have blackjack.");
                }
                else if (playerBJ)
                {
                    // Player blackjack pays 3:2
                    int payout = Mathf.FloorToInt(bet * 1.5f);
                    playerController.Model.RecordWin(payout);
                    sb.AppendLine($"{handLabel}BLACKJACK! You win {payout} B-Bucks (3:2).");
                }
                else // dealerBJ && !playerBJ
                {
                    playerController.Model.RecordLoss(bet);
                    sb.AppendLine($"{handLabel}Dealer blackjack. You lose {bet} B-Bucks.");
                }

                // Blackjack hands are checked
                continue;
            }

            // Hand Actions
            if (hand.IsBust())
            {
                playerController.Model.RecordLoss(bet);
                sb.AppendLine($"{handLabel}Bust ({playerTotal}). You lose {bet} B-Bucks.");
            }
            else if (dealerBust)
            {
                playerController.Model.RecordWin(bet);
                sb.AppendLine($"{handLabel}Dealer busts ({dealerTotal}). You win {bet} B-Bucks.");
            }
            else if (playerTotal > dealerTotal)
            {
                playerController.Model.RecordWin(bet);
                sb.AppendLine($"{handLabel}You win {bet} B-Bucks. {playerTotal} vs {dealerTotal}.");
            }
            else if (playerTotal < dealerTotal)
            {
                playerController.Model.RecordLoss(bet);
                sb.AppendLine($"{handLabel}You lose {bet} B-Bucks. {playerTotal} vs {dealerTotal}.");
            }
            else
            {
                playerController.Model.RecordPush();
                sb.AppendLine($"{handLabel}Push at {playerTotal}.");
            }
        }

        return sb.ToString().Trim();
    }

    private void AfterRoundCheck()
    {
        currentState = GameState.RoundResult;
        playerController.ClearInsurance();
        dealerHoleHidden = false;

        RefreshUIHands(false);

        if (playerController.Model.IsBroke())
        {
            currentState = GameState.GameOver;
            sessionTimerRunning = false;

            // Clear the table so no cards / hand labels are shown on the start screen
            playerController.ClearHands();
            dealerController.ClearHand();
            dealerHoleHidden = false;
            RefreshUIHands(false);   // hide hand labels

            ui.ShowMessage("You have 0 B-Bucks. Game Over.");
            ui.SetBettingUIVisible(false);
            ui.SetGameButtonsVisible(false);
            ui.SetCashOutEnabled(false);
            ui.SetStartButtonVisible(true);
            return;
        }

        GoToBettingState();
    }

    #endregion
}
