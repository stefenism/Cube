using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject player;
    public GameObject fireball;
    public GameObject launchPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    float clock;
    // Update is called once per frame
    void Update()
    {
        CastToPlayer();
        clock += Time.deltaTime;
        if (clock > 5)
        {
            clock = 0;
            //Instantiate(fireball, launchPoint.transform);
        }
    }


    Vector3 lookDirection;
    
    void CastToPlayer()
    {
        RaycastHit hit;
        LayerMask mask = 1 << gameObject.layer;


        lookDirection = Vector3.ProjectOnPlane(Quaternion.LookRotation(player.transform.position - transform.position, transform.up) * Vector3.forward,transform.up);


        if (Physics.Raycast(transform.position,lookDirection  , out hit, 10, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject == player|| hit.collider.transform.IsChildOf(player.transform))
            {
                transform.rotation = Quaternion.LookRotation(lookDirection,transform.up);

                //Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }
        //Debug.DrawRay(transform.position, lookDirection*10, Color.green,0.1f);
    }
}
