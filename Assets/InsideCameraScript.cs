using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideCameraScript : MonoBehaviour
{
    
    //location and offset for movement
    Quaternion rotationOffset;
    Vector3 initialPivotPoint;
    Vector3 rotatedStartPoint;
    Vector3 locationOffset;
    public bool followPlayer = false;
    public bool useOffset = true;

    //Referenced Objects
    public GameObject playerCameraPoint;
    public GameObject player;
    public GameObject pinhole;

    //zoom stuff
    float zoomedOutConstant = 1.6f;
    float zoomedInConstant = 0;
    bool zooming = false;
    bool zoomingIn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(followPlayer)
            MoveCamera();
        if(zooming)
            Zoom();
    }

    public void SetZoom(bool inward)
    {
        zooming = true;
        zoomingIn = inward;
    }

    void Zoom()
    {
        if (zoomingIn)
        {
            if (pinhole.transform.localPosition.z > zoomedInConstant)
            {
                pinhole.transform.localPosition += new Vector3(0, 0, -(0.03f* (pinhole.transform.localPosition.z*3+1)));
            }
            else
            {
                zooming = false;
            }
        }
        else//out
        {
            if (pinhole.transform.localPosition.z < zoomedOutConstant)
            {
                pinhole.transform.localPosition += new Vector3(0, 0, (0.03f * (pinhole.transform.localPosition.z * 3 + 1)));

            }
            else
            {
                zooming = false;
                gameObject.SetActive(false);
            }
        }

        
    }





    public void CalculateOffset(Vector3 newPlayerPosition, Quaternion exitRotation)
    {
        Vector3 rotatedPlayerToCameraPoint = RotatePointAroundPivot(playerCameraPoint.transform.position, player.transform.position, exitRotation) - player.transform.position;//Gets the distance the camera is supose to be away from the player at the correct rotation
        locationOffset = newPlayerPosition + rotatedPlayerToCameraPoint; //Finds the location the new camera needs to start at
        rotationOffset = exitRotation; // The change in rotation between the portals
        initialPivotPoint = newPlayerPosition; //The point to rotate the world around to calculate the camera synced movements
        rotatedStartPoint = RotatePointAroundPivot(playerCameraPoint.transform.position, initialPivotPoint, rotationOffset); //the first camera location used to subtract from the new camera movements to get a change(delta moved

    }

    void MoveCamera()
    {
        if (!useOffset)
        {

            transform.position = playerCameraPoint.transform.position;
        }
        else
        {

            transform.position = locationOffset+ (RotatePointAroundPivot(playerCameraPoint.transform.position, initialPivotPoint, rotationOffset)- rotatedStartPoint); //calculate delta moved from the main camera and apply it to this camera at the correct rotation
        }


    }




   public Vector3 RotatePointAroundPivot(Vector3 point,  Vector3 pivot,  Quaternion rot) //rotates a vector around a pivot
    {
   Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = rot  * dir; // rotate it
    point = dir + pivot; // calculate rotated point
   return point; // return it
 }
}
