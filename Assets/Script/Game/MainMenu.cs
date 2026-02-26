using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenCredits()
    {
        Debug.Log("Open credits");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}