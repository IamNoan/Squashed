using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Game : MonoBehaviour
{

    #region Variables

    //Turn Based Implementation
    public GameObject playerturn;
    public GameObject iaturn;
    
    //gameObjects Imports
    public GameObject game;
    public GameObject ObstaclePrefab;

    //Buildings imports
    public GameObject build;
    public Sprite sbuild1;
    public Sprite sbuild2;
    
    //Unit type 1
    public GameObject unit1;
    public Sprite sprite1;
    
    //Unit type 2
    public GameObject unit2;
    public Sprite sprite2;
    
    //Other imports
    public Camera cam;
    public Grid gri;
    
    //Creating utils
    public List<GameObject> ObsList = new List<GameObject>();
    public List<(float,float)> CoordList = new List<(float,float)>();
    public List<GameObject> P1unit = new List<GameObject>();
    public List<GameObject> P2unit = new List<GameObject>();
    public float DeplacementRange;
    public float DetectionRange;

    #endregion
    
    #region private methods

    private void AddUnits(List<GameObject> li, int team)
    {
        float x;
        Sprite spr;
        if (team==1)
        {
            x = -10.5f;
            spr = sprite1;
        }
        else
        {
            x = 4.5f;
            spr = sprite2;
        }
        
        //Creation des fourmis a partir des prefab
        li.Add(Instantiate(unit1,new Vector3(x,-5.5f,-1),Quaternion.identity));
        li.Add(Instantiate(unit2,new Vector3(x,5.5f,-1),Quaternion.identity));
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

    private void BuildingsInit(int team)
    {
        Sprite spr;
        float x;
        if (team == 1)
        {
            spr = sbuild1;
            x = -17.5f;
        }
        else
        {
            spr = sbuild2;
            x = 8.5f;
        }

        //Create a new building and change his parameters
        var newbuild = Instantiate(build, new Vector3(x,0.5f,-1), Quaternion.identity);
        newbuild.GetComponent<Building>().X = transform.position.x;
        newbuild.GetComponent<Building>().Y = transform.position.y;
        newbuild.GetComponent<Building>().health = 500;
        newbuild.GetComponent<Building>().team = team;
        newbuild.GetComponent<Building>().cost = 20;
        newbuild.GetComponent<SpriteRenderer>().sprite = spr;
        
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
        BuildingsInit(1);
        BuildingsInit(2);
        playerturn.SetActive(true);
        iaturn.SetActive(false);
        
    }

    #endregion
    
}
