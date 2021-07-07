using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Sniper : Units
{
    public GameObject actionattack;
    
    void Start()
    {
        paused = false;
        foreach (GameObject C in CaseList)
        {
            C.SetActive(false);
        }

    }
    
    void OnMouseDown()
    {
        if (!paused)
        {
            if (game.GetComponent<Game>().WaitForAction == false && !isMoving)
            {
                StartCoroutine(waitbeforeactions());
            }
        }
    }

    IEnumerator waitbeforeactions()
    {
        yield return new WaitForSeconds(0.2f);
        Actions();
    }
}
