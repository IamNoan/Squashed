using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageZone : AoE
{
    public int ZoneAttack; //Puissance de la zone
    public AoE Mobject;

    public MageZone()
    {

    }

    // Start is called before the first frame update
    public void InstantiateMageZone (MageZone MZone, Vector3 position, int Attack, int team = 2)
    {
        MZone.ZoneAttack = Attack;
        MZone.GetComponent<AoE>().duration = 3;

        var u = Instantiate(MZone, position, Quaternion.identity);

        Vector3 position2 = new Vector3(position.x - 1, position.y - 1, -1);
        var u2 = Instantiate(MZone, position2, Quaternion.identity);
        Vector3 position3 = new Vector3(position.x - 1, position.y, - 1);
        var u3 = Instantiate(MZone, position3, Quaternion.identity);
        Vector3 position4 = new Vector3(position.x - 1, position.y + 1, -1);
        var u4 = Instantiate(MZone, position4, Quaternion.identity);

        Vector3 position5 = new Vector3(position.x, position.y - 1, -1);
        var u5 = Instantiate(MZone, position5, Quaternion.identity);
        Vector3 position6 = new Vector3(position.x, position.y + 1, -1);
        var u6 = Instantiate(MZone, position6, Quaternion.identity);
        
        Vector3 position7 = new Vector3(position.x + 1, position.y - 1, -1);
        var u7 = Instantiate(MZone, position7, Quaternion.identity);
        Vector3 position8 = new Vector3(position.x + 1, position.y, - 1);
        var u8 = Instantiate(MZone, position8, Quaternion.identity);
        Vector3 position9 = new Vector3(position.x + 1, position.y + 1, -1);
        var u9 = Instantiate(MZone, position9, Quaternion.identity);

        
    }

    public override void Effect()
    {
        if (team == 1)
        {
            foreach (var U in game.GetComponent<Game>().P1unit)
            {
                if (U.GetComponent<Units>().transform.position.x == this.transform.position.x)
                {
                    if (U.GetComponent<Units>().transform.position.y == this.transform.position.y)
                    {
                        //Mettre un joli effet visuel peut-être?
                        U.GetComponent<Units>().health -= ZoneAttack;
                    }
                }
            }
        }
        else
        {
            foreach (var U in game.GetComponent<Game>().P2unit)
            {
                if (U.GetComponent<Units>().transform.position.x == this.transform.position.x)
                {
                    if (U.GetComponent<Units>().transform.position.y == this.transform.position.y)
                    {
                        //Mettre un joli effet visuel peut-être?
                        U.GetComponent<Units>().health -= ZoneAttack;
                    }
                }
            }
        }
    }

}
