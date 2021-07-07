using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsActionsForBase : MonoBehaviour
{
    private GameObject game;
    public GameObject actualunit;

    private void Start()
    {
        game = GameObject.Find("Game");
    }
    
    private void SameForAllButtons()
    {
        game.GetComponent<Game>().WaitForAction = false;
        actualunit.GetComponent<Building>().ActionsMenu.SetActive(false);

        actualunit.GetComponent<Building>().buttonsshown = false;
        actualunit.GetComponent<Building>().isclicked = false;
    }

    
    public void cancel()
    {
        SameForAllButtons();
    }
    
    public void info()
    {
        SameForAllButtons();
        actualunit.GetComponent<Building>().Info();
    }
    
    public void upgrade()
    {
        actualunit.GetComponent<Building>().Upgrade();
        SameForAllButtons();
    }
    
    public void hatch()
    {
        SameForAllButtons();
        actualunit.GetComponent<Building>().Hatching();
    }
    
}
