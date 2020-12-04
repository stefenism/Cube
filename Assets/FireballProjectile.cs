using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward*1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform != transform)
        {
            Destroy(this.gameObject);
        }
            
    }
}
