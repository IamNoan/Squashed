using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsActions : MonoBehaviour
{
    private GameObject game;
    
    /// <summary>
    /// Don't forget to import the unit in this
    /// </summary>
    public GameObject actualunit;

    private void Start()
    {
        game = GameObject.Find("Game");
    }

    private void SameForAllButtons()
    {
        game.GetComponent<Game>().WaitForAction = false;
        foreach (Transform button in actualunit.transform.Find("ToAddToUnitsPrefabsAttack").transform.Find("ActionsMenu"))
        {
            button.gameObject.SetActive(false);
        }
        
    }
    
    public void move()
    {
        SameForAllButtons();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    { 
        yield return new WaitForSeconds(0.2f);
        actualunit.GetComponent<Units>().isMoving = true;
        Debug.Log("Ismoving");
    }
    public void attack()
    {
        SameForAllButtons();
        StartCoroutine(Attack());
    }
    
    IEnumerator Attack()
    { 
        yield return new WaitForSeconds(0.2f);
        actualunit.GetComponent<Units>().ShowPotentialTargets();
        actualunit.GetComponent<Units>().isAttacking = true;
    }
    public void endTurn()
    {
        SameForAllButtons();
        actualunit.GetComponent<Units>().hasMoved = true;
        
    }
    public void cancel()
    {
        SameForAllButtons();
    }

    //Spécific actions
    public void plantbase()
    {
        SameForAllButtons();
        actualunit.GetComponent<Queen>().Plant();
    }
    
}
