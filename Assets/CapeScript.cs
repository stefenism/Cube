using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeScript : MonoBehaviour
{
    public Collider myBody;
    
    public Collider[] colliderToIgnore;

    public Animation capeAnimation;

    public GameObject player;

    public GameObject capePushWindPar;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider c in colliderToIgnore)
        {
            Physics.IgnoreCollision(myBody, c);
            capeAnimation = GetComponent<Animation>();
        }

    }


    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * 2.5f) / 10, 0);



    }

    public void CapePushAnimation(float delay)
    {
        capeAnimation.Play();

        Invoke("CapePushParticle", delay);
    }

    void CapePushParticle()
    {
        Instantiate(capePushWindPar, transform.position, transform.rotation * Quaternion.Euler(0, 90, 0));
    }
}
