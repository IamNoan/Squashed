using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }
    /*
    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene();
    }
    
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    */
    public void StartMulti()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
