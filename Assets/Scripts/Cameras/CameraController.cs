using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool playerMode = true;
    public GameObject player;
    public Transform playerCameraLocation;
    private Rigidbody rigidbody;
    Transform cinCameraPoint;

    public float cameraState=1;


    private void Awake()
    {
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        //player = pm.gameObject;
        playerCameraLocation = pm.playerCameraLocation.transform;

    }

    void Start()
    {

        //cinCameraPoint = transform;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
        MoveCamera();


    }

    // Update is called once per frame
    void Update()
    {


       
        //if(!playerMode)
            CheckBlockClick();
    }

    LevelBlockManipulation selectedManipulationScript;

    /// <summary>
    /// Uses raycasts to select movable blocks
    /// </summary>
    void CheckBlockClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = LayerMask.GetMask("Water");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, layerMask, QueryTriggerInteraction.Collide))
            {
                if (selectedManipulationScript != null) //make sure nothing is selected
                {
                    selectedManipulationScript.Selected(false);
                    selectedManipulationScript = null;
                }

                if (hit.transform.TryGetComponent(out selectedManipulationScript))//select a movable block if it can
                {
                    selectedManipulationScript.Selected(true,hit.point);
                }
                
            }

        }
        if (Input.GetMouseButtonUp(0))//release movable block
        {
            if(selectedManipulationScript != null)
            {
                selectedManipulationScript.Selected(false);
                selectedManipulationScript = null;


                
            }
        }
    }




    

    public void SetCameraView(Transform cameraPoint)//changes between camera views
    {
        InvokeRepeating("HandleCameraPositionEase", 0.01f, 0.01f);
        cinCameraPoint = cameraPoint;
        playerMode = false;
    }

    public void SetPlayerView()
    {
        playerMode = true;
        InvokeRepeating("HandleCameraPositionEase", 0.01f, 0.01f);
    }


    void LateUpdate()
    {
        MoveCamera();
    }



    /// <summary>
    /// Calculates where the camera should be and moves it there
    /// </summary>
    void MoveCamera()
    {
        if (player.transform)
        {


            transform.rotation = Quaternion.Lerp(cinCameraPoint.rotation, playerCameraLocation.rotation, cameraState);
            transform.position = Vector3.Lerp(cinCameraPoint.position, playerCameraLocation.position, cameraState);



        }


    }




    void HandleCameraPositionEase()
    {
        if (playerMode)
        {
            if (cameraState <= 1)
            {
                cameraState += 0.01f;
            }
            else
            {


                CancelInvoke("HandleCameraPositionEase");
            }
        }
        else
        {
            if (cameraState > 0)
            {
                

                cameraState -= 0.01f;
            }
            else
            {

                CancelInvoke("HandleCameraPositionEase");
            }

        }
    }


    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        else if (angle > 360F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rot) //rotates a vector around a pivot
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = rot * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}