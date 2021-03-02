using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public float X;
    public float Y;
    public int health;
    public int team;
    public List<GameObject>[] allunits = new List<GameObject>[2];


}
