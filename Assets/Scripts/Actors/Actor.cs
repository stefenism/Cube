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

    void Update() {
        checkForParent();
    }

    void initialize(){
        //figure out which dimension you're in...on...?
        rb = GetComponent<Rigidbody>();
        actorCollider = GetComponent<BoxCollider>();
    }

    void checkForParent(){
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10)) {
            transform.SetParent(hit.collider.transform);
        }
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