using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    private static bool active;
    public GameObject eturn;

    public static bool Active
    {
        get => active;
        set => active = value;
    }

    private void Start()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            eturn.SetActive(true);
        }
        else
        {
            eturn.SetActive(false);
        }
    }
}
