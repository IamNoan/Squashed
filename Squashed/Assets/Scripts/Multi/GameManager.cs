using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public Tilemap tmap;
    public List<GameObject> ObsList = new List<GameObject>();
    public List<(float,float)> CoordList = new List<(float,float)>();
    public List<GameObject> P1unit = new List<GameObject>();
    public List<GameObject> P2unit = new List<GameObject>();
    
    void Start()
    {
        var player = PhotonNetwork.Instantiate("PhotonPlayer",Vector2.zero,Quaternion.identity);
        DontDestroyOnLoad(gameObject);
        
    }
    
    
   
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
