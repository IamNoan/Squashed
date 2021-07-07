using System;
using System.Collections;
using System.Collections.Generic;
using Class_Hierarchy;
using TMPro;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    #region Variables

    //Turn Based Implementation
    public GameObject playerturn;
    public GameObject iaturn;
    
    //Player ressources
    public int royaljelly;
    public int wood;
    public int rjbyturn;
    public int woodbyturn;
    public int food;
    public int foodbyturn;

    /// <summary>
    /// IA Ressources
    /// </summary>
    public int royaljellyIA;
    public int woodIA;
    public int rjbyturnIA;
    public int woodbyturnIA;
    public int foodIA;
    public int foodbyturnIA;

    public uint actualturn;
    

    //Buildings imports
    public GameObject build;
    public Sprite sbuild1;
    public Sprite sbuild2;


    #region SpritesImports

    //Sprites des Obstacles et la liste qui les contient
    public Sprite Owater1;
    public Sprite Owater2;
    
    public Sprite OdeepHole;
    public Sprite Odeadtreedeep;
    public Sprite OdeephillsL;
    public Sprite OdeephillsL2;
    public Sprite Odeephillsm;
    public Sprite OdeepRock;
    
    public Sprite Obush1;
    public Sprite Obush2;
    public Sprite Olog1;
    public Sprite Olog2;
    public Sprite Olog3;
    public Sprite Osmallpond;
    public Sprite OcutTree;
    public Sprite OgrassRocks;

    public Sprite Ocactuar;
    public Sprite Orock;
    public Sprite Odeadtree;
    public Sprite Ocactus1;
    public Sprite Ocactus2;
    public Sprite Obones1;
    public Sprite Obones2;
    public Sprite OBigRock1;
    public Sprite OBigRock2;
    public Sprite OBigRock3;
    public Sprite OBigRock4;
    public Sprite OBigRock5;
    public Sprite OBigRock6;
    public Sprite OBigRock7;
    public Sprite OBigRock8;
    public Sprite OBigRock9;

    
    public List<Sprite> SpriteList = new List<Sprite>();
    
    public Tilemap Tm;
    
    #endregion

    #region unitsimports

    /// <summary>
    /// Ants
    /// </summary>
    public GameObject rifleman;
    public GameObject sargeant;
    public GameObject beetle;
    public GameObject sniper;
    public GameObject engineer;
    public GameObject queen;

    public List<GameObject> antslist = new List<GameObject>();

    /// <summary>
    /// Termites
    /// </summary>
    public GameObject fyrd;
    public GameObject shaman;
    public GameObject ranger;
    public GameObject cataphract;
    public GameObject earthworm;
    public GameObject royalcouple;

    public List<GameObject> termiteslist = new List<GameObject>();
    
    #endregion


    //Other imports
    public GameObject attackcase;
    public TMP_Text goldtext;
    public TMP_Text woodtext;
    public TMP_Text foodtext;
    
    //Creating utils
    public List<(float,float)> CoordList = new List<(float,float)>();
    public List<(float, float)> SubCoordList = new List<(float, float)>();
    public List<GameObject> P1unit = new List<GameObject>();
    public List<GameObject> P2unit = new List<GameObject>();
    public List<GameObject> Dens = new List<GameObject>();
    public List<GameObject> DensIA = new List<GameObject>();
    
    public float DeplacementRange;
    public float DetectionRange;
    public List<AoE> AoEList = new List<AoE>();

    public bool WaitForAction; //When true, cannot start another action

    private bool pausemenu;

    public GameObject endturnbut;
    #endregion
    
    #region private methods
    
    

    private void InitUnitLists()
    {
        antslist.Add(rifleman);
        antslist.Add(sargeant);
        antslist.Add(beetle);
        antslist.Add(sniper);
        antslist.Add(engineer);
        antslist.Add(queen);
        
        termiteslist.Add(fyrd);
        termiteslist.Add(shaman);
        termiteslist.Add(ranger);
        termiteslist.Add(cataphract);
        termiteslist.Add(earthworm);
        termiteslist.Add(royalcouple);
        
    }

    private void ObstaclesInit()
    {
        //Initie la liste (jsp comment faire pour que la liste soit remplie avant le lancement donc méthode Gorille)

        SpriteList.Add(Owater1);
        SpriteList.Add(Owater2);
        SpriteList.Add(OdeepHole);
        SpriteList.Add(Odeadtreedeep);
        SpriteList.Add(OdeepRock);
        SpriteList.Add(OdeephillsL);
        SpriteList.Add(OdeephillsL2);
        SpriteList.Add(Odeephillsm);

        SpriteList.Add(Obush1);
        SpriteList.Add(Obush2);
        SpriteList.Add(Olog1);
        SpriteList.Add(Olog2);
        SpriteList.Add(Olog3);
        SpriteList.Add(Osmallpond);
        SpriteList.Add(OcutTree);
        SpriteList.Add(OgrassRocks);

        SpriteList.Add(Ocactuar);
        SpriteList.Add(Orock);
        SpriteList.Add(Odeadtree);
        SpriteList.Add(Ocactus1);
        SpriteList.Add(Ocactus2);
        SpriteList.Add(Obones1);
        SpriteList.Add(Obones2);
        SpriteList.Add(OBigRock1);

        SpriteList.Add(OBigRock2);
        SpriteList.Add(OBigRock3);
        SpriteList.Add(OBigRock4);
        SpriteList.Add(OBigRock5);
        SpriteList.Add(OBigRock6);
        SpriteList.Add(OBigRock7);
        SpriteList.Add(OBigRock8);
        SpriteList.Add(OBigRock9);

        float a;
        float b;
        Sprite S;

        // Ajoute les coordonnées des obstacles.
        Vector3Int V = new Vector3Int(0, 0, 0);

        for (int x = -100; x< 100; x++)
        {
            for (int y = -100; y< 100; y++)
            {
                V = new Vector3Int(x, y, 0);
                S = Tm.GetSprite(V);
                if (SpriteList.Contains(S))
                {
                    SubCoordList.Add(((float)x + 0.5f, (float)y + 0.5f));
                }
            }
        }

        foreach (var s in SubCoordList)
        {
            CoordList.Add(s);
        }
        V = new Vector3Int(0, 0, -1);
    }

    private void BuildingsInit(int team)
    {
        Sprite spr;
        float x;
        float y = 0.5f;
        if (team == 1)
        {
            spr = sbuild1;
            x = -45.5f;
        }
        else
        {
            spr = sbuild2;
            x = 39.5f;
            y = -8.5f;
        }

        //Create a new building and change his parameters
        var newbuild = Instantiate(build, new Vector3(x,y,0), Quaternion.identity);
        newbuild.GetComponent<Building>().team = team;
        newbuild.GetComponent<SpriteRenderer>().sprite = spr;
        newbuild.GetComponent<Building>().me = newbuild;
        if (team==1)
        {
            Dens.Add(newbuild);
        }
        else
        {
            DensIA.Add(newbuild);
        }
        
    }
    #endregion
    
    #region public methods

    
    /// <summary>
    /// Can be called to create a new unit
    /// </summary>
    /// <param name="unit"> Prefab unit to create </param>
    /// <param name="position"> Position </param>
    /// <param name="team"></param>
    public void InstantiateUnit(GameObject unit, Vector3 position,int team)
    {
        Debug.Log("InstantiateUnit");

        GameObject u;
        if (team==1)
        {
            u = Instantiate(unit, new Vector3(position.x,position.y,-1), Quaternion.identity);
            P1unit.Add(u);
        }
        else
        {
            u = Instantiate(unit, new Vector3(position.x,position.y,-1), Quaternion.identity);
            P2unit.Add(u);
        }
        
        unit.GetComponent<Units>().team = team;
        unit.GetComponent<Entity>().me = unit;
        unit.GetComponent<Units>().hasMoved = true;
        u.GetComponent<Units>().InitStats(unit);
    }
    
    //On appelle cette méthode pour mettre à jour les positions connues par coordlist (comprenant obstacles, bases et unitées)
    //Appelée:
    // - En début de jeu (après que CoordList soit initialisée)
    // - à chaque fois qu'une nouvelle unité apparaît
    // - à chaque fois qu'une unité meurt
    // - à chaque fois qu'une unité est déplacée.
    public void CoordUpdate()
    {
        List<(float, float)> NCoordList = new List<(float, float)>();

        // à partir d'ici on met à jour chaque coordonnée.     (vérifier si les bases sont inclues ou non. à ajuster selon si elles sont inclues dans allunits)
        foreach (var V in P1unit)
        {
            NCoordList.Add((V.GetComponent<Units>().transform.position.x, V.GetComponent<Units>().transform.position.y));
        }

        foreach (var V in P2unit)
        {
            NCoordList.Add((V.GetComponent<Units>().transform.position.x, V.GetComponent<Units>().transform.position.y));
        }

        CoordList = NCoordList;
    }
    
    
    public void gotoIATurn()
    {
        playerturn.SetActive(false);
        iaturn.SetActive(true);
    }

    #endregion
    
    #region MonoBehavior

    void Start()//Démarrage du jeu
    {
        InitUnitLists();
        ObstaclesInit();
        BuildingsInit(1);
        BuildingsInit(2);
        playerturn.SetActive(true);
        iaturn.SetActive(false);
        
        CoordUpdate();

        royaljelly = 0;
        wood = 20;
        food = 20;
        rjbyturn = 0;
        woodbyturn = 0;
        foodbyturn = 0;
        
        royaljellyIA = 0;
        woodIA = 20;
        foodIA = 20;
        rjbyturnIA = 0;
        woodbyturnIA = 0;
        foodbyturnIA = 0;

        pausemenu = false;
        
    }

    private void Update()
    {
        goldtext.text = "Total Royal Jelly : " + royaljelly;
        woodtext.text = "Total wood : " + wood;
        foodtext.text = "Total food : " + food;
        if (WaitForAction)
        {
            endturnbut.SetActive(false);
        }
        else
        {
            endturnbut.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Menu").GetComponent<Button>().onClick.Invoke();
        }
        
        
    }

    

    #endregion

    #region Saving

    public void Save()
    {
        
    }

    public void Load()
    {
        
    }
    #endregion
}
