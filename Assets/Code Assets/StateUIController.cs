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
    public Slider betSlider;

    // Texts
    public TextMeshProUGUI totalMonetLeftTB;

    public TextMeshProUGUI betSliderValueTB;
    public TextMeshProUGUI makeBetAmtTB;

    public TextMeshProUGUI yourBetTB; // Literally says your bet.
    public TextMeshProUGUI currentBetAmtTB;
    public TextMeshProUGUI gameStateTB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
