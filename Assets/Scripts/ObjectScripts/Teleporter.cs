using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    PlayerControls player;
    public Dimension thisDimension;
    public Teleporter exitPortal;
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        player = PlayerManager.playerDaddy.player;
    }

    float animationBoost;
    float scrollTotal;
    bool pauseAnimChange = false;
    // Update is called once per frame
    void Update()
    {
        //Incresses pulse speeds up when the player gets close.
        if(player.gameObject.layer == gameObject.layer)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            
            animationBoost = Mathf.Clamp( (6-distance)/2 ,0,3f)+0.2f;

            scrollTotal -= animationBoost * Time.deltaTime;
            mesh.material.SetFloat("_ScrollTotal", scrollTotal);
            pauseAnimChange = false;
        }
        else
        {
            if(pauseAnimChange == false)
            {
                mesh.material.SetFloat("_ScrollTotal", 0.5f);
                pauseAnimChange = true;
            }
            
        }

        if (beamPos > beamTarget)
        {
            beamPos -= 0.025f;
            beamPos = Mathf.Clamp(beamPos, 0, 1);
            mesh.material.SetFloat("_BeamAmount", beamPos);

        }else if (beamPos < beamTarget)
        {
            beamPos += 0.025f;
            beamPos = Mathf.Clamp(beamPos, 0, 1);
            mesh.material.SetFloat("_BeamAmount", beamPos);
        }
        else
        {

        }
    }


    float beamPos = 0;
    float beamTarget = 0;




    public void OpenPortal()
    {

        beamTarget = 1;//Set Shader to drop beam
        player.DisableControls(true);//freeze player
        Invoke("BeamUp",0.6f); 


    }

    void BeamUp()
    {
        beamTarget = 0;
        player.gameObject.SetActive(false);
        Invoke("TeleportPlayer", 0.6f);
        Invoke("SetPlayerParent", 0.55f);
    }
    void SetPlayerParent()
    {
        player.transform.SetParent(exitPortal.thisDimension.transform);
    }
    public void TeleportPlayer()
    {

        exitPortal.lockPortal = true;//disable other portals teleport

        //Teleports and rotates player
        player.transform.position = exitPortal.transform.position;// + (RotatePointAroundPivot(player.transform.position, exitPortal.transform.position, Quaternion.Inverse(exitPortal.transform.rotation * transform.rotation)) - exitPortal.transform.position);
        player.transform.rotation = exitPortal.thisDimension.transform.rotation; //Quaternion.Euler(exitPortal.transform.rotation.eulerAngles + new Vector3(90, 0, 0)) ;
        player.gameObject.layer = exitPortal.gameObject.layer;
        player.GetComponent<PlayerActor>().setDimension(exitPortal.thisDimension);
        
        exitPortal.beamTarget = 1;//Drop Beam 
        Invoke("ClosePortal", 0.6f);//wait for animation




       
    }

    public void ClosePortal()
    {
        player.gameObject.SetActive(true);//show player
        exitPortal.beamTarget = 0;//rasise beam
        player.DisableControls(false);//Regain control

    }

    public bool lockPortal = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Player" && !lockPortal)
        {
            OpenPortal();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {

            lockPortal = false;

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
