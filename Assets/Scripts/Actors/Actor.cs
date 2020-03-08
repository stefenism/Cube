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
            Dimension possibleDimension = hit.collider.gameObject.GetComponent<Dimension>();
            if (possibleDimension != null)
            {
                transform.SetParent(possibleDimension.transform);
                checkSetDimension(possibleDimension);
            }
            else
            {
                Transform hitTransform = hit.collider.transform;
                transform.SetParent(hitTransform.parent);
                Dimension currentDimension = hitTransform.gameObject.GetComponentInParent(typeof(Dimension)) as Dimension;
                if (currentDimension != null)
                {
                    checkSetDimension(currentDimension);
                }
            }
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