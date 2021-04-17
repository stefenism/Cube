using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor {
    public Dimension firstDim;
    public Dimension secondDim;
    void Start() {
        // gameObject.layer = 6;
        //this.setDimension(firstDim);
        Invoke("DelayedStart", 0.1f);
    }

    void DelayedStart()
    {
        gameObject.layer = 0;
        //this.setDimension(secondDim);
        //gameObject.layer = secondDim.currentLayer;
        //transform.SetParent(parentOverride);

    }

    //private void FixedUpdate()
    //{
    //    if(dimension.gravity!=null)
    //   transform.rotation= Quaternion.LookRotation(dimension.gravity, Vector3.up);
    //}
}