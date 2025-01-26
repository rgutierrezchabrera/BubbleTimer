using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    void Start()
    {
        canvas.transform.GetChild(0).gameObject.SetActive(true);
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        Invoke("ShowPresents", 2.0f);
    }

    private void ShowPresents()
    {
        canvas.transform.GetChild(0).gameObject.SetActive(false);
        canvas.transform.GetChild(1).gameObject.SetActive(true);
        Invoke("ShowSigns", 2.0f);
    }

    private void ShowSigns()
    {
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        canvas.transform.GetChild(2).gameObject.SetActive(true);
        Invoke("ChangeScene", 2.0f);
    }
    private void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}