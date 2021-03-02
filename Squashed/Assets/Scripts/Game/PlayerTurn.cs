using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject game;

    void OnEnable()
    {
        foreach (var unit in game.GetComponent<Game>().P1unit)
        {
            unit.GetComponent<Units>().hasMoved = false;
        }
        foreach (var unit in game.GetComponent<Game>().P2unit)
        {
            unit.GetComponent<Units>().hasMoved = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
