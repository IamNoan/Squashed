
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Units : Entity
{
    public int movingRange;
    public int attackingRange;

    public string type;
    public int cost;

//Stats avant randomisation (au moment du spawn, les stats sont légèrement randomisées
    public int BaseHealth;
    public float BaseDefense; //Va de 0 à 0.9
    public int BaseAttack; //Attaque de base sans les éventuels modificateurs
    public float BaseArmorPen; //Va de 0 à 0.3
    public int BaseAccuracy; //On mettra une valeur de 1 à 100
    public int BaseCritRate; // De 0 à 100

    //Stats après la randomisation (Selon InitStats())
    //public int Health; Deja importée
    public int Accuracy; 
    public int AttackPower; //Renommée de Attack à AttackPower pour ne pas avoir de conflits avec la fonction attaque 
    public int detectionRange;
    public bool hasMoved;
    protected Camera camera;
    private Grid grid;
    public bool isMoving;
    public bool isAttacking;
    public bool mouseup;
    protected Vector3 mousepos;
    
    

    public GameObject bluecase;
    public List<GameObject> CaseList = new List<GameObject>();

    //Cible d'une attaque
    public GameObject target;
    
    //Imports Actions
    public GameObject actionmove;
    public GameObject actionend;
    public GameObject actioncancel;


    #region Methods

    public void InitStats(GameObject unit)
    {

        //Vie
        unit.GetComponent<Units>().health = Random.Range(health - (health/10), health + (health / 10));
        Debug.Log(this.health + " health");
        //Précision
        unit.GetComponent<Units>().Accuracy = Random.Range(BaseAccuracy - (BaseAccuracy/10), BaseAccuracy + (BaseAccuracy/10));
        Debug.Log(this.Accuracy + " current accuracy");
        Debug.Log(BaseAccuracy + "Base accuracy");
        //Attaque
        unit.GetComponent<Units>().AttackPower = Random.Range(BaseAttack - (BaseAttack/10), BaseAttack + (BaseAttack/10));
        Debug.Log(this.AttackPower + " Attack");
    }
    
    public void Actions() //Lors de l'action, active tous les boutons de l'unité
    {
        if (!hasMoved)
        {
            var vect = camera.WorldToScreenPoint(new Vector3(transform.position.x+1,transform.position.y));
            ActionsMenu.SetActive(true);
            foreach (Transform button in ActionsMenu.transform)
            {
                button.transform.position = vect;
                button.gameObject.SetActive(true);
                vect.y += 52;
            }
            game.GetComponent<Game>().WaitForAction = true;
        }
        
    }

    
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
    protected float Abs(float x)
    {
        if (x > 0) return x;
        else return (-x);
    }

    // Dans le cas d'un déplacement strictement Vertical:
    // Vérifie si il a un cube entre la position du joueur et la case ciblée
    // ReSharper disable Unity.PerformanceAnalysis
    public bool VertiCheck(float px, float py, float my)
    {
        bool Ofound = false;
        float a;
        float b;
        int longueur = game.GetComponent<Game>().CoordList.Count;

        for (int i = 0; i < longueur; i++)
        {
            (a, b) = game.GetComponent<Game>().CoordList[i];

            if (a == px)
            {
                if ((py < b && b <= my) || (my <= b && b < py))
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
        int longueur = game.GetComponent<Game>().CoordList.Count;

        for (int i = 0; i < longueur; i++)
        {
            (a, b) = game.GetComponent<Game>().CoordList[i];

            if (b == py)
            {
                if ((px < a && a <= mx) || (mx <= a && a < px))
                {
                    Ofound = true;
                    i += longueur;
                }
            }
        }
        return Ofound;
    }

    private bool showed;
    //fonction qui détermine quand et quelle case afficher.
    public void PrintCases()
    {
        
        //Quand l'unité a été cliquée et attend une direction.
        float x = transform.position.x;
        float y = transform.position.y;
        float a = movingRange;
        float follower = 0; //gere le nombre de cases par ligne
        bool descente = false; //gere la descente du losange
        for (float i = y+a; i > y-a-1; i-=1)
        {
            for (float j = x-follower; j < x+follower+1; j+=1)
            {
                if (!game.GetComponent<Game>().CoordList.Contains((j,i)) &&!game.GetComponent<Game>().SubCoordList.Contains((j,i)) )
                {
                    CaseList.Add(Instantiate(bluecase, new Vector2(j,i), Quaternion.identity));
                }
            }

            if (!descente) //On change le nbr de cases par ligne a la montée qui augmente puis a la descente du losange
            {
                if (follower<a)
                {
                    follower++;
                }
                else if (follower == a)
                {
                    follower--;
                    descente = true;
                }
            }
            else
            {
                follower--;
            }
        }
        
    }

    private bool firstreached;
    public Vector3 movetarget; //Place where to move
    private Vector3 first;
    private void Deplacement()
    {


        int MoveRange = movingRange;
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
        if (Abs(mousepos.x + 0.5f - transform.position.x) + Abs(mousepos.y + 0.5f - transform.position.y) <= MoveRange && !IsPresent(game.GetComponent<Game>().CoordList, (mousepos.x + 0.5f, mousepos.y + 0.5f)) && !IsPresent(game.GetComponent<Game>().SubCoordList, (mousepos.x + 0.5f, mousepos.y +0.5f)))
        {

            // Ce foreach détruit toutes les cases une fois le mouvement fait
            foreach (GameObject C in CaseList)
            {
                Destroy(C);
            }
            

            //Met le target et le first qui permettent le déplacement en temps réel à leurs positions
            movetarget = new Vector3(mousepos.x + 0.5f, mousepos.y + 0.5f, -1);
            ChangeTarget();
            hasMoved = true;
            isMoving = false;
        }
        
        game.GetComponent<Game>().CoordUpdate();
    }


    public void ChangeTarget() //Initialise les points de déplacement
    {
        if (movetarget.x-transform.position.x>movetarget.y-transform.position.y)
        {
            first = new Vector3(movetarget.x,transform.position.y,-1);
        }
        else
        {
            first = new Vector3(transform.position.x,movetarget.y,-1);
        }
        firstreached = false;
    }
    private void Move()
    {
        if (!firstreached)
        {
            transform.position = Vector3.MoveTowards(transform.position, first , 5 * Time.deltaTime);
            if (transform.position.x == first.x && transform.position.y == first.y)
            {
                firstreached = true;
            }
        }
        else
        {
            game.GetComponent<Game>().CoordUpdate();
            transform.position = Vector3.MoveTowards(transform.position, movetarget, 5 * Time.deltaTime);
        }
    }
    
    // PARTIE ATTAQUE
    
    //Fonction qui vérifie si une unité se trouve à la case sélectionnée
    public bool Findtarget(float X, float Y)
    {
        float a = 0;
        float b = 0;
        bool Found = false;

        foreach (var U in game.GetComponent<Game>().P1unit)
        {
            if (U.GetComponent<Units>().transform.position.x == X && U.GetComponent<Units>().transform.position.y == Y)
            {
                target = U;
                Found = true;
            }
        }

        foreach (var U in game.GetComponent<Game>().P2unit)
        {
            if (U.GetComponent<Units>().transform.position.x == X && U.GetComponent<Units>().transform.position.y == Y)
            {
                target = U;
                Found = true;
                Debug.Log("target found: "+ U.GetComponent<Units>().health);
            }
        }

        foreach (var U in game.GetComponent<Game>().Dens)
        {
            if (U.GetComponent<Building>().transform.position.x == X && U.GetComponent<Building>().transform.position.y == Y)
            {
                target = U;
                Found = true;
            }
        }

        foreach (var U in game.GetComponent<Game>().DensIA)
        {
            if (U.GetComponent<Building>().transform.position.x == X && U.GetComponent<Building>().transform.position.y == Y)
            {
                target = U;
                Found = true;
            }
        }
        /*
        foreach (var C in game.GetComponent<Game>().CoordList)
        {
            (a, b) = C;
        }
        */

        return Found;
    }

    public GameObject AttackPixel;
    private List<GameObject> atkpixels = new List<GameObject>();
    public void ShowPotentialTargets()
    {
        //Quand l'unité a été cliquée et attend une cible.
        float x = transform.position.x;
        float y = transform.position.y;
        float a = movingRange;
        float follower = 0; //gere le nombre de cases par ligne
        bool descente = false; //gere la descente du losange
        for (float i = y+a; i > y-a-1; i-=1)
        {
            for (float j = x-follower; j < x+follower+1; j+=1)
            {
                if (game.GetComponent<Game>().CoordList.Contains((j,i)))
                {
                    atkpixels.Add(Instantiate(AttackPixel,new Vector3(j,i),Quaternion.identity));
                    Debug.Log("target found");
                }
            }

            if (!descente) //On change le nbr de cases par ligne a la montée qui augmente puis a la descente du losange
            {
                if (follower<a)
                {
                    follower++;
                }
                else if (follower == a)
                {
                    follower--;
                    descente = true;
                }
            }
            else
            {
                follower--;
            }
        }

    }

    // Version avec les base stats
    public void Attack()
    {
        //On remplacera clicked par selected une fois qu'on l'aura implémenté
        if (isAttacking && !hasMoved)
        {

            //Vérifie la portée d'attaque et s'il y a une cible
            if (Abs(mousepos.x + 0.5f - transform.position.x) + Abs(mousepos.y + 0.5f - transform.position.y) <= attackingRange)
            {
                //Vérifie s'il y a une cible
                if (Findtarget(mousepos.x + 0.5f, mousepos.y + 0.5f))
                {
                    //Vérifie si l'attaque touche (on pourra rajouter une mécanique de distance qui change la précision en fonction de la distance plus tard)
                    int rand = Random.Range(1, 101);
                    int crit = Random.Range(1, 101);

                    float multipliyer = 1;

                    if (BaseAccuracy >= rand)
                    {
                        Debug.Log("Attack hit!");
                        //Si l'attaque est un coup critique
                        if (BaseCritRate >= crit)
                        {
                            multipliyer = 2;
                            Debug.Log("Critical hit");
                        }
                        
                        Debug.Log(mousepos);

                        Debug.Log(" The target is " + target.GetComponent<Entity>().name + target.transform.position);
                        int damage = (int)(BaseAttack * multipliyer * (1 - target.GetComponent<Units>().BaseDefense + BaseArmorPen));
                        target.GetComponent<Units>().health -= damage;

                        foreach (var pAtkpixel in atkpixels)
                        {
                            Destroy(pAtkpixel);
                        }
                        atkpixels.Clear();

                        isAttacking = false;
                        hasMoved = true;
                        // Ajouter script pour l'animation ici
                    }
                    else 
                    {
                        isAttacking = false;
                        hasMoved = true;
                    }
                }
                //S'il n'y a pas de cible
                else
                {
                    //There is nothing to attack here
                }
            }
            // Si l'endroit cliqué est trop loin
            else
            {
                //Afficher "Out of the unit's range ou un truc du genre
            }
        }
    }

    public Text damagetext;
    IEnumerator DamagePrint(Vector3 targetpos,int damage)
    {
        damagetext.gameObject.SetActive(true);
        damagetext.transform.position = camera.WorldToScreenPoint(targetpos);
        damagetext.text = "-" + damage;

        yield return new WaitForSeconds(1.5f);
        damagetext.gameObject.SetActive(false);
        yield return null;

    }

    //
    //
    //
    // Attaque officielle (fonctionne normalement avec les stats randomisées)

    /*
    public void Attack()
    {
        //On remplacera clicked par selected une fois qu'on l'aura implémenté
        if (isAttacking && !hasMoved)
        {
            
            //Vérifie la portée d'attaque et s'il y a une cible
            if (Abs(mousepos.x + 0.5f - transform.position.x) + Abs(mousepos.y + 0.5f - transform.position.y) <= attackingRange)
            {
                //Vérifie s'il y a une cible
                if (Findtarget(mousepos.x + 0.5f, mousepos.y + 0.5f))
                {
                    Debug.Log(target.GetComponent<Units>().health + "health remaining before attack");
                    //Vérifie si l'attaque touche (on pourra rajouter une mécanique de distance qui change la précision en fonction de la distance plus tard)
                    int rand = Random.Range(1, 101);
                    int crit = Random.Range(1, 101);

                    float multipliyer = 1;
                    
                    
                    Debug.Log(rand + " random hit value");
                    Debug.Log(Accuracy + "Accuracy at attack");

                    if (Accuracy >= rand)
                    {
                        Debug.Log("Attack hit!");
                        //Si l'attaque est un coup critique
                        if (CritRate >= crit)
                        {
                            multipliyer = 1.5f;
                            Debug.Log("Critical hit");
                        }

                        target.GetComponent<Units>().health -= (int)(AttackPower * multipliyer * (1 - target.GetComponent<Units>().Defense + ArmorPenetration));
                        Debug.Log(target.GetComponent<Units>().health + "health remaining after attack");

                        isAttacking = false;
                        hasMoved = true;
                        // Ajouter script pour l'animation ici
                    }
                    else Debug.Log("Attack missed!");
                }
                //S'il n'y a pas de cible
                else
                {
                    //There is nothing to attack here
                }
            }
            // Si l'endroit cliqué est trop loin
            else
            {
                //Afficher "Out of the unit's range ou un truc du genre
            }
        }
    }

    */

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        grid = GameObject.Find("Map").GetComponent<Grid>();
        sizebar = hpbar.Find("Bar");
        Maxhealth = health;
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        game = GameObject.Find("Game");
        movetarget = transform.position;
        first = transform.position;
        firstreached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {

            Move();
            if (isMoving) //Tant que le personnage doit se déplacer
            {
                if (!showed)
                {
                    PrintCases();
                    showed = true;
                }
            
                if (Input.GetMouseButtonDown(0) && isMoving && !hasMoved)
                {
                    Deplacement(); //Modifie la movetarget a la position ou la fourmi doit aller
                }
            }

            if (isAttacking)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mousepos = camera.ScreenToWorldPoint(Input.mousePosition);
                    mousepos = grid.WorldToCell(mousepos);
                    Attack();
                }
            }

            if (hasMoved)
            {
                GetComponent<SpriteRenderer>().color = Color.gray;
                showed = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }

            SetHP(health);
            hpbar.position = camera.WorldToScreenPoint(new Vector3(transform.position.x,transform.position.y+1));
            
            if (health<=0)
            {
                Death();
            }
            
        }
    }

    private void OnMouseUp()
    {
        if (isMoving)
        {
            mouseup = true;
        }
    }

    #endregion
}

