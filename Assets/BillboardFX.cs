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

        camTransform = PlayerManager.playerDaddy.playerCameraLocation.transform;
    }

    void Update()
    {
        transform.rotation = camTransform.rotation * originalRotation;
    }
}