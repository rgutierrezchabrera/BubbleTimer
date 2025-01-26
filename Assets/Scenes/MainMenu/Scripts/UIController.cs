using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void GoPlay(string siguienteEscena)
    {
        GetComponent<AudioSource>().Stop();
        if (siguienteEscena == "Game1")
        {
            SceneManager.LoadScene("Level1");
        }
        else if (siguienteEscena == "Creditos")
        {
            SceneManager.LoadScene("Credits");
        }
    }


    public void VolverMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }



    // Method to exit the game
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();             // Quits the application
    }
}
