using System.Collections;
using System.Collections.Generic;
using Multi;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerManager : PunTurnManager
{

    public Camera cam;
    public Text txt;
    public int team;

    private bool myturn;

    public GameObject p1base;
    public GameObject p2base;

    public GameObject first1units;
    public GameObject first2units;

    public GameObject Player1Units;
    public GameObject Player2Units;

    public GameObject Player1;
    public GameObject Player2;
    
    ///<summary> Parameter that activates the game after the base is planted </summary>
    ///<value>False until game start, true after</value>
    bool based;
    
    private PhotonView PV;


    #region private methods

    private Vector3 CoordNearCoord(Vector3 co)
    {
        float posx = Random.Range(co.x - 3, co.x + 3);
        float posy = Random.Range(co.y - 3, co.y + 3);
        return new Vector3(posx, posy, co.z);
    }

    private void FirstTurn()
    {
        if (team==1)
        {
            myturn = true;
        }
        else
        {
            myturn = false;
        }
    }

    #endregion
    #region MonoBehavior

    
    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
        
    }
    
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        team = PV.ViewID/1000;
        transform.position = Vector3.zero;
        cam.transform.position = Vector3.zero;


        DontDestroyOnLoad(gameObject);

        StartCoroutine(WaitBeforeStart());

    }

    IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(0.5f);
    }
    
    public override void OnPlayerLeftRoom(Player other)
    {
        SceneManager.LoadScene("Menu");
    }




    void Start()
    {
        based = false;
        txt.text = "Choose your base location please...(Can be placed only on blue field)";
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<Canvas>().gameObject);
        }
    }
    // Update is called once per frame
    

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !based && PV.IsMine)
        {
            Vector3 mouspos = Input.mousePosition;
            mouspos = cam.ScreenToWorldPoint(new Vector3(mouspos.x, mouspos.y, 1));
            int x = (int)mouspos.x;
            int y = (int)mouspos.y;
            mouspos = new Vector3(x + 0.5f, y + 0.5f, 1);
            PV.RPC("ChooseBaseLocation", RpcTarget.All, team, mouspos); //Appel de la fonction RPC
            based = true;
            txt.text = "Base placed";
            FirstTurn();
        }

        if (myturn)
        {
            if (team==1)
            {
                Player1.SetActive(true);
            }
            else
            {
                Player2.SetActive(true);
            }
        }
    }
    
    #endregion

    #region RPC
    
    [PunRPC]
    void ChooseBaseLocation(int Team, Vector3 mouspos)
    {
        if (Team==1)
        {
            p2base.SetActive(false);
            p1base.transform.position = mouspos;
            first2units.SetActive(false);
            foreach (Transform unit in first1units.transform)
            {
                unit.gameObject.transform.position = CoordNearCoord(p1base.transform.position);
            }
        }
        else
        {
            p1base.SetActive(false);
            p2base.transform.position = mouspos;
            first1units.SetActive(false);
            foreach (Transform unit in first2units.transform)
            {
                unit.gameObject.transform.position = CoordNearCoord(p2base.transform.position);
            }
        }
    }

    #endregion
}
