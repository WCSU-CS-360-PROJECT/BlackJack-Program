using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles all UI: text, buttons, sliders, and card rendering.
public class BlackjackUIController : MonoBehaviour
{
    [Header("Text (TMP)")]
    [SerializeField] private TMP_Text messageText;       // StatusTB
    [SerializeField] private TMP_Text bBucksText;        // TotalMoneyLeftTB
    [SerializeField] private TMP_Text highScoreText;     // HighScoreTB
    [SerializeField] private TMP_Text timerText;         // TimerTB (decision timer)
    [SerializeField] private TMP_Text sessionTimeText;   // SessionTimeTB (session timer)
    [SerializeField] private TMP_Text winStreakText;     // WinStreakTB
    [SerializeField] private TMP_Text betText;           // BetAmtTB ("$x")

    [Header("Hand Labels")]
    [SerializeField] private TMP_Text activeHandLabel;   // label near main hand
    [SerializeField] private TMP_Text splitHand1Label;   // label near splitHand1Parent
    [SerializeField] private TMP_Text splitHand2Label;   // label near splitHand2Parent
    [SerializeField] private TMP_Text splitHand3Label;   // label near splitHand3Parent

    [Header("Buttons")]
    [SerializeField] private Button startGameButton;     // StartGameBtn
    [SerializeField] private Button dealButton;          // SetBetBtn ("BET!")
    [SerializeField] private Button hitButton;           // HITbtn
    [SerializeField] private Button standButton;         // STANDbtn
    [SerializeField] private Button doubleButton;        // DoubleDownBtn
    [SerializeField] private Button splitButton;         // SplitBtn
    [SerializeField] private Button insuranceButton;     // InsuranceBtn
    [SerializeField] private Button cashOutButton;       // CashOutBtn
    [SerializeField] private Button exitButton;          // ExitBtn

    [Header("Bet UI")]
    [SerializeField] private Slider betSlider;           // BetSlider

    [Header("Card Rendering")]
    [SerializeField] private Transform playerCardsParent;    // Active hand 
    [SerializeField] private Transform dealerCardsParent;    // Dealer row

    // Separate parents for each extra hand
    [SerializeField] private Transform splitHand1Parent;     // 1st extra hand
    [SerializeField] private Transform splitHand2Parent;     // 2nd extra hand
    [SerializeField] private Transform splitHand3Parent;     // 3rd extra hand

    [SerializeField] private GameObject cardPrefab;          // CardPrefab (UI Image)

    private BlackjackGameController game;

    public void Initialize(BlackjackGameController controller)
    {
        game = controller;
        WireButtons();
        WireSlider();
    }

    private void WireButtons()
    {
        if (startGameButton != null)
            startGameButton.onClick.AddListener(() => game.OnStartGamePressed());

        if (dealButton != null)
            dealButton.onClick.AddListener(() => game.OnDealPressed());

        if (hitButton != null)
            hitButton.onClick.AddListener(() => game.OnHitPressed());

        if (standButton != null)
            standButton.onClick.AddListener(() => game.OnStandPressed());

        if (doubleButton != null)
            doubleButton.onClick.AddListener(() => game.OnDoubleDownPressed());

        if (splitButton != null)
            splitButton.onClick.AddListener(() => game.OnSplitPressed());

        if (insuranceButton != null)
            insuranceButton.onClick.AddListener(() => game.OnInsurancePressed());

        if (cashOutButton != null)
            cashOutButton.onClick.AddListener(() => game.OnCashOutPressed());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => game.OnExitPressed());
    }

    private void WireSlider()
    {
        if (betSlider != null)
        {
            betSlider.wholeNumbers = true;
            betSlider.onValueChanged.AddListener(OnBetSliderChanged);
        }
    }

    private void OnBetSliderChanged(float value)
    {
        int v = Mathf.RoundToInt(value);
        UpdateBet(v);
    }

    #region Text Updates

    public void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }

    public void UpdateBBucks(int value)
    {
        if (bBucksText != null)
            bBucksText.text = $"Balance: {value} B-Bucks";
    }

    public void UpdateHighScore(int value)
    {
        if (highScoreText != null)
            highScoreText.text = $"Highscore: {value}";
    }

    public void UpdateWinStreak(int streak)
    {
        if (winStreakText != null)
            winStreakText.text = $"Win Streak: {streak}";
    }

    public void UpdateTimer(float seconds)
    {
        if (timerText == null) return;

        if (seconds <= 0f)
            timerText.text = "Timer";
        else
            timerText.text = $"Time: {seconds:F1}s";
    }

    public void UpdateSessionTime(float seconds)
    {
        if (sessionTimeText == null) return;

        int total = Mathf.FloorToInt(seconds);
        int minutes = total / 60;
        int secs = total % 60;
        sessionTimeText.text = $"{minutes:D2}:{secs:D2}";
    }

    public void UpdateBet(int amount)
    {
        if (betText != null)
            betText.text = $"${amount}";

        if (betSlider != null && Mathf.RoundToInt(betSlider.value) != amount)
            betSlider.value = amount;
    }

    #endregion

    #region Buttons & Interactables

    public void SetBetSliderRange(int min, int max)
    {
        if (betSlider == null) return;

        betSlider.minValue = min;
        betSlider.maxValue = max;
        betSlider.wholeNumbers = true;
    }

    public int GetBetSliderValue()
    {
        if (betSlider == null) return 0;
        return Mathf.RoundToInt(betSlider.value);
    }

    public void SetActionButtonsEnabled(bool enabled)
    {
        if (hitButton != null) hitButton.interactable = enabled;
        if (standButton != null) standButton.interactable = enabled;
        if (doubleButton != null) doubleButton.interactable = enabled;
        if (splitButton != null) splitButton.interactable = enabled;
        if (insuranceButton != null) insuranceButton.interactable = enabled;
    }

    public void SetCashOutEnabled(bool enabled)
    {
        if (cashOutButton != null)
            cashOutButton.interactable = enabled;
    }

    public void SetBettingUIVisible(bool visible)
    {
        if (betSlider != null) betSlider.gameObject.SetActive(visible);
        if (dealButton != null) dealButton.gameObject.SetActive(visible);
        if (betText != null) betText.gameObject.SetActive(visible);
        if (cashOutButton != null) cashOutButton.gameObject.SetActive(visible);
    }

    public void SetGameButtonsVisible(bool visible)
    {
        if (hitButton != null) hitButton.gameObject.SetActive(visible);
        if (standButton != null) standButton.gameObject.SetActive(visible);
        if (doubleButton != null) doubleButton.gameObject.SetActive(visible);
        if (splitButton != null) splitButton.gameObject.SetActive(visible);
        if (insuranceButton != null) insuranceButton.gameObject.SetActive(visible);
    }

    public void SetStartButtonVisible(bool visible)
    {
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(visible);
    }

    #endregion

    #region Card Rendering

    private void ClearChildCards(Transform parent)
    {
        if (parent == null) return;

        for (int i = parent.childCount - 1; i >= 0; i--)
            GameObject.Destroy(parent.GetChild(i).gameObject);
    }

    private void SpawnCard(Transform parent, Card card, bool faceDown = false)
    {
        if (parent == null || cardPrefab == null || card == null) return;

        GameObject go = GameObject.Instantiate(cardPrefab, parent);
        var img = go.GetComponent<Image>();
        if (img != null)
            img.sprite = CardSpriteProvider.GetCardSprite(card, faceDown);
    }

    private void ClearHandLabels()
    {
        if (activeHandLabel != null)
        {
            activeHandLabel.text = "";
            activeHandLabel.gameObject.SetActive(false);
        }
        if (splitHand1Label != null)
        {
            splitHand1Label.text = "";
            splitHand1Label.gameObject.SetActive(false);
        }
        if (splitHand2Label != null)
        {
            splitHand2Label.text = "";
            splitHand2Label.gameObject.SetActive(false);
        }
        if (splitHand3Label != null)
        {
            splitHand3Label.text = "";
            splitHand3Label.gameObject.SetActive(false);
        }
    }

    public void RenderHands(List<Hand> playerHands, int activeHandIndex, Hand dealerHand, bool hideDealerHoleCard)
    {
        // Clear all parents & labels
        ClearChildCards(playerCardsParent);
        ClearChildCards(dealerCardsParent);
        ClearChildCards(splitHand1Parent);
        ClearChildCards(splitHand2Parent);
        ClearChildCards(splitHand3Parent);
        ClearHandLabels();

        // Hands
        if (playerHands != null && playerHands.Count > 0)
        {
            if (activeHandIndex < 0 || activeHandIndex >= playerHands.Count)
                activeHandIndex = 0;

            // Check if any hand actually has cards; if not, don't show labels
            bool hasAnyCards = false;
            for (int i = 0; i < playerHands.Count; i++)
            {
                Hand hCheck = playerHands[i];
                if (hCheck != null && hCheck.Cards.Count > 0)
                {
                    hasAnyCards = true;
                    break;
                }
            }

            if (hasAnyCards)
            {
                // Active hand
                Hand activeHand = playerHands[activeHandIndex];
                if (activeHand != null)
                {
                    foreach (var c in activeHand.Cards)
                        SpawnCard(playerCardsParent, c, false);
                }

                // Label for active area
                if (activeHandLabel != null)
                {
                    int handNumber = activeHandIndex + 1;
                    activeHandLabel.text = $"Hand {handNumber} (Active)";
                    activeHandLabel.gameObject.SetActive(true);
                }

                // Other hands 
                Transform[] extraParents = new Transform[] { splitHand1Parent, splitHand2Parent, splitHand3Parent };
                TMP_Text[] extraLabels = new TMP_Text[] { splitHand1Label, splitHand2Label, splitHand3Label };

                int extraSlot = 0;
                for (int i = 0; i < playerHands.Count; i++)
                {
                    if (i == activeHandIndex) continue;
                    if (extraSlot >= extraParents.Length) break; // ignore beyond 3 extras

                    Transform parent = extraParents[extraSlot];
                    TMP_Text label = extraLabels[extraSlot];
                    extraSlot++;

                    Hand h = playerHands[i];
                    if (h == null || parent == null) continue;

                    float cardSpacing = 30f;

                    for (int j = 0; j < h.Cards.Count; j++)
                    {
                        Card card = h.Cards[j];
                        if (card == null) continue;

                        GameObject go = GameObject.Instantiate(cardPrefab, parent);
                        var img = go.GetComponent<Image>();
                        if (img != null)
                            img.sprite = CardSpriteProvider.GetCardSprite(card, false);

                        // Space cards
                        RectTransform rt = go.GetComponent<RectTransform>();
                        if (rt != null)
                        {
                            float x = j * cardSpacing;
                            rt.anchoredPosition = new Vector2(x, 0f);
                        }
                    }

                    if (label != null && h.Cards.Count > 0)
                    {
                        int handNumber = i + 1; // same numbering used in ResolveOutcomeMultiHand
                        label.text = $"Hand {handNumber}";
                        label.gameObject.SetActive(true);
                    }
                }
            }
        }

        // Dealer Hand
        if (dealerCardsParent != null && dealerHand != null)
        {
            for (int i = 0; i < dealerHand.Cards.Count; i++)
            {
                bool faceDown = hideDealerHoleCard && (i == 1);
                SpawnCard(dealerCardsParent, dealerHand.Cards[i], faceDown);
            }
        }
    }

    #endregion
}
