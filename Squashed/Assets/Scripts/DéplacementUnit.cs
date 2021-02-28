using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DéplacementUnit : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera;
    public Grid grid;
    public bool clicked;
    public SpriteRenderer spriteRenderer;
    public Sprite movesprite;
    public Vector3 mousepos;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked && Input.GetMouseButtonDown(0))
        {
            //Récupère la position de la souris par rapport à la camera
            mousepos = camera.ScreenToWorldPoint(Input.mousePosition);
            //Permet de changer la position en position par rapport aux tiles
            mousepos = grid.WorldToCell(mousepos);
            //Change la position sur la case ou le joueur a cliqué
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(mousepos.x+0.5f,mousepos.y+0.5f,0),10);
        }
    }

    //Se lance quand on clique sur le collider de l'objet
    void OnMouseDown()
    {
        clicked = true;
        //Change le sprite actuel en movesprite
        //spriteRenderer.sprite = movesprite;
        
    }

    private void OnMouseExit()
    {
        
    }
}
