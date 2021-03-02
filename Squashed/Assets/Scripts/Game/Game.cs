using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Game : MonoBehaviour
{

    #region Variables

    public GameObject playerturn;
    public GameObject iaturn;
    public GameObject game;
    public GameObject ObstaclePrefab;
    public GameObject unit1;
    public Sprite sprite1;
    public GameObject unit2;
    public Sprite sprite2;
    public Camera cam;
    public Grid gri;
    
    

    public List<GameObject> ObsList = new List<GameObject>();
    public List<(float,float)> CoordList = new List<(float,float)>();
    private List<GameObject> P1unit = new List<GameObject>();
    private List<GameObject> P2unit = new List<GameObject>();

    #endregion
    
    #region private methods

    private void AddUnits(List<GameObject> li, int team)
    {
        int x;
        Sprite spr;
        if (team==1)
        {
            x = -10;
            spr = sprite1;
        }
        else
        {
            x = 4;
            spr = sprite2;
        }
        li.Add(Instantiate(unit1,new Vector3(x,-5,-1),Quaternion.identity));
        li.Add(Instantiate(unit2,new Vector3(x,5,-1),Quaternion.identity));
        foreach (var unit in li)
        {
            unit.GetComponent<Units>().team = team;
            unit.GetComponent<Units>().camera = cam;
            unit.GetComponent<Units>().grid = gri;
            unit.GetComponent<SpriteRenderer>().sprite = spr;
            unit.GetComponent<Units>().CList = game;
            unit.GetComponent<Units>().X = unit.transform.position.x;
            unit.GetComponent<Units>().Y = unit.transform.position.y;
        }
        
    }

    private void ObstaclesInit()
    {
        float a;
        float b;
        //Vector3 SpawnCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
        //Instantiate(myPrefab,new Vector3(SpawnCenter.x,SpawnCenter.y,-1) ,Quaternion.identity);

        // Ajoute les coordonnées des obstacles.
        CoordList.Add((-1.5f, -1.5f));
        CoordList.Add((4.5f, -2.5f));
        CoordList.Add((-3.5f, 2.5f));
        CoordList.Add((2.5f, 3.5f));
        CoordList.Add((-5.5f, 4.5f));
        

        //Génère 5 Obstacles selon les coordonnées de la liste CoordList

        for (int i = 0; i < CoordList.Count; i++)
        {
            (a, b) = CoordList[i];
            ObsList.Add((GameObject)Instantiate(ObstaclePrefab, new Vector3( a, b, -1), Quaternion.identity));
        }
    }
    #endregion


    #region public methods

    public void gotoIATurn()
    {
        playerturn.SetActive(false);
        iaturn.SetActive(true);
    }

    #endregion
    
    #region MonoBehavior

    void Start()
    {
        AddUnits(P1unit,1);
        AddUnits(P2unit,2);
        ObstaclesInit();
        playerturn.SetActive(true);
        iaturn.SetActive(false);
        
    }

    #endregion
    
}
