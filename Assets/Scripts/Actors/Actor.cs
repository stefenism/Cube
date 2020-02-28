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

    void checkForParent()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            Transform hitTransform = hit.collider.transform;
            transform.SetParent(hitTransform.parent);
            Dimension currentDimension = hitTransform.gameObject.GetComponentInParent(typeof(Dimension)) as Dimension;
            if(currentDimension != null){
                checkSetDimension(currentDimension);
            }
            // why doesn't this work?
            //else if (hitTransform.TryGetComponent(out Dimension otherDimension))
            //{
            //	checkSetDimension(otherDimension);
            //}
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