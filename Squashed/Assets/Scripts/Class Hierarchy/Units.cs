using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Units : Entity
{
    public int movingRange;
    public int attackingRange;
    public int accuracy;
    public List<Units> units;
    public bool hasMoved;
    
    public Camera camera;
    public Grid grid;
    public bool clicked;
    public bool exited;
    private Vector3 mousepos;
    public GameObject CList;
    //portée de déplacement
    private float MoveRange;
    private float MemoryRange;
    public GameObject bluecase;
    public List<GameObject> CaseList = new List<GameObject>();

    #region Methods

    private static bool IsPresent(List<(float,float)> liste, (float,float) couple)
    {
        bool found = false;
        int i = 0;
        while (i < liste.Count)
        {
            if (couple == liste[i])
            {
                found = true;
                i += liste.Count;
            }
            i += 1;
        }
        return found;
    }

    //valeur absolue
    private float Abs(float x)
    {
        if (x > 0) return x;
        else return (-x);
    }

    // Dans le cas d'un déplacement strictement Vertical:
    // Vérifie si il a un cube entre la position du joueur et la case ciblée
    public bool VertiCheck(float px, float py, float my)
    {
        bool Ofound = false;
        float a;
        float b;
        int longueur = CList.GetComponent<Game>().CoordList.Count;

        for (int i = 0; i < longueur; i++)
        {
            (a, b) = CList.GetComponent<Game>().CoordList[i];

            if (a == px)
            {
                if ((py <= b && b <= my) || (my <= b && b <= py))
                {
                    Ofound = true;
                    i += longueur;
                }
            }
        }
        return Ofound;
    }

    // Dans le cas d'un déplacement strictement Horizontal:
    // Vérifie si il a un cube entre la position du joueur et la case ciblée
    public bool HorizCheck(float px, float py, float mx)
    {
        bool Ofound = false;
        float a;
        float b;
        int longueur = CList.GetComponent<Game>().CoordList.Count;

        for (int i = 0; i < longueur; i++)
        {
            (a, b) = CList.GetComponent<Game>().CoordList[i];

            if (b == py)
            {
                if ((px <= a && a <= mx) || (mx <= a && a <= px))
                {
                    Ofound = true;
                    i += longueur;
                }
            }
        }
        return Ofound;
    }
    
     // fonction qui initie toutes les cases
    private void ZonePrinter()
    {
        float px = transform.position.x;
        float py = transform.position.y;
        // C'est pas beau mais c'est temporaire

        for (int i = 0; i < 7; i++)
        {
            CaseList.Add(Instantiate(bluecase, new Vector3(px, py + 3 - i, -1), Quaternion.identity));
        }
        CaseList.Add(Instantiate(bluecase, new Vector3(px-1,py+2, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px+1, py+2, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-2, py+1, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-1, py+1, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px+1, py+1, -1), Quaternion.identity));

        CaseList.Add(Instantiate(bluecase, new Vector3(px+2, py+1, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-3, py, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-2, py, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-1, py, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px+1, py, -1), Quaternion.identity));

        CaseList.Add(Instantiate(bluecase, new Vector3(px+2, py, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px+3, py, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-2, py-1, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-1, py-1, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px+1, py-1, -1), Quaternion.identity));

        CaseList.Add(Instantiate(bluecase, new Vector3(px+2, py-1, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px-1, py-2, -1), Quaternion.identity));
        CaseList.Add(Instantiate(bluecase, new Vector3(px+1, py-2, -1), Quaternion.identity)); 
    }

    //Cette fonction déplace toutes les cases avec le joueur (utilisée dans l'Update de mouvement)
    private void ZoneMover(float mouvementX, float mouvementY)
    {
        foreach (GameObject C in CaseList)
        {
            C.transform.position = new Vector3(C.transform.position.x + mouvementX, C.transform.position.y + mouvementY, 0);
        }
    }

    //fonction qui détermine quand et quelle case afficher.
    private void Shower()
    {
        
        //Quand l'unité a été cliquée et attend une direction.
        foreach (GameObject C in CaseList)
        {
            if (IsPresent(CList.GetComponent<Game>().CoordList, (C.transform.position.x, C.transform.position.y)))
            {
                C.SetActive(false);
            }
            else C.SetActive(true);
        }

        //Ne pas oublier de mettre un foreach qui clear les cases après le déplacement dans l'update
    }
    #endregion

    #region MonoBehavior

    //Start

    private void Start()
    {
        ZonePrinter();
        foreach (GameObject C in CaseList)
        {
            C.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        MoveRange = 3;
        MemoryRange = MoveRange;

        if (clicked)
        {
            Shower();
        }

        if (clicked && Input.GetMouseButtonDown(0) && !hasMoved && exited)
        {
            //Récupère la position de la souris par rapport à la camera
            mousepos = camera.ScreenToWorldPoint(Input.mousePosition);
            //Permet de changer la position en position par rapport aux tiles (mousepos x et y de base ne contiennent pas les +0.5f)
            mousepos = grid.WorldToCell(mousepos);

            //Check si le déplacement est strictement vertical
            if (mousepos.x + 0.5f - transform.position.x == 0 && VertiCheck(transform.position.x, transform.position.y, mousepos.y +0.5f))
            {
                MoveRange -= 2;
                if (MoveRange < 1) MoveRange = 1;
            }

            //Check si le déplacement est strictement horizontal
            if(mousepos.y +0.5f - transform.position.y == 0 && HorizCheck(transform.position.x,transform.position.y, mousepos.x +0.5f))
            {
                MoveRange -= 2;
                if (MoveRange < 1) MoveRange = 1;
            }
            
            

            //Check si la case est à portée et si elle est occupée par un obstacle.
            if (Abs(mousepos.x + 0.5f - transform.position.x) + Abs(mousepos.y + 0.5f - transform.position.y) <= MoveRange && !IsPresent(CList.GetComponent<Game>().CoordList, (mousepos.x + 0.5f, mousepos.y + 0.5f)))
            {

                // Ce foreach désactive toutes les cases une fois le mouvement fait
                foreach (GameObject C in CaseList)
                {
                    C.SetActive(false);
                }

                ZoneMover(mousepos.x + 0.5f - transform.position.x, mousepos.y + 0.5f - transform.position.y);

                //Change la position sur la case ou le joueur a cliqué
                transform.position = new Vector3(mousepos.x + 0.5f, mousepos.y + 0.5f, 0);
                hasMoved = true;
                
            }
            MoveRange = MemoryRange;
        }

        if (hasMoved)
        {
            this.GetComponent<SpriteRenderer>().color = Color.gray;
            clicked = false;
            exited = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Input.GetMouseButtonDown(1))
        {
            clicked = false;
            exited = false;
        }
    }

    void OnMouseDown()
    {
        clicked = true;
    }

    private void OnMouseExit()
    {
        if (clicked)
        {
            exited = true;
        }
    }

   

    #endregion
}

