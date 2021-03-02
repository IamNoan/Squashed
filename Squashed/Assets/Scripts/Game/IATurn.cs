using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATurn : MonoBehaviour
{
    
    public GameObject playerturn;
    public GameObject iaturn;
    public GameObject game;
    public static float Abs(float x) { return x > 0 ? x : -x; }

    public static float Distance(Entity e1, Entity e2)
    {
        return Abs(e1.X - e2.X) + Abs(e1.Y - e2.Y);
    }

    public static float Distance(GameObject e1, GameObject e2)
    {
        return Abs(e1.transform.position.x - e2.transform.position.x) + Abs(e1.transform.position.y - e2.transform.position.y);
    }

    void OnEnable()
    {
    
        float pos1x;
        float pos2x;
        float pos1y;
        float pos2y;
        float dist;
        float closestx=0;
        float closesty=0;
        float deplmax = game.GetComponent<Game>().DeplacementRange;
        float distmin = game.GetComponent<Game>().DetectionRange;
        foreach (var unit2 in game.GetComponent<Game>().P2unit)
        {
            unit2.GetComponent<Units>().hasMoved = false;
            foreach (var unit1 in game.GetComponent<Game>().P1unit)
            {
                pos1x = unit1.transform.position.x;
                pos2x = unit2.transform.position.x;
                pos1y = unit1.transform.position.y;
                pos2y = unit2.transform.position.y;
                dist = Distance(unit1, unit2);
                if (dist < distmin)
                {
                    distmin = dist;
                    closestx = pos1x;
                    closesty = pos1y;
                }
            }
            if (distmin < deplmax)
            {
                unit2.transform.position = new Vector3(closestx, closesty, -1);   
            }
            //else unit2.transform.position = new Mathf.Lerp(Vector3(unitx, unity, -1), Vector3(closestx, closesty, -1), (distmin / deplmax));
            unit2.GetComponent<Units>().hasMoved = true;
        }
        
        playerturn.SetActive(true);
        iaturn.SetActive(false);
        
}
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
