using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Units
{
    //Stats de base:

    /*BaseHealth = 80;
    BaseDefense = 0.1f;
    BaseAttack = 5;
    BaseArmorPen = 0.1f;
    BaseAccuracy = 80;
    BaseCritRate = 10;*/

    public int SpellCastRange = 3;
    public int SpellCoolDown = 5;
    public int SpellRange = 6;

    static bool isCasting = false;

    

    //On l'appelle pour faire spawn un nouveau soldat à x,y
    /*
    public Mage(int x, int y)
    {
        this.movingRange = 2;
        this.attackingRange = 5;
        InitStats();
        //Rajouter un Instantiate à x y
    }
    */

    // Pas utile pour l'instant
    /*private void InitButtons()
    {
        MenuList = new List<GameObject>(); //Liste des boutons de la fourmi
        var vect = new Vector3(440, 195);

        GameObject newCanvas = Instantiate(_canvas); //Canvas contenant les boutons
        GameObject newButtonObj = Instantiate(_btnobj); //Objet tenant le script ButtonsActions
        newButtonObj.GetComponent<ButtonsActions>().game = game;
        newButtonObj.GetComponent<ButtonsActions>().actualunit = me;
        newButtonObj.transform.SetParent(newCanvas.transform);

        hbar = Instantiate(hbar, camera.WorldToScreenPoint(new Vector3(transform.position.x,transform.position.y+ 1)), Quaternion.identity);
        hbar.transform.SetParent(newCanvas.transform);

        foreach (var action in ActionList) //Crée tous les types d'actions puis les désactive
        {
            GameObject but = Instantiate(action, vect, Quaternion.identity);
            but.transform.SetParent(newCanvas.transform); //Met les boutons dans un canvas
            switch (action.name)
            {
                case "Déplacer":
                    but.GetComponent<Button>().onClick.AddListener(newButtonObj.GetComponent<ButtonsActions>().move); //Change l'action du bouton
                    break;
                case "Attaquer":
                    but.GetComponent<Button>().onClick.AddListener(newButtonObj.GetComponent<ButtonsActions>().attack); //Change l'action du bouton
                    break;
                case "Fin":
                    but.GetComponent<Button>().onClick.AddListener(newButtonObj.GetComponent<ButtonsActions>().endTurn); //Change l'action du bouton
                    break;
                case "Annuler":
                    but.GetComponent<Button>().onClick.AddListener(newButtonObj.GetComponent<ButtonsActions>().cancel); //Change l'action du bouton
                    break;
                //Rajouter le bouton Spell ici

            }

            but.SetActive(false);

            MenuList.Add(but);
            vect.y += 22;
        }
    }*/

    //Va poser une zone de dégàt à l'endroit indiqué 
    public void Spell()
    {
        //On remplacera clicked par selected une fois qu'on l'aura implémenté
        if (isCasting && !this.hasMoved)
        {

            //Vérifie la portée de spellcast
            if (Abs(this.mousepos.x + 0.5f - this.transform.position.x) + Abs(this. mousepos.y + 0.5f - this.transform.position.y) <= SpellRange)
            {
                game.GetComponent<MageZone>().InstantiateMageZone(new MageZone(),new Vector3(this.mousepos.x + 0.5f, this.mousepos.y + 0.5f), 2, AttackPower);
                //Appeler les instantiate zone ici
            }
            // Si l'endroit cliqué est trop loin
            else
            {
                //Afficher "Out of the unit's range ou un truc du genre
            }
        }
    }

    void Start()
    {
        paused = false;
        foreach (GameObject C in CaseList)
        {
            C.SetActive(false);
        }
        //InitActions();
        //InitButtons();

    }

    void OnMouseDown()
    {
        if (game.GetComponent<Game>().WaitForAction == false && !isMoving)
        {
            Actions();
        }

    }
}
