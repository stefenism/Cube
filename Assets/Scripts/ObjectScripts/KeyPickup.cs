using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{

    bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickedUp)
        {
            transform.localPosition += new Vector3(0, 0.02f, 0);
            transform.localScale -= new Vector3( 0.005f, 0.002f, 0.0003f);
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0, 10), Mathf.Clamp(transform.localScale.y, 0, 10), Mathf.Clamp(transform.localScale.z, 0, 10));
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(pickedUp == false)
        {
            if (other.TryGetComponent(out PlayerManager pm))
            {
                pm.GainKey();
                pickedUp = true;
                Invoke("DestroySelf", 5f);
            }
        }
        

       

    }
    void DestroySelf()
    {
        Object.Destroy(this.gameObject);
    }
}
