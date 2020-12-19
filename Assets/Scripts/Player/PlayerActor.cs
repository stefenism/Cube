using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor {

    void Start() {
        gameObject.layer = 6;
    }

    //private void FixedUpdate()
    //{
    //    if(dimension.gravity!=null)
    //   transform.rotation= Quaternion.LookRotation(dimension.gravity, Vector3.up);
    //}
}