using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject player;
    public GameObject fireball;
    public GameObject launchPoint;
    public GameObject pivotPoint;
    Actor actorScript;
    public ParticleSystem flameParticle;
    public float chargeTime = 5;
    public float fireStartOffset = 1;

    // Start is called before the first frame update
    void Start()
    {
        actorScript = GetComponent<Actor>();
    }
    float charge;
    // Update is called once per frame
    GameObject currentFireball;
    void Update()
    {
        CastToPlayer();
        
        if (!(charge >= chargeTime))
        {
            charge += Time.deltaTime;
        }

        if(charge>=chargeTime-fireStartOffset && !flameParticle.isPlaying)
        {
            flameParticle.Play();
        }
    }


    Vector3 lookDirection;

    Quaternion currentRotation;
    Quaternion targetRotation;
    int detects=0;

    void CastToPlayer()
    {
        RaycastHit hit;
        LayerMask mask = 1 << gameObject.layer;


        lookDirection = Vector3.ProjectOnPlane(Quaternion.LookRotation(player.transform.position - transform.position, transform.up) * Vector3.forward,transform.up);


        if (Physics.Raycast(transform.position,lookDirection  , out hit, 10, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject == player|| hit.collider.transform.IsChildOf(player.transform))
            {
                detects +=1;
                currentRotation = pivotPoint.transform.rotation;
                targetRotation = Quaternion.LookRotation(lookDirection, transform.up);
                pivotPoint.transform.rotation = Quaternion.RotateTowards(currentRotation,targetRotation  , 2.5f );

                if (Quaternion.Angle(pivotPoint.transform.rotation, targetRotation)<5)
                {
                    if (charge >= chargeTime && detects >= 4)
                    {
                        ShootFireball();
                    }
                }


            }
            else
            {
                if (detects > 0)
                {
                    detects-=1;
                }
            }
        }
        //Debug.DrawRay(transform.position, lookDirection*10, Color.green,0.1f);
    }

    void ShootFireball()
    {
        currentFireball = Instantiate(fireball, launchPoint.transform.position, launchPoint.transform.rotation, actorScript.getDimension().transform);
        currentFireball.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(currentFireball.GetComponent<Collider>(), this.GetComponent<Collider>());
        flameParticle.Stop();
        charge = 0;
    }
}
