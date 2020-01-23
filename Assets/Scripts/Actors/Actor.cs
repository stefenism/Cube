using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    private Rigidbody rb;
    private Collider actorCollider;

    [SerializeField]
    private Dimension dimension;

    void Awake() {
        initialize();
    }

    void initialize(){
        //figure out which dimension you're in...on...?
        rb = GetComponent<Rigidbody>();
        actorCollider = GetComponent<BoxCollider>();
    }

    public Rigidbody getRigidbody(){
        return rb;
    }

    public void setDimension(Dimension newDimension) {
        Debug.Log("setting dimension now boss: " + newDimension.gameObject.name);
        dimension = newDimension;
    }

    public Dimension getDimension(){return dimension;}
}