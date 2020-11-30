using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeScript : MonoBehaviour
{
    public Collider myBody;
    
    public Collider[] colliderToIgnore;

    public Animation capeAnimation;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider c in colliderToIgnore)
        {
            Physics.IgnoreCollision(myBody, c);
            capeAnimation = GetComponent<Animation>();
        }

    }

    float ti=0;
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, Mathf.Sin(Time.time*2.5f)/10, 0);
      
        
        //ti += 1;
        //if (ti>100)
        //{
        //    capeAnimation.Play();
        //    ti = 0;
        //}
    }
}
