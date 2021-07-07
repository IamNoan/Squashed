using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void SceneChange (int SceneToChangeTo)
    {
        SceneManager.LoadScene("Menu");
    }

    public void PauseMenu(GameObject pauseMenu)
    {
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = true;
        }
        GameObject.Find("Camera").GetComponent<CameraController>().paused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame(GameObject pauseMenu)
    {
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = false;
        }

        GameObject.Find("Camera").GetComponent<CameraController>().paused = false;
        pauseMenu.SetActive(false);
    }

    public void Save()
    {
        
    }
    public void SaveAndQuit()
    {
        
        SceneManager.LoadScene("Menu");
    }
    
}
