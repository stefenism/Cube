using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideController : MonoBehaviour
{
    CameraController cameraController;
    InsideCameraScript insideCamera;
    public Dimension thisDimension;
    public GameObject portal;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        insideCamera = cameraController.insideCamera.GetComponent<InsideCameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenPortal(GameObject player)
    {
        insideCamera.gameObject.SetActive(true);
        insideCamera.followPlayer = true;
        Vector3 npp = transform.position + (RotatePointAroundPivot(player.transform.position, portal.transform.position, Quaternion.Inverse(portal.transform.rotation * transform.rotation)) - portal.transform.position);
        insideCamera.CalculateOffset(npp,Quaternion.Inverse( portal.transform.rotation) * transform.rotation);
        insideCamera.useOffset = true;
        insideCamera.SetZoom(true);
    }

    public void TeleportPlayer(GameObject player)
    {
        insideCamera.gameObject.SetActive(true);
        
        player.transform.position = transform.position + (RotatePointAroundPivot( player.transform.position,portal.transform.position, Quaternion.Inverse(portal.transform.rotation * transform.rotation)) - portal.transform.position);
        player.transform.rotation = transform.rotation;
        player.layer = 31;
        player.GetComponent<PlayerActor>().setDimension(thisDimension);
        cameraController.CalculateOffset(portal.transform.position + RotatePointAroundPivot((player.transform.position), transform.position, portal.transform.rotation * transform.rotation) - transform.position, portal.transform.rotation * transform.rotation);
        //player.transform.SetParent(transform);
        cameraController.offsetOn = true;
        
        insideCamera.useOffset = false;
        insideCamera.followPlayer = true;
    }

    public void ClosePortal()
    {
        insideCamera.followPlayer = true;
        insideCamera.SetZoom(false);

    }

    public void ReturnPlayer(GameObject player)
    {
        player.transform.rotation = portal.transform.rotation;
        player.layer = portal.layer;
        player.transform.position = portal.transform.position+ RotatePointAroundPivot((player.transform.position ), transform.position, portal.transform.rotation * transform.rotation) - transform.position;//Gets the offset and rotation to teleport the player too.
        cameraController.offsetOn = false;
        insideCamera.useOffset = true;

    }
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.name == "Player")
        {
            ClosePortal();
            portal.GetComponent<InsidePortalScript>().darkness.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {

            ReturnPlayer(other.gameObject);

        }
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rot) //rotates a vector around a pivot
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = rot * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
