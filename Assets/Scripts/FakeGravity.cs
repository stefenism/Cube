using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGravity : MonoBehaviour
{
    Rigidbody r;
    public Vector3 currentGravity = Vector3.down;
    public float gravityMod = 0.5f;
    public bool enableGravity = true;


    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (enableGravity)
        {
            r.AddForce(currentGravity * gravityMod);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
            {

                transform.SetParent(hit.collider.transform);
                currentGravity = -transform.up;



            }
        }

    }
}
