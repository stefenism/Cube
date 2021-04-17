using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Vector3 startLocal;
    float startingRandomOffset;
    public MonoBehaviour actionScript;
    // Start is called before the first frame update
    void Start()
    {
        startLocal = gameObject.transform.localPosition;
        startingRandomOffset = Random.Range(0,2);
    }
    bool bob =true;
    // Update is called once per frame
    void Update()
    {
        if (collecting)
        {
            //startLocal += new Vector3(0, 0.04f, 0);
            transform.position =  Vector3.Lerp(transform.position, PlayerManager.playerDaddy.player.transform.position+ PlayerManager.playerDaddy.player.transform.up, 0.04f);
            if (transform.localScale.x > 0)
            {
                transform.localScale = transform.localScale -new Vector3(0.006f,0.006f,0.006f);
            }
            else
            {
                if(actionScript!=null)
                    actionScript.Invoke("Collected", 0);
                Destroy(gameObject);
                
            }
        }
            
            
        if(bob)
            gameObject.transform.localPosition = startLocal + new Vector3(0,Mathf.Sin( Time.time+ startingRandomOffset) /7, 0);
        
    }
    bool collecting=false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collecting = true;
            bob = false;
            //add collection code here
        }
    }
}
