using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public GameObject MyUnits;
    private void OnEnable()
    {
        foreach (Transform myunit in MyUnits.transform)
        {
            myunit.transform.GetComponent<Units>().hasMoved = false;
        }
    }
}
