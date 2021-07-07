using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Class_Hierarchy;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Building : Entity
{
    public string name;

    public int woodbyturn;
    public int royaljellybyturn;
    public int foodbyturn;
    
    public Camera camera;
    public GameObject actioninfo;
    public GameObject actionhatch;
    public GameObject actionupgrade;
    public GameObject actioncancel;
    
    private GameObject _canvas;

    private GameObject _btnobj;

    public bool isclicked;
    public bool buttonsshown;

    public List<GameObject> UnlockedUnits = new List<GameObject>();

    #region private functions


    /// <summary>
    /// List of unlockables Units
    /// </summary>
    public Queue<UnitType> UnlockablesUnits = new Queue<UnitType>();

    public bool HPUpgrades;
    
    //Other Ressources Upgrades
    public bool WoodUpgrade;
    public bool FoodUpgrade;
    public bool RoyalJellyUpgrade;
    
    /// <summary>
    /// Initialise the parameters of the upgrades
    /// </summary>
    private void InitUpgradesList()
    {
        WoodUpgrade = false;
        FoodUpgrade = false;
        RoyalJellyUpgrade = false;
        UnlockablesUnits.Enqueue(UnitType.Sargeant);
        UnlockablesUnits.Enqueue(UnitType.Beetle);
        UnlockablesUnits.Enqueue(UnitType.Sniper);
        UnlockablesUnits.Enqueue(UnitType.Engineer);
        HPUpgrades = false;
    }

    private void ShowButtons()
    {
        var vect = camera.WorldToScreenPoint(new Vector3(transform.position.x+1,transform.position.y));
        ActionsMenu.SetActive(true);
        foreach (Transform button in ActionsMenu.transform)
        {
            button.transform.position = vect;
            button.gameObject.SetActive(true);
            vect.y += 52;
        }
    }

    #endregion

    #region ButtonsFonctions

    public Text unitText;
    public Text WoodText;
    public Text RoyalJellyText;
    public Text FoodText;
    public Text UnitsnbrText;
    public Text NestsText;
    
    /// <summary>
    /// Get Infos
    /// </summary>
    public void Info()
    {
        InfoMenu.SetActive(true);
        
        WoodText.text = "Wood Income : "+ game.GetComponent<Game>().woodbyturn.ToString();
        RoyalJellyText.text = "Royal Jelly Income : "+game.GetComponent<Game>().rjbyturn.ToString();
        FoodText.text = "Food Income : "+game.GetComponent<Game>().foodbyturn.ToString();
        UnitsnbrText.text = "Number of Units : "+game.GetComponent<Game>().P1unit.Count.ToString();
        NestsText.text = "Number of Nests : " + game.GetComponent<Game>().Dens.Count.ToString();
        
        //List of Units creation
        float y = unitText.transform.position.y-50;
        Dictionary<string, int> u = new Dictionary<string,int>();
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            string name = unit.GetComponent<Units>().type;
            if (u.ContainsKey(name))
            {
                u[name]++;
            }
            else
            {
                u.Add(name,1);
            }
            unit.GetComponent<Units>().paused = true;

        }

        foreach (var unittype in u.Keys)
        {
            var txt = Instantiate(unitText, new Vector3(unitText.transform.position.x,y), Quaternion.identity);
            txt.text = "x"+ u[unittype] + " " + unittype;
            txt.transform.SetParent(InfoMenu.transform);
            y -= 50;
        }
        camera.GetComponent<CameraController>().paused = true;
    }

    public GameObject UpgradeMenu;

    public GameObject WoodUpButton;
    public GameObject FoodUpButton;
    public GameObject RoyalJellyUpButton;
    public GameObject NestUpButton;
    public GameObject UnlockButton;
    public GameObject UnlockText;
    /// <summary>
    /// Upgrade system
    /// </summary>
    public void Upgrade()
    {
        UpgradeMenu.SetActive(true);
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = true;
        }
        camera.GetComponent<CameraController>().paused = true;
        if (UnlockablesUnits.Count!=0)
        {
            UnitType q = UnlockablesUnits.Peek();
            switch (q)
            {
                case UnitType.Sargeant:
                    UnlockText.GetComponent<Text>().text = "Sargent x50 Wood";
                    break;
                case UnitType.Beetle :
                    UnlockText.GetComponent<Text>().text = "Beetle x150 Wood";
                    break;
                case UnitType.Sniper:
                    UnlockText.GetComponent<Text>().text = "Sniper x80 Wood";
                    break;
                case UnitType.Engineer:
                    UnlockText.GetComponent<Text>().text = "Engineer x100 Wood";
                    break;
            }
        }
        else
        {
            UnlockButton.SetActive(false);
        }
        
        if (WoodUpgrade)
        {
            WoodUpButton.SetActive(false);
        }
        if (FoodUpgrade)
        {
            FoodUpButton.SetActive(false);
        }
        if (RoyalJellyUpgrade)
        {
            RoyalJellyUpButton.SetActive(false);
        }
        if (UnlockedUnits.Count == 6)
        {
            UnlockButton.SetActive(false);
        }
        if (HPUpgrades)
        {
            NestUpButton.SetActive(false);
        }
        
    }

    
    /// <summary>
    /// Hatching new ants system
    /// </summary>
    public GameObject noneonnest;
    public GameObject someoneonnest;
    public GameObject HatchingButton;
    public void Hatching()
    {
        
        bool unitonpos = false;
        HatchMenu.SetActive(true);
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = true;
            if (transform.position == unit.transform.position)
            {
                unitonpos = true;
            }
        }
        camera.GetComponent<CameraController>().paused = true;
        if (unitonpos)
        {
            noneonnest.SetActive(false);
            someoneonnest.SetActive(true);
        }
        else
        {
            noneonnest.SetActive(true);
            someoneonnest.SetActive(false);
            Vector3 butpos = HatchingButton.transform.position;
            foreach (var unlockedUnit in UnlockedUnits) //Crée un bouton pour chaque type d'unité dans la liste unlocked units
            {
                
                var button = Instantiate(HatchingButton, butpos, Quaternion.identity);
                button.transform.SetParent(noneonnest.transform);
                button.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = unlockedUnit.GetComponent<SpriteRenderer>().sprite;
                string type = unlockedUnit.GetComponent<Units>().type;
                button.transform.GetChild(1).gameObject.GetComponent<Text>().text = type;

                switch (type) //Change la fonction du bouton en fonction de quel type d'unité c'est
                {
                    case "Rifleman":
                        button.GetComponent<Button>().onClick.AddListener(ClickedOnRifle);
                        button.transform.GetChild(2).gameObject.GetComponent<Text>().text = "x" + unlockedUnit.GetComponent<Units>().cost + " Food";
                        break;
                    case "Sargeant":
                        button.GetComponent<Button>().onClick.AddListener(ClickedOnSargeat);
                        button.transform.GetChild(2).gameObject.GetComponent<Text>().text = "x" + unlockedUnit.GetComponent<Units>().cost + " Food";
                        break;
                    case "Beetle":
                        button.GetComponent<Button>().onClick.AddListener(ClickedOnBeetle);
                        button.transform.GetChild(2).gameObject.GetComponent<Text>().text = "x" + unlockedUnit.GetComponent<Units>().cost + " Food";
                        break;
                    case "Sniper":
                        button.GetComponent<Button>().onClick.AddListener(ClickedOnSniper);
                        button.transform.GetChild(2).gameObject.GetComponent<Text>().text = "x" + unlockedUnit.GetComponent<Units>().cost + " Food";
                        break;
                    case "Engineer":
                        button.GetComponent<Button>().onClick.AddListener(ClickedOnEngineer);
                        button.transform.GetChild(2).gameObject.GetComponent<Text>().text = "x" + unlockedUnit.GetComponent<Units>().cost + " Food";
                        break;
                    case "Queen":
                        button.GetComponent<Button>().onClick.AddListener(ClickedOnQueen);
                        button.transform.GetChild(2).gameObject.GetComponent<Text>().text = "x20 Royal Jelly";
                        break;
                }
                butpos.x += 300f;
                if (butpos.x>1600)
                {
                    butpos.x = 305;
                    butpos.y = 240 ;
                }
            }
        }
        
    }

    #region ClickedOnUnit

    public void ClickedOnRifle()
    {
        if (game.GetComponent<Game>().food>=20)
        {
            game.GetComponent<Game>().food -= 20;
            Hatch(UnitType.Rifleman);
            ExitHatchMenu();
        }
        
    }
    
    public void ClickedOnSargeat()
    {
        if (game.GetComponent<Game>().food>=30)
        {
            game.GetComponent<Game>().food -= 30;
            Hatch(UnitType.Sargeant);
            ExitHatchMenu();
        }
        
    }
    
    public void ClickedOnBeetle()
    {
        if (game.GetComponent<Game>().food>=60)
        {
            game.GetComponent<Game>().food -= 60;
            Hatch(UnitType.Beetle);
            ExitHatchMenu();
        }
        
    }
    
    public void ClickedOnSniper()
    {
        if (game.GetComponent<Game>().food>=25)
        {
            game.GetComponent<Game>().food -= 25;
            Hatch(UnitType.Sniper);
            ExitHatchMenu();
        }
        
    }
    
    public void ClickedOnEngineer()
    {
        if (game.GetComponent<Game>().food>=40)
        {
            game.GetComponent<Game>().food -= 40;
            Hatch(UnitType.Engineer);
            ExitHatchMenu();
        }
        
    }
    
    public void ClickedOnQueen()
    {
        if (game.GetComponent<Game>().royaljelly >= 20)
        {
            game.GetComponent<Game>().royaljelly -= 20;
            Hatch(UnitType.Queen);
            ExitHatchMenu();
        }
    }

    #endregion
    
    private void Hatch(UnitType type)
    {
        //Instantiate a new unit depending on his type
        switch (type)
        {
            case UnitType.Rifleman:
                game.GetComponent<Game>().InstantiateUnit(game.GetComponent<Game>().rifleman,transform.position,team);
                break;
            case UnitType.Sargeant:
                game.GetComponent<Game>().InstantiateUnit(game.GetComponent<Game>().sargeant,transform.position,team);
                break;
            case UnitType.Beetle :
                game.GetComponent<Game>().InstantiateUnit(game.GetComponent<Game>().beetle,transform.position,team);
                break;
            case UnitType.Sniper:
                game.GetComponent<Game>().InstantiateUnit(game.GetComponent<Game>().sniper,transform.position,team);
                break;
            case UnitType.Engineer:
                game.GetComponent<Game>().InstantiateUnit(game.GetComponent<Game>().engineer,transform.position,team);
                break;
            case UnitType.Queen:
                game.GetComponent<Game>().InstantiateUnit(game.GetComponent<Game>().queen,transform.position,team);
                break;
        }
    }
    

    public GameObject InfoMenu;
    public void ExitInfoMenu()
    {
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = false;
        }
        camera.GetComponent<CameraController>().paused = false;
        InfoMenu.SetActive(false);
    }
    
    public void ExitUpgradesMenu()
    {
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = false;
        }
        camera.GetComponent<CameraController>().paused = false;
        UpgradeMenu.SetActive(false);
    }
    
    public GameObject HatchMenu;
    public void ExitHatchMenu()
    {
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            unit.GetComponent<Units>().paused = false;
        }
        camera.GetComponent<CameraController>().paused = false;
        HatchMenu.SetActive(false);
    }
    public void SetName(string value)
    {
        name = value;
    }
    
    #endregion

    #region MonoBehavior

    
    void Start()
    {
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        game = GameObject.Find("Game");
        foodbyturn = 20;
        woodbyturn = 20;
        royaljellybyturn = 0;
        UnlockedUnits.Add(game.GetComponent<Game>().rifleman);
        UnlockedUnits.Add(game.GetComponent<Game>().queen);
        name = "Your Base";
        _btnobj = GameObject.Find("ButtonsObjectForBase");
        _canvas = GameObject.Find("Canvas");
        InitUpgradesList(); 
        sizebar = hpbar.Find("Bar");
        Maxhealth = health;
        isclicked = false;
        buttonsshown = false;
    }

    void Update()
    {
        if (!paused)
        {
            SetHP(health);
            Vector2 camtovect = camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1));
            hpbar.position = camtovect;
            if (health<=0)
            {
                Death();
            }

            if (isclicked && !buttonsshown)
            {
                ShowButtons();
                buttonsshown = true;
            }
        }
        
    }

    void OnMouseDown()
    {
        if (team==1)
        {
            isclicked = true;
        }
    }
    #endregion
}
