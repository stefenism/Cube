using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    private Rigidbody rb;
    private Collider actorCollider;

    public bool isGrounded = false;
    public float halfHeight = 0.69f;

    [SerializeField]
    protected Dimension dimension;

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

    void checkForParent()
    {
        RaycastHit hit;
        LayerMask mask = 1 << gameObject.layer;

        // the player needs to be on the "Up" layer for this to work
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10,mask, QueryTriggerInteraction.Ignore))
        {
            Transform currentObject = hit.transform;
            bool notFound = true;
            while(currentObject!=null && notFound)
            {
                if (currentObject.TryGetComponent<Dimension>(out Dimension dim))
                {
                    transform.SetParent(dim.transform);
                    checkSetDimension(dim);
                    notFound = false;
                }
                else
                {
                    currentObject = currentObject.parent;
                }
            }
            if (hit.distance <= halfHeight + 0.01)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }

    }

    void checkSetDimension(Dimension newDimension){
        if(dimension != newDimension){
            if(dimension != null){
                dimension.tryRemoveActor(this);
            }
            setDimension(newDimension);
            newDimension.tryAddActor(this);
        }
    }

    public Rigidbody getRigidbody(){
        return rb;
    }

    public void setDimension(Dimension newDimension) {
        dimension = newDimension;
    }

    public Dimension getDimension(){return dimension;}
}