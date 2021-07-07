using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class IATurn : MonoBehaviour
{
    List<Vector2> targetlist = new List<Vector2>();
    public bool winning;
    public int wood;
    public int gold;
    public GameObject playerturn;
    public GameObject iaturn;
    public GameObject game;
    public Camera camera;
    public int nbcontrolpoints;
    public int turntimer;
    public int undercontrol;
    List<GameObject> Queens = new List<GameObject>();

    public static List<Units> seenlist = new List<Units>();

    public float Abs(float x) { return x > 0 ? x : -x; }
    private bool IsPresent(List<Vector2> list, Vector2 coo)
    {
        bool found = false;
        int i = 0;
        while (i < list.Count&&!found)
        {
            if (coo == list[i])
            {
                found = true;
                i += list.Count;
            }
            i += 1;
        }
        return found;
    }
    public Vector2 targeting(GameObject u, GameObject t)
    {
        bool left=true;
        bool up=true;
        float X = u.transform.position.x;
        float Y = u.transform.position.y;
        Vector2 target = u.transform.position;
        
        float deplmax = game.GetComponent<Game>().DeplacementRange;
        for (int i=0; i<deplmax;i++)
        {
            if (u.transform.position.x - t.transform.position.x > 0)
                left = true;
            else
                left = false;


            if (u.transform.position.y - t.transform.position.y > 0)
                up = false;
            else
                up = true;

            if (left)
            {
                if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X - 1, Y)))
                {
                    X = X - 1;
                    i++;
                    if (up)
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y + 1)))
                        {
                            Y = Y + 1;
                            i++;
                        }
                    }
                    else
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y - 1)))
                        {
                            Y = Y - 1;
                            i++;
                        }
                    }
                }
            }
            else
            {
                if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X + 1, Y)))
                {
                    X = X + 1;
                    i++;

                    if (up)
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y + 1)))
                        {
                            Y = Y + 1;
                            i++;
                        }
                    }
                    else
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y - 1)))
                        {
                            Y = Y - 1;
                            i++;
                        }
                    }
                }
            }
        }
        return new Vector2(X, Y);
    }
    public Vector2 targeting(GameObject u, Vector3 t)
    {
        bool left = true;
        bool up = true;
        float X = u.transform.position.x;
        float Y = u.transform.position.y;
        Vector2 target = u.transform.position;
        
        float deplmax = game.GetComponent<Game>().DeplacementRange;
        for (int i = 0; i < deplmax; i++)
        {
            if (u.transform.position.x - t.x > 0)
                left = true;
            else
                left = false;


            if (u.transform.position.y - t.y > 0)
                up = false;
            else
                up = true;
            if (left)
            {
                if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X - 1, Y)))
                {
                    X = X - 1;
                    i++;
                    if (up)
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y + 1)))
                        {
                            Y = Y + 1;
                            i++;
                        }
                    }
                    else
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y - 1)))
                        {
                            Y = Y - 1;
                            i++;
                        }
                    }
                }
            }
            else
            {
                if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X + 1, Y)))
                {
                    X = X + 1;
                    i++;

                    if (up)
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y + 1)))
                        {
                            Y = Y + 1;
                            i++;
                        }
                    }
                    else
                    {
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (X, Y - 1)))
                        {
                            Y = Y - 1;
                            i++;
                        }
                    }
                }
            }
        }
        return new Vector2(X, Y);
    }
    public bool IstargetValid(Vector2 t)
    {
        return (!IsPresent(game.GetComponent<Game>().SubCoordList, (t.x, t.y)));
    }
    private bool IsPresent(List<(float, float)> liste, (float, float) couple)
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
    public Vector2 arrondi(Vector2 vect)
    {
        Double X = (Double)vect.x;
        Double Y = (Double)vect.y;
        return new Vector2(((float)Math.Truncate(X)) + 0.5f, ((float)Math.Truncate(Y)) + 0.5f);
    }
    public int estimate()
    {
        return seenlist.Count;
    }
    public float buildingratio()
    {
        float u = (float)game.GetComponent<Game>().DensIA.Count;
        float n = (u + (float)game.GetComponent<Game>().Dens.Count);
        return (u / n);
    }
    public float Occupation()
    {
        float u = (float)undercontrol;
        float n = (float)nbcontrolpoints;
        return (u / n);
    }

    public float Distvect(Vector3 vect1, Vector3 vect2)
    {
        return Abs(vect1.x - vect2.x) + Abs(vect1.y - vect2.y);
    }
    public float Distance(GameObject e1, GameObject e2)
    {
        return Abs(e1.transform.position.x - e2.transform.position.x) + Abs(e1.transform.position.y - e2.transform.position.y);
    }

    public void Attack(GameObject u)
    {
        u.GetComponent<Entity>().health -= 50;
        
        /*u.GetComponent<Entity>().hbar.value -= 50f;
        if (u.GetComponent<Entity>().hbar.value <= 0f)
        {
            removefromseenlist(u);
            //u.GetComponent<Entity>().Death();
        }
*/

    }
    public void removefromseenlist(GameObject u)
    {
        int len = seenlist.Count;
        bool done = false;

        for (int i = 0; i < len && !done; i++)
        {
            if (u.GetComponent<Units>() == seenlist[i])
            {
                seenlist.RemoveAt(i);
                done = true;
            }

        }
    }
    public void SpawnUnit()
    {
        List<GameObject> De = game.GetComponent<Game>().DensIA;
        bool occ = false;
        foreach (GameObject D in De)
        {
            foreach (var unit2 in game.GetComponent<Game>().P2unit)
            {
                if (unit2.transform.position.x == D.transform.position.x && unit2.transform.position.y == D.transform.position.y)
                {
                    occ = true;
                    break;
                }

            }
            if (!occ)
            {
                game.GetComponent<Game>().P2unit.Add(Instantiate(game.GetComponent<Game>().fyrd, new Vector3(D.transform.position.x, D.transform.position.y, -1), Quaternion.identity));
                game.GetComponent<Game>().foodIA -= 50;
                game.GetComponent<Game>().CoordUpdate();
            }
        }
    }
    public void SpawnRanger()
    {
        List<GameObject> De = game.GetComponent<Game>().DensIA;
        bool occ = false;
        foreach (GameObject D in De)
        {
            foreach (var unit2 in game.GetComponent<Game>().P2unit)
            {
                if (unit2.transform.position.x == D.transform.position.x && unit2.transform.position.y == D.transform.position.y)
                {
                    occ = true;
                    break;
                }

            }
            if (!occ)
            {
                game.GetComponent<Game>().P2unit.Add(Instantiate(game.GetComponent<Game>().ranger, new Vector3(D.transform.position.x, D.transform.position.y, -1), Quaternion.identity));
                game.GetComponent<Game>().foodIA -= 20;
                game.GetComponent<Game>().woodIA -= 5;
                game.GetComponent<Game>().CoordUpdate();
            }
        }
    }
    public void SpawnWorm()
    {
        List<GameObject> De = game.GetComponent<Game>().DensIA;
        bool occ = false;
        foreach (GameObject D in De)
        {
            foreach (var unit2 in game.GetComponent<Game>().P2unit)
            {
                if (unit2.transform.position.x == D.transform.position.x && unit2.transform.position.y == D.transform.position.y)
                {
                    occ = true;
                    break;
                }

            }
            if (!occ)
            {
                game.GetComponent<Game>().P2unit.Add(Instantiate(game.GetComponent<Game>().earthworm, new Vector3(D.transform.position.x, D.transform.position.y, -1), Quaternion.identity));
                game.GetComponent<Game>().foodIA -= 40;
                game.GetComponent<Game>().CoordUpdate();
            }
        }
    }
    public void SpawnShaman()
    {
        List<GameObject> De = game.GetComponent<Game>().DensIA;
        bool occ = false;
        foreach (GameObject D in De)
        {
            foreach (var unit2 in game.GetComponent<Game>().P2unit)
            {
                if (unit2.transform.position.x == D.transform.position.x && unit2.transform.position.y == D.transform.position.y)
                {
                    occ = true;
                    break;
                }

            }
            if (!occ)
            {
                game.GetComponent<Game>().P2unit.Add(Instantiate(game.GetComponent<Game>().shaman, new Vector3(D.transform.position.x, D.transform.position.y, -1), Quaternion.identity));
                game.GetComponent<Game>().foodIA -= 50;
                game.GetComponent<Game>().CoordUpdate();
            }
        }
    }
    public void SpawnQueen()
    {
        List<GameObject> De = game.GetComponent<Game>().DensIA;
        bool occ = false;
        foreach (GameObject D in De)
        {
            foreach (var unit2 in game.GetComponent<Game>().P2unit)
            {
                if (unit2.transform.position.x == D.transform.position.x && unit2.transform.position.y == D.transform.position.y)
                {
                    occ = true;
                    break;
                }

            }
            if (!occ)
            {
                GameObject queen = (Instantiate(game.GetComponent<Game>().royalcouple, new Vector3(D.transform.position.x, D.transform.position.y, -1), Quaternion.identity));//changer initguer par queen 
                Queens.Add(queen);
                game.GetComponent<Game>().P2unit.Add(queen);
                queen.GetComponent<Units>().hasMoved = true;
                game.GetComponent<Game>().royaljellyIA -= 20;
                game.GetComponent<Game>().CoordUpdate();
            }
        }
    }
    public void QueenBehavior()
    {
        foreach (var q in Queens)
        {
            q.GetComponent<Units>().hasMoved = true;
            float deplmax = game.GetComponent<Game>().DeplacementRange; //meme que les autres unit
            float distmin = game.GetComponent<Game>().DetectionRange;
            Vector2 go = transform.position;

            if (!IsPresent((game.GetComponent<Game>().SubCoordList), (go.x * (deplmax / distmin), go.y * (deplmax / distmin))))
                go = new Vector2(go.x * (deplmax / distmin), go.y * (deplmax / distmin));

            q.GetComponent<Units>().movetarget = go;
            q.GetComponent<Units>().ChangeTarget();
            game.GetComponent<Game>().CoordUpdate();
            q.GetComponent<Units>().hasMoved = true;
        }
    }
    public void ShamanBehavior()
    {
        bool casted=false;
        foreach(var unit2 in game.GetComponent<Game>().P2unit)
        {
            if (unit2 is Mage)
            {
                foreach (var unit1 in game.GetComponent<Game>().P1unit)
                {
                    //IASpell(new Vector3(unit1.transform.position.x, unit1.transform.position.y, -1));
                    float dist = Vector3.Distance(new Vector3(unit2.transform.position.x, unit2.transform.position.y, -1), new Vector3(unit1.transform.position.x, unit1.transform.position.y, -1));
                    if (dist <= game.GetComponent<Mage>().SpellCastRange)
                    {
                        Attack(unit1);
                        casted = true;
                    }
                    if (casted)
                        break;
                }
                
            }
            

        }
    }
    public void IASpell(Vector3 vect)
    {
        int attack = game.GetComponent<Mage>().AttackPower;
        int spellcd = game.GetComponent<Mage>().SpellCoolDown;
        int spellrange = game.GetComponent<Mage>().SpellCastRange;
        Vector3 center;
        bool casted=false;
        foreach(var M in game.GetComponent<Game>().P2unit)
        {
            if (M is Mage)
            {

                if(spellcd==0)
                {
                    foreach(var u in game.GetComponent<Game>().P1unit)
                    {
                        if(Vector3.Distance(new Vector3(M.transform.position.x, M.transform.position.y,-1),new Vector3(u.transform.position.x, u.transform.position.y, -1))<=spellrange)
                        {
                            game.GetComponent<Game>().AoEList.Add(Instantiate(game.GetComponent<MageZone>().Mobject, vect, Quaternion.identity));
                            casted = true;
                            M.GetComponent<Units>().hasMoved = true;
                        }
                        if (casted)
                            break;
                    }
                    
                    
                }

            }
        }
        
        
    }

    

    public void IAprep()
    {
        foreach (var unit2 in game.GetComponent<Game>().P2unit)
        {
            unit2.GetComponent<Units>().hasMoved = false;
        }
    }
    public void WhatToDo()
    {
        IAprep();
        System.Random random = new System.Random();
        int randomvalue = random.Next(1, 4);    //1,2

        //Doit trouver la base ennemie en vue la plus a droite plutot que le cpoint
        //Vector3 C = findCpoint();
        Vector3 D = FindClosestBase();
        // Occupation doit check les batiments au lieu des cpoints
        float O = buildingratio();
        CheckforAnts();
        
        ShamanBehavior();
        QueenBehavior();
        

        if (estimate() < game.GetComponent<Game>().P2unit.Count)// * (1 + (1 / (randomvalue) / 2)))
            winning = true;
        else winning = false;
        if (winning)
        {
            if (O < 0.5f)
            {
                // ! Implémenter une Queen pour les termites avec une ia qui va vers une position de base appropriée

                if (game.GetComponent<Game>().royaljellyIA >= 20)
                {
                    //SpawnQueen();
                    
                }
                if (randomvalue == 3)
                {
                    
                    if (game.GetComponent<Game>().foodIA >= 20)//valeur tempo
                    {
                        if(game.GetComponent<Game>().woodIA >= 5 && randomvalue==1)
                        {
                            SpawnRanger();
                        }
                        else if (game.GetComponent<Game>().woodIA >= 40 && randomvalue == 2)
                        {
                            //SpawnWorm();
                        }
                        else
                            SpawnUnit();
                    }
                }

            }
            else
            {


                // Créer unité sur batiments au lieu de sur une ligne qui monte
                while (game.GetComponent<Game>().foodIA >= 50) // à ameliorer avec l'implementations d autres unitées
                {
                    //SpawnShaman();
                }
                while (game.GetComponent<Game>().foodIA >= 20) // à ameliorer avec l'implementations d autres unitées
                {
                    if (game.GetComponent<Game>().woodIA >= 5 && randomvalue == 1)
                    {
                        SpawnRanger();
                    }
                    else
                        SpawnUnit();
                }

                if (turntimer < 0)
                {
                    D.x += 5;
                    gatherunits(D);
                    turntimer = 3;

                }
                else if (turntimer == 0)
                {
                    D.x -= 5;
                    gatherunits(D);
                }
                else
                {
                    gatherunits(D);
                    turntimer--;
                }

            }
        }
        else
        {
            // Créer unité sur batiments au lieu de sur une ligne qui monte

            while (game.GetComponent<Game>().foodIA >= 20) // à ameliorer avec l'implementations d autres unitées
            {
                SpawnUnit();
            }

        }
        


    }

    public void gatherunits(Vector3 vect)
    {
        bool pathed=false;
        foreach (var unit2 in game.GetComponent<Game>().P2unit)
        {
            

            if (Distvect(new Vector3(unit2.transform.position.x, unit2.transform.position.y), vect) < 80f)
            {
                float deplmax = game.GetComponent<Game>().DeplacementRange;
                float distmin = game.GetComponent<Game>().DetectionRange;
                float dist = Distvect(new Vector3(unit2.transform.position.x, unit2.transform.position.y), vect);
                Vector2 go = transform.position;
                foreach(var unit1 in game.GetComponent<Game>().P1unit)
                {
                    float distd = Vector3.Distance(new Vector3(unit2.transform.position.x, unit2.transform.position.y, -1), new Vector3(unit1.transform.position.x, unit1.transform.position.y, -1));
                    if (distd <= unit2.GetComponent<Units>().detectionRange)
                    {
                        if (distd <= deplmax)
                        {

                            if (!IstargetValid(new Vector2(unit1.transform.position.x + 1, unit1.transform.position.y))&& !IsPresent(targetlist,new Vector2(unit1.transform.position.x + 1, unit1.transform.position.y)))
                            {
                                go = new Vector2(unit1.transform.position.x + 1, unit1.transform.position.y);
                                targetlist.Add(go);
                            }
                                
                            else
                            {
                                go = new Vector2(unit1.transform.position.x - 1, unit1.transform.position.y);
                                targetlist.Add(go);
                            }
                                
                            Attack(unit1);
                            unit2.GetComponent<Units>().hasMoved = true;
                        }
                        else
                        {
                            //Vector2 v = arrondi(new Vector2(((unit1.transform.position.x - unit2.transform.position.x) / dist) * deplmax, ((unit1.transform.position.y - unit2.transform.position.y) / dist) * deplmax));
                            Vector2 v = targeting(unit2,unit1);
                            
                            if (!IstargetValid(v)&&!IsPresent(targetlist,v))
                            {
                                targetlist.Add(v);

                                go = v;
                                
                            }
                            else
                            {
                                pathfinding(unit2);
                                pathed = true;
                            }
                            

                        }
                        if (unit2.GetComponent<Units>().hasMoved == false)
                            break;

                    }
                    
                }
                if (unit2.GetComponent<Units>().hasMoved == false)
                {
                    foreach (var n in game.GetComponent<Game>().Dens)
                    {

                        if (dist <= unit2.GetComponent<Units>().detectionRange)
                        {
                            if (dist<= deplmax)
                            {

                                if (!IstargetValid(new Vector2(n.transform.position.x + 1, n.transform.position.y)) && !IsPresent(targetlist, new Vector2(n.transform.position.x + 1, n.transform.position.y)))
                                {
                                    go = new Vector2(n.transform.position.x + 1, n.transform.position.y);
                                    targetlist.Add(go);
                                }
                                    
                                else
                                {
                                    go = new Vector2(n.transform.position.x - 1, n.transform.position.y);
                                    targetlist.Add(go);
                                }
                                    
                                Attack(n);
                                unit2.GetComponent<Units>().hasMoved = true;

                            }
                            else
                            {
                                //Vector2 v = arrondi(new Vector2(((n.transform.position.x - unit2.transform.position.x) / dist) * deplmax, ((n.transform.position.y - unit2.transform.position.y) / dist) * deplmax));
                                
                                Vector2 v = targeting(unit2,n);
                                if (!IstargetValid(v)&&!IsPresent(targetlist,v))
                                {
                                    targetlist.Add(v);

                                    go = v;

                                }
                                else
                                {
                                    pathfinding(unit2);
                                    pathed = true;
                                }
                                
                            }
                            if (unit2.GetComponent<Units>().hasMoved == false)
                                break;


                        }
                    }
                    if (unit2.GetComponent<Units>().hasMoved == false)
                    {

                        //Vector2 v = arrondi(new Vector2(((vect.x - unit2.transform.position.x) / dist) * deplmax, ((vect.y - unit2.transform.position.y) / dist) * deplmax));
                        
                        
                        Vector2 v = targeting(unit2, vect);
                        if (!IsPresent(game.GetComponent<Game>().SubCoordList, (v.x, v.y))&& !IsPresent(targetlist,v))
                        {
                            go = v;
                            targetlist.Add(v);
                        }
                            
                        else
                        {
                            pathfinding(unit2);
                            pathed = true;
                        }
                        
                    }
                    
                }
                if (!pathed)
                {
                    unit2.GetComponent<Units>().movetarget = go;
                    unit2.GetComponent<Units>().ChangeTarget();
                    game.GetComponent<Game>().CoordUpdate();
                    unit2.GetComponent<Units>().hasMoved = true;
                }

                pathed = false;

            }
        }
    }
    public Vector3 FindClosestBase()
    {
        undercontrol = 0;
        List<GameObject> De = game.GetComponent<Game>().Dens;
        GameObject FoundD = null;
        float X = De[0].transform.position.x;
        foreach (var D in De)
        {
            if (D.transform.position.x >= X)
            {
                X = D.transform.position.x;
                FoundD = D;
            }
        }
        return new Vector3(FoundD.transform.position.x, FoundD.transform.position.y);
    }
    /*public Vector3 findCpoint()
    {
        undercontrol = 0;
        List<ControlPoints> Cp = game.GetComponent<Game>().Cpoints;
        ControlPoints CECP = null;
        float X = -10000f;
        foreach (ControlPoints C in Cp)
        {
            if (C.Team == 1)
            {
                if (C.x > X)
                {
                    X = C.x;
                    CECP = C;
                }
            }
            else
                undercontrol++;
        }
        return new Vector3(CECP.x, CECP.y);
    }*/

    public void CheckforAnts()
    {
        float pos1x;
        float pos2x;
        float pos1y;
        float pos2y;
        float dist;
        //float seenHP;
        bool Found;


        List<Units> seenthisturn = new List<Units>();

        foreach (var unit2 in game.GetComponent<Game>().P2unit) //Pour chaque termite
        {
            foreach (var unit1 in game.GetComponent<Game>().P1unit) //Pour chaque fourmi
            {

                pos1x = unit1.transform.position.x;
                pos2x = unit2.transform.position.x;
                pos1y = unit1.transform.position.y;
                pos2y = unit2.transform.position.y;
                dist = Distance(unit1, unit2);
                if (dist <= unit2.GetComponent<Units>().detectionRange) //si la fourmi est a porté de detection de la termite 
                {
                    /*
                    seenthisturn.Add(unit1.GetComponent<Units>());
                    seenHP = unit1.GetComponent<Units>().health;
                    var seenType = unit1.GetComponent<Units>().GetType();*/

                    Found = false;
                    foreach (Units U in seenthisturn)
                    {

                        if (U == unit1.GetComponent<Units>())
                            Found = true;

                        if (Found)
                            break;
                    }
                    if (!Found)
                    {
                        seenlist.Add(unit1.GetComponent<Units>());
                    }


                }
            }

        }
        foreach (Units U1 in seenlist)
        {
            for (int i = 0; i < seenthisturn.Count; i++)
            {
                if (U1.GetComponent<Units>().health == seenthisturn[i].GetComponent<Units>().health && ReferenceEquals(U1.GetComponent<Units>().GetType(), seenthisturn[i].GetComponent<Units>().GetType()))
                {
                    seenthisturn.RemoveAt(i);
                }

            }
        }
        foreach (Units U in seenthisturn)
        {
            seenlist.Add(U);
        }


    }
    public void pathfinding(GameObject G)
    {
        Vector2 go = G.transform.position;
        if(!IsPresent((game.GetComponent<Game>().SubCoordList), (G.transform.position.x, G.transform.position.y+3)))
        {
            go= new Vector2(G.transform.position.x, G.transform.position.y + 3);

        }
        else if (!IsPresent((game.GetComponent<Game>().SubCoordList), (G.transform.position.x-2, G.transform.position.y)))
        {
            go = new Vector2(G.transform.position.x-2, G.transform.position.y);
        }
        G.GetComponent<Units>().movetarget = go;
        G.GetComponent<Units>().ChangeTarget();
        G.GetComponent<Units>().hasMoved = true;
        game.GetComponent<Game>().CoordUpdate();

    }
    public void Move()
    {
        bool pathed = false;
        float pos1x;
        float pos2x;
        float pos1y;
        float pos2y;
        float dist;
        float closestx = -50f;
        float closesty = 5f;
        float deplmax = game.GetComponent<Game>().DeplacementRange;
        float distmin = game.GetComponent<Game>().DetectionRange;

        foreach (var unit2 in game.GetComponent<Game>().P2unit) //Pour chaque termite
        {
            if (unit2.GetComponent<Units>().hasMoved == false)
            {
                foreach (var unit1 in game.GetComponent<Game>().P1unit)
                {
                    pos1x = unit1.transform.position.x;
                    pos2x = unit2.transform.position.x;
                    pos1y = unit1.transform.position.y;
                    pos2y = unit2.transform.position.y;
                    dist = Vector3.Distance(new Vector3(unit1.transform.position.x,unit1.transform.position.y,-1), new Vector3(unit2.transform.position.x, unit2.transform.position.y, -1));
                    if (dist <= unit2.GetComponent<Units>().attackingRange && !(unit2 is Mage))
                    {
                        Attack(unit1);
                        playerturn.SetActive(true);
                        iaturn.SetActive(false);
                    }

                    if (dist < distmin)
                    {

                        distmin = dist;
                        closestx = pos1x;
                        closesty = pos1y;

                    }

                    Vector2 go = transform.position;
                    if (distmin <= deplmax)// && !IsPresent((game.GetComponent<Game>().CoordList), (closestx * (deplmax / distmin), closesty * (deplmax / distmin))))
                    {
                        targetlist.Add(new Vector2(closestx + 1, closesty));

                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (closestx + 1, closesty))&& !IsPresent(targetlist,new Vector2(closestx + 1, closesty)))
                            go = new Vector2(closestx + 1, closesty);
                        else
                            go = new Vector2(closestx - 1, closesty);
                    }

                    
                    else
                    {
                        //Vector2 v = arrondi(new Vector2(((closestx - unit2.transform.position.x) / dist) * deplmax, ((closesty - unit2.transform.position.y) / dist) * deplmax));
                        
                        Vector2 v = targeting(unit2,new Vector3(closestx,closesty,-1));
                        if (!IsPresent((game.GetComponent<Game>().SubCoordList), (v.x, v.y)))
                        {
                            targetlist.Add(v);

                            go = v;
                        }
                        else
                        {
                            pathfinding(unit2);
                            pathed = true;
                        }
                            
                    }

                    if (!pathed)
                    {
                        game.GetComponent<Game>().CoordUpdate();
                        unit2.GetComponent<Units>().movetarget = go;
                        unit2.GetComponent<Units>().ChangeTarget();
                        unit2.GetComponent<Units>().hasMoved = true;
                        game.GetComponent<Game>().CoordUpdate();
                        
                    }
                    pathed = false;

                }
            }
        }

        
    }

    void OnEnable()
    {
        //CheckforAnts();
        playerturn.SetActive(true);
        iaturn.SetActive(false);
    }
    void Start()
    {
        targetlist = new List<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        //WhatToDo();
        //Move();
        
    }
}