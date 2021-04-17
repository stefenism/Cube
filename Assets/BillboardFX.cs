using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    Transform camTransform;

    Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;

        camTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = camTransform.rotation;// * originalRotation;
    }
}