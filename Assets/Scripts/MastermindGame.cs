using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MastermindGame : MonoBehaviour
{
    // References to the UI elements
    [SerializeField] private Button[] incrementButtons;     // 4 buttons for incrementing the input digits
    [SerializeField] private Button[] inputDigitButtons;    // 4 buttons for the input digits (0 to 5)
    [SerializeField] private Button[] decrementButtons;     // 4 buttons for decrementing the input digits

    private float timeValue;                                // Remaining time in seconds
    private bool timerExpired;                              // If the timer has expired

    public GameObject[] tries;  // Slots for the tries (try1, try2, ..., try8) (GameObjects containing Text)
    private int currentTry = 0;  // Current try number (0 - 7 for try1 to try8)

    // The player's current input (4 digits)
    private int[] currentInput = new int[4];

    // Secret code for testing (change to your desired secret number)
    private int[] secretCode = new int[] { 4, 2, 4, 2 };
    private int[] secretCode1 = new int[] { 4, 2, 4, 2 };
    private int[] secretCode2 = new int[] { 2, 0, 2, 5 };
    private int[] secretCode3 = new int[] { 1, 2, 3, 4 };

    private bool isTimerRunning = true; // Add a flag to check if the timer should keep running

    [Header("Timer Settings")]
    [SerializeField] private int minutes = 1;               // Total game time in minutes
    [SerializeField] private Button checkButton;            // Button to submit the current input and check if it's correct
    [SerializeField] private TMP_Text timeText;             // Reference to the timer UI

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

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

        timeValue = minutes * 60;
        timerExpired = false;
        // Start the countdown
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        // Keep updating the countdown until time runs out or the timer is stopped
        while (timeValue > 0 && isTimerRunning)
        {
            yield return new WaitForSeconds(1f);  // Wait for 1 second
            timeValue--;
            UpdateTimerUI(timeValue);
        }

        if (isTimerRunning)
        {
            TimerExpired(); // Trigger timer expiration when time runs out
        }
        else
        {
            Debug.Log("Timer stopped.");
        }
    }

    void StopTimer()
    {
        // Stop the timer coroutine by setting the flag to false
        isTimerRunning = false;
        // Optionally, you could also update the timer UI here to freeze the last time shown
        UpdateTimerUI(timeValue); // Update the timer UI with the current value to freeze it
    }

    private void TimerExpired()
    {
        ShowLoseScreen();
    }

    private void ShowLoseScreen()
    {
        // Activate the lose panel
        losePanel.SetActive(true);
        // Disable other panels
        winPanel.SetActive(false);
        // Trigger scene change after 2 seconds
        StartCoroutine(WaitAndMainMenu());
    }

    private void ShowWinScreen()
    {
        // Activate the win panel
        winPanel.SetActive(true);
        // Disable other panels
        losePanel.SetActive(false);
        // Trigger scene change after 2 seconds
        StartCoroutine(WaitAndLoadLevel());
    }

    private void UpdateTimerUI(float timeToDisplay)
    {
        // Format and display the time on the UI
        if (timeToDisplay < 0) timeToDisplay = 0;

        float mins = Mathf.FloorToInt(timeToDisplay / 60);  // Calculate minutes
        float secs = Mathf.FloorToInt(timeToDisplay % 60);  // Calculate seconds

        timeText.text = string.Format("{0:00}:{1:00}", mins, secs);  // Display time as MM:SS
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
    // Check if the current input matches the secret code
    void CheckCombination()
    {
        if (currentTry >= tries.Length)
        {
            ShowLoseScreen();
            Debug.Log("Change to menuuu");
            Debug.Log("No more tries left.");
            return; // No more tries available
        }

        // Get the current try slot (for example, try1, try2, ..., try8)
        GameObject currentTrySlot = tries[currentTry];
        TMP_Text[] tryTexts = currentTrySlot.GetComponentsInChildren<TMP_Text>();

        bool allCorrect = true; // Flag to check if all digits are correct

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
                allCorrect = false; // Not all digits are correct, set the flag to false
            }
            else
            {
                // Red: incorrect digit
                tryTexts[i].color = Color.red;
                allCorrect = false; // Not all digits are correct, set the flag to false
            }

            // Update the try button text with the digit
            tryTexts[i].text = currentInput[i].ToString();
        }

        // Check if all digits are correct
        if (allCorrect)
        {
            Debug.Log("You found the right combination!"); // Notify that the correct combination was found
            ShowWinScreen(); // Optionally, call ShowWinScreen if you want to display a win panel or similar
            StopTimer(); // Stop the timer since the player won
            return; // Exit the method early to prevent the currentTry from incrementing
        }

        // After checking, move to the next try slot
        currentTry++;

        // Debug log to indicate the check is complete
        Debug.Log("Combination checked. Move to next try.");
    }

    // Coroutine to wait 2 seconds before loading the next level
    private IEnumerator WaitAndLoadLevel()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        Debug.Log("Change level");
        SceneManager.LoadScene("Level2");  // Load Level2
    }

    private IEnumerator WaitAndMainMenu()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        Debug.Log("Change to menu");
        SceneManager.LoadScene("MainMenu");  // MainMenu
    }
}
