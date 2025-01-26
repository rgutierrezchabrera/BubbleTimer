using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MastermindGame : MonoBehaviour
{
    // References to the UI elements
    public Button[] incrementButtons;  // 4 buttons for incrementing the input digits
    public Button[] inputDigitButtons; // 4 buttons for the input digits (0 to 5)
    public Button[] decrementButtons;  // 4 buttons for decrementing the input digits
    public Button checkButton;         // Button to submit the current input and check if it's correct

    public GameObject[] tries;  // Slots for the tries (try1, try2, ..., try8) (GameObjects containing Text)
    private int currentTry = 0;  // Current try number (0 - 7 for try1 to try8)

    // The player's current input (4 digits)
    private int[] currentInput = new int[4];

    // Secret code for testing (change to your desired secret number)
    private int[] secretCode = new int[] { 4, 2, 4, 2 };

    void Start()
    {
        // Initialize current input with default values (e.g., 0 for all)
        for (int i = 0; i < currentInput.Length; i++)
        {
            currentInput[i] = 0; // Starting value for each digit

            // Check if the button is assigned in the inspector
            if (inputDigitButtons[i] != null)
            {
                // Get the TMP_Text component for the button's label (digit)
                TMP_Text buttonText = inputDigitButtons[i].GetComponentInChildren<TMP_Text>();
                // Update the UI text with the current input digit
                buttonText.text = currentInput[i].ToString();
            }
        }

        // Set up listeners for increment and decrement buttons
        for (int i = 0; i < incrementButtons.Length; i++)
        {
            int index = i;  // Local copy of the index for the listener

            // Increment button listener
            incrementButtons[i].onClick.AddListener(() => IncrementDigit(index));

            // Decrement button listener
            decrementButtons[i].onClick.AddListener(() => DecrementDigit(index));
        }

        // Set up listener for the check button
        checkButton.onClick.AddListener(CheckCombination);
    }

    // Increment the digit at the given index
    void IncrementDigit(int index)
    {
        // Increment the digit, wrap around if it goes above 5
        currentInput[index] = (currentInput[index] + 1) % 6;

        // Update the UI to reflect the new value
        TMP_Text buttonText = inputDigitButtons[index].GetComponentInChildren<TMP_Text>();
        buttonText.text = currentInput[index].ToString();
    }

    // Decrement the digit at the given index
    void DecrementDigit(int index)
    {
        // Decrement the digit, wrap around if it goes below 0
        currentInput[index] = (currentInput[index] - 1 + 6) % 6;

        // Update the UI to reflect the new value
        TMP_Text buttonText = inputDigitButtons[index].GetComponentInChildren<TMP_Text>();
        buttonText.text = currentInput[index].ToString();
    }

    // Check if the current input matches the secret code
    void CheckCombination()
    {
        if (currentTry >= tries.Length)
        {
            Debug.Log("No more tries left.");
            return; // No more tries available
        }

        // Get the current try slot (for example, try1, try2, ..., try8)
        GameObject currentTrySlot = tries[currentTry];
        TMP_Text[] tryTexts = currentTrySlot.GetComponentsInChildren<TMP_Text>();

        // Loop through each digit in the current input
        for (int i = 0; i < currentInput.Length; i++)
        {
            // Check if the digit matches the secret code
            if (currentInput[i] == secretCode[i])
            {
                // Green: correct digit in the correct position
                tryTexts[i].color = Color.green;
            }
            else if (System.Array.Exists(secretCode, x => x == currentInput[i]))
            {
                // Yellow: correct digit in the wrong position
                tryTexts[i].color = Color.yellow;
            }
            else
            {
                // Red: incorrect digit
                tryTexts[i].color = Color.red;
            }

            // Update the try button text with the digit
            tryTexts[i].text = currentInput[i].ToString();
        }

        // After checking, move to the next try slot
        currentTry++;

        // Debug log to indicate the check is complete
        Debug.Log("Combination checked. Move to next try.");
    }
}
