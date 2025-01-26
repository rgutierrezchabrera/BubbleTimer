using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsFinish : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CreditsEnd());
    }

    IEnumerator CreditsEnd()
    {
        yield return new WaitForSeconds(16f);
        SceneManager.LoadScene("MainMenu");
    }
}
