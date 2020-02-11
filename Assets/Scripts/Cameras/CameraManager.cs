using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera;
    public Camera superCamera;
    public bool playerMode = true;
    float cameraState = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        HandlePlayerCameraPosition();
        if (Input.GetButtonDown("Shift View"))
            ChangeView();

    }

       


    public void ChangeView()//changes between camera views
    {
        if (playerMode)
        {
            playerCamera.clearFlags = CameraClearFlags.Nothing;
        }
        else
        {
            playerCamera.clearFlags = CameraClearFlags.Nothing;
        }
        playerMode = !playerMode;
    }

    void HandlePlayerCameraPosition()//eventually needs to be invoked 
    {
        if (playerMode)
        {
            if (cameraState <= 1)
            {
                playerCamera.rect = new Rect((1 - cameraState) * 0.03f, (1 - cameraState) * 0.76f, 0.2f + 0.8f * cameraState, 0.2f + 0.8f * cameraState);

                cameraState += 0.1f;
            }
            else
            {
                playerCamera.rect = new Rect(0, 0, 1, 1);

                //put cancel invoke here
            }
        }
        else
        {
            if (cameraState > 0)
            {
                playerCamera.rect = new Rect((1 - cameraState) * 0.03f, (1 - cameraState) * 0.76f, 0.2f + 0.8f * cameraState, 0.2f + 0.8f * cameraState);

                cameraState -= 0.1f;
            }
            else
            {
                //put cancel invoke here
            }

        }
    }
}
