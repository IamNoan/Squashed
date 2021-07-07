using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Queen : Units
{

    public GameObject actionplant;
    public GameObject nid;

    public Sprite nidteam1;
    public Sprite nidteam2;

    public void Plant()
    {
        var n = Instantiate(nid, transform.position, Quaternion.identity);
        if (team==1)
        {
            n.GetComponent<SpriteRenderer>().sprite = nidteam1;
            n.GetComponent<Building>().team = 1;
            n.GetComponent<Building>().game = game;
        }
        else
        {
            n.GetComponent<SpriteRenderer>().sprite = nidteam2;
            n.GetComponent<Building>().team = 2;
        }
        n.GetComponent<Building>().me = n;
        n.GetComponent<Building>().camera = camera;
        n.GetComponent<Building>().health = 500;

        game.GetComponent<Game>().Dens.Add(n);
        health = 0;
    }

    void Start()
    {
        type = "Queen";
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
