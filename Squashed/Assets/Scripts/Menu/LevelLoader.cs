using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    
    public GameObject LoadingScreen;
    public Slider slider;
    public GameObject GamemodeSelector;
    public void LoadLevel(int SceneToLoad)
    {
        GamemodeSelector.SetActive(false);
        StartCoroutine(LoadAsynchronusly(SceneToLoad));
        
    }
    IEnumerator LoadAsynchronusly (int SceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad);
        
        LoadingScreen.SetActive(true);
        
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f); //parceque le load de unity s'arrête à 0,9
            slider.value = progress;
            
            yield return null;
        }
    }
}
