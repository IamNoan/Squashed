using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Entity : MonoBehaviour
{
    
    public GameObject game;
    public GameObject ActionsMenu;
    public int health;
    public int Maxhealth;
    public int team;
    public List<GameObject>[] allunits = new List<GameObject>[2];
    
    /// <summary>
    /// The object of the hpbar
    /// </summary>
    public Transform hpbar;

    protected Transform sizebar;


    public GameObject me;
    
    public bool paused;

    

    public void SetHP(int actualhp)
    {
        float size = actualhp/Maxhealth;
        StartCoroutine(ChangeHP(health,size));
    }

    IEnumerator ChangeHP(float oldhp, float newhp)
    {
        float i = oldhp/100f;
        while (i>newhp)
        {
            sizebar.localScale = new Vector3(i,1f);
            i -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    //Cette fonction est appelée a chaque update quand l'unité 0hp ou moins
    public void Death()
    {
        if (me.GetComponent<Units>().health <= 0)
        {
            if (game.GetComponent<Game>().P1unit.Contains(me))
            {
                game.GetComponent<Game>().P1unit.Remove(me);
            }

            if (game.GetComponent<Game>().P2unit.Contains(me))
            {
                game.GetComponent<Game>().P2unit.Remove(me);
            }
            Destroy(me);
            Destroy(hpbar);
            game.GetComponent<Game>().CoordUpdate();
        }
        
    }
}
