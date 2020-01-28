using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimension : MonoBehaviour {

    private enum TriggerAction{ENTER, EXIT}

    [SerializeField]
    private List<Actor> containedActors = new List<Actor>();

    private BoxCollider box;
    
    public Vector3 gravityDown = Vector3.zero;
    public Vector3 up = Vector3.zero;
    public Vector3 right = Vector3.zero;
    public Vector3 gravity = Vector3.zero;

    void Start() {
        initialize();
    }

    void initialize() {
        DimensionManager.dimensionDaddy.addDimension(this);
        box = GetComponent<BoxCollider>();

        findAllActorsInBox();
    }

    void Update() {
        findLocalDirections();
        findLocalGravity();
        // drawDebugDirections();
    }

    void FixedUpdate() {
        applyGravity();
    }

    void findAllActorsInBox() {

    }

    void findLocalDirections(){
        gravityDown = transform.up;
        up = transform.forward;
        right = transform.right;
    }

    void findLocalGravity(){
        gravity = gravityDown * Physics.gravity.y;
    }

    void applyGravity(){
        foreach(Actor a in containedActors){
            a.getRigidbody().AddForce(gravity);
        }
    }

    void checkIfActor(Collider collider, TriggerAction action) {
        if(collider.TryGetComponent(out Actor newActor)){
            if(action == TriggerAction.ENTER){
                tryAddActor(newActor);
            }
            else if(action == TriggerAction.EXIT){
                tryRemoveActor(newActor);
            }
        }
    }

    public void tryAddActor(Actor newActor){
        Debug.Log("trying to add actor: " + newActor.gameObject.name);
        print("and I am: " + this.gameObject.name + " of " + this.transform.parent.name);
        print("my contained actors: " + containedActors.Count);
        if(!containedActors.Contains(newActor)){
            containedActors.Add(newActor);
            newActor.setDimension(this);
        }
    }

    public void tryRemoveActor(Actor newActor){
        if(containedActors.Contains(newActor)){
            containedActors.Remove(newActor);
        }
    }

    void drawDebugDirections(){
        Debug.DrawRay(transform.position, up, Color.red, 100);
        Debug.DrawRay(transform.position, right, Color.green, 100);
    }

    void OnTriggerEnter(Collider collider){
        // Debug.Log("new trigger happened with: " + collider.gameObject.name);
        // checkIfActor(collider, TriggerAction.ENTER);
    }

    void OnTriggerExit(Collider collider)
    {
        // checkIfActor(collider, TriggerAction.EXIT);
    }
}