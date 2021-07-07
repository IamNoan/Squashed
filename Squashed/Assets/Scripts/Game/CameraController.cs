using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ? 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
public class CameraController : MonoBehaviour {
    public float offset;
    public float speed;
    //x - min y - max
    public Vector2 minMaxXPosition;
    public Vector2 minMaxYPosition;
    private float screenWidth;
    private float screenHeight;
    private Vector3 cameraMove;
    
    public float zoomSpeed;
    public float orthographicSizeMin;
    public float orthographicSizeMax;
    public float fovMin;
    public float fovMax;
    private Camera myCamera;

    public bool paused;
    // Use this for initialization
    void Start()
    {
        myCamera = GetComponent<Camera>();
        paused = false;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        cameraMove.x = transform.position.x;
        cameraMove.y = transform.position.y;
        cameraMove.z = transform.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            //Move camera
            if ((Input.mousePosition.x > screenWidth - offset) && transform.position.x < minMaxXPosition.y)
            {
                cameraMove.x += MoveSpeed();
            }
            if ((Input.mousePosition.x < offset) && transform.position.x > minMaxXPosition.x)
            {
                cameraMove.x -= MoveSpeed();
            }
            if ((Input.mousePosition.y > screenHeight - offset) && transform.position.y < minMaxYPosition.y)
            {
                cameraMove.y += MoveSpeed();
            }
            if ((Input.mousePosition.y < offset) && transform.position.y > minMaxYPosition.x)
            {
                cameraMove.y -= MoveSpeed();
            }
            transform.position = cameraMove;
            
            //Zoom Part
            if (myCamera.orthographic)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    myCamera.orthographicSize += zoomSpeed;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    myCamera.orthographicSize -= zoomSpeed;
                }
                myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    myCamera.fieldOfView += zoomSpeed;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    myCamera.fieldOfView -= zoomSpeed;
                }
                myCamera.fieldOfView = Mathf.Clamp(myCamera.fieldOfView, fovMin, fovMax);
            }
        }
    }
    float MoveSpeed()
    {
        return speed * Time.deltaTime;
    }
}
