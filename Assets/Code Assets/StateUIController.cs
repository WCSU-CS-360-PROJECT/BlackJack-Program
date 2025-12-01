using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateUIController : MonoBehaviour
{
    // Getting all buttons, texts, slider, etc. to control.
    // Interactables
    public Button hitBtn;
    public Button standBtn;
    public Button doubleBtn;
    public Button splitBtn;
    public Button insuranceBtn;
    public Slider betSlider;
    public Button makeBetBtn;
    public Button cashOutBtn;

    // Texts
    public TextMeshProUGUI totalMonetLeftTB;

    public TextMeshProUGUI AmtToBetTextTB; // Literally says Amount to Bet
    public TextMeshProUGUI betSliderValueTB;
    
    public TextMeshProUGUI yourBetTB; // Literally says your bet.
    public TextMeshProUGUI currentBetAmtTB; 

    public TextMeshProUGUI gameStatusTB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Start State AND Betting/Cash Out State
    void StartState()
    {
        // NON-ACTIVES
        // Hiding all but MoneyLeft, Status, Bet Slider, Make Bet Button, Cash Out Button
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(false);
        splitBtn.gameObject.SetActive(false);
        insuranceBtn.gameObject.SetActive(false);
        yourBetTB.gameObject.SetActive(false);
        currentBetAmtTB.gameObject.SetActive(false);
        // ACTIVES
        // Button and Slider Activations
        betSlider.gameObject.SetActive(true);
        makeBetBtn.gameObject.SetActive(true);
        cashOutBtn.gameObject.SetActive(true);

        // Text Activations
        gameStatusTB.gameObject.SetActive(true);
        AmtToBetTextTB.gameObject.SetActive(true);
        betSliderValueTB.gameObject.SetActive(true);
        totalMonetLeftTB.gameObject.SetActive(true); // This should always be true.

        // Initial Text Setups
        gameStatusTB.text = "Place your bet!";
    }

    // Player Choice State
    void PlayerChoiceState()
    {
        // NON-ACTIVES
        // Hiding Bet Slider, Make Bet Button, Amount to Bet Text
        betSlider.gameObject.SetActive(false);
        makeBetBtn.gameObject.SetActive(false);
        AmtToBetTextTB.gameObject.SetActive(false);
        betSliderValueTB.gameObject.SetActive(false);

        // Should already be inactive from StartState, but just in case
        cashOutBtn.gameObject.SetActive(false);

        // ACTIVES
        // Showing Hit, Stand, Double, Split Buttons and Your Bet Text
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        doubleBtn.gameObject.SetActive(true); // if first turn
        splitBtn.gameObject.SetActive(true);  // if first turn and has a pair
        insuranceBtn.gameObject.SetActive(true); // only if dealer shows an Ace
        yourBetTB.gameObject.SetActive(true);
        currentBetAmtTB.gameObject.SetActive(true);
        // Update Game Status
        gameStatusTB.text = "Your turn! Choose an action.";
    }


    // Dealer Turn State - While Dealer is giving cards. NO INTERACTABLES
    void DealerTurnState()
    {
        // NON-ACTIVES
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(false);
        splitBtn.gameObject.SetActive(false);
        insuranceBtn.gameObject.SetActive(false);
        // ACTIVES
        // Update Game Status
        // BetSetCanvas Should be inactive already.
        gameStatusTB.text = "Dealer's turn...";
    }

    // Game Over State
    void GameOverState()
    {
        // NON-ACTIVES
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(false);
        splitBtn.gameObject.SetActive(false);
        insuranceBtn.gameObject.SetActive(false);
        yourBetTB.gameObject.SetActive(false);
        currentBetAmtTB.gameObject.SetActive(false);
        // ACTIVES
        cashOutBtn.gameObject.SetActive(true);
        // Update Game Status
        gameStatusTB.text = "Game Over! Cash out or play again.";
    }



}
