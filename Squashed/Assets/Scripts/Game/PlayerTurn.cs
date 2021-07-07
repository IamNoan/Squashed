using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject game;
    public TMP_Text turntxt;
    public TMP_Text txt;


    
    void OnEnable()
    {
        int getwood = 0;
        int getroyaljelly = 0;
        int getfood = 0;
        foreach (var den in game.GetComponent<Game>().Dens)
        {
            getwood += den.GetComponent<Building>().woodbyturn;
            getfood += den.GetComponent<Building>().foodbyturn;
            getroyaljelly += den.GetComponent<Building>().royaljellybyturn;
        }
        game.GetComponent<Game>().actualturn++;
        game.GetComponent<Game>().wood += getwood;
        game.GetComponent<Game>().woodbyturn = getwood;
        game.GetComponent<Game>().royaljelly += getroyaljelly;
        game.GetComponent<Game>().rjbyturn = getroyaljelly;
        game.GetComponent<Game>().food += getfood;
        game.GetComponent<Game>().foodbyturn = getfood;

        int getwoodIA = 0;
        int getroyaljellyIA = 0;
        int getfoodIA = 0;
        foreach (var den in game.GetComponent<Game>().DensIA)
        {
            getwoodIA += den.GetComponent<Building>().woodbyturn;
            getfoodIA += den.GetComponent<Building>().foodbyturn;
            getroyaljellyIA += den.GetComponent<Building>().royaljellybyturn;
        }
        game.GetComponent<Game>().woodIA += getwoodIA;
        game.GetComponent<Game>().woodbyturnIA = getwoodIA;
        game.GetComponent<Game>().royaljellyIA += getroyaljellyIA;
        game.GetComponent<Game>().rjbyturnIA = getroyaljellyIA;
        game.GetComponent<Game>().foodIA += getfoodIA;
        game.GetComponent<Game>().foodbyturnIA = getfoodIA;

        txt.text = "Turn "+game.GetComponent<Game>().actualturn;
        StartCoroutine(TurnTextFade());
        foreach (var unit in game.GetComponent<Game>().P1unit)
        {
            unit.GetComponent<Units>().hasMoved = false;
        }
        foreach (var unit in game.GetComponent<Game>().P2unit)
        {
            unit.GetComponent<Units>().hasMoved = true;
        }
        CheckEndGame();
    }
    
    private void CheckEndGame()
    {
        int i = 0;
        foreach (var unit in game.GetComponent<Game>().P1unit)
        {
            if (unit.GetComponent<Units>().type=="Queen")
            {
                i++;
            }
        }
        int j = 0;
        foreach (var unit in game.GetComponent<Game>().P2unit)
        {
            if (unit.GetComponent<Units>().type=="Queen")
            {
                j++;
            }
        }

        if (game.GetComponent<Game>().Dens.Count == 0 && i == 0 )
        {
            
        }
        else if (game.GetComponent<Game>().DensIA.Count == 0 && i == 0)
        {
            
        }
    }

    IEnumerator TurnTextFade()
    {
        turntxt.text = "Turn : " + game.GetComponent<Game>().actualturn;
        turntxt.color = new Color(turntxt.color.r, turntxt.color.g, turntxt.color.b, 0);
        while (turntxt.color.a < 1.0f)
        {
            turntxt.color = new Color(turntxt.color.r, turntxt.color.g, turntxt.color.b, turntxt.color.a + (Time.deltaTime / 1f));
            yield return null;
        }

        yield return new WaitForSeconds(1);
        
        while (turntxt.color.a > 0.0f)
        {
            turntxt.color = new Color(turntxt.color.r, turntxt.color.g, turntxt.color.b, turntxt.color.a - (Time.deltaTime / 1f));
            yield return null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
