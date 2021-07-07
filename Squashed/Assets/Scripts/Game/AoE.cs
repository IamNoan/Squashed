using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AoE : Entity
{
    public int duration;
    GameObject game;

    // Start is called before the first frame update
    public abstract void Effect();

    void UpdateZones()
    {
        foreach (AoE z in game.GetComponent<Game>().AoEList)
        {
            //Vérifie si la zone a expiré
            if (z.duration < 1)
            {
                game.GetComponent<Game>().AoEList.Remove(z);
                Destroy(z);
            }
            else
            {
                z.duration -= 1;
                Effect();
            }
        }
        
    }
}
