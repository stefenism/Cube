using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardGravityCosmetic : MonoBehaviour
{
    public GameObject referenceObject;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += -referenceObject.transform.up*0.1f;
    }
}
