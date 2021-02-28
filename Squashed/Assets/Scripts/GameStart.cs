using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject myPrefab;
    public Camera camera;
    void Start()
    {
        Vector3 SpawnCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
        Instantiate(myPrefab,new Vector3(SpawnCenter.x,SpawnCenter.y,-1) ,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
