using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimension : MonoBehaviour
{

    private enum TriggerAction { ENTER, EXIT }

    [SerializeField]
    private List<Actor> containedActors = new List<Actor>();

    private BoxCollider box;

    public Vector3 gravityDown = Vector3.down;
    public Vector3 up = Vector3.zero;
    public Vector3 right = Vector3.zero;
    public Vector3 gravity = Vector3.zero;
    float lastDiff = 100;
    MeshRenderer[] childMeshes;
    float lagUpdateChance = 0f;
    //pinecone physics variables
    public int pineconeUp;
    public int pineconeForward;
    int pineconeLastUp = -1;
    int pineconeLastForward = -1;
    public int currentLayer;

    CameraController cameraController;

    public enum BoundaryDirection
    {
        North,
        South,
        East,
        West
    }
    public GameObject boundaryColliderNorth;
    public GameObject boundaryColliderSouth;
    public GameObject boundaryColliderEast;
    public GameObject boundaryColliderWest;

    void Start()
    {
        initialize();
    }

    void initialize()
    {
        DimensionManager.dimensionDaddy.addDimension(this);
        box = GetComponent<BoxCollider>();

        findAllActorsInBox();

        UpdateChildRenders();
        SetDimentionDither();
        InvokeRepeating("SetDimentionDither", 0.05f, 0.05f);
        //pinecone physics

    }

    void Update()
    {
        findLocalDirections();
        findLocalGravity();

        // drawDebugDirections();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        HandelPineconeCollision();
    }

    void HandelPineconeCollision()
    {
        pineconeUp = Mathf.RoundToInt(Vector3.Angle(gravity, Vector3.up) / 45);
        pineconeForward = Mathf.RoundToInt(Vector3.Angle(gravity, Vector3.forward) / 45);

        if (Mathf.RoundToInt(Vector3.Angle(gravity, Vector3.right) / 45)<2)//flips if over 180 degree rotation
        {
            pineconeForward += 4;
        }

        if (pineconeUp != pineconeLastUp || pineconeForward != pineconeLastForward)
        {
            if (pineconeUp == 4)
            {
                SetDimensionLayer(31);
            }
            else if (pineconeUp == 0)
            {
                SetDimensionLayer(6);
            }
            else
            {
                SetDimensionLayer(31 - (pineconeForward + 1) - (pineconeUp-1)*8); 
            }

        }
        pineconeLastUp = pineconeUp;
        pineconeLastForward = pineconeForward;

    }

    void SetDimensionLayer(int layerNumber)
    {
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in allChildren )
        {
            t.gameObject.layer = layerNumber;
        }
        transform.gameObject.layer = layerNumber;
        currentLayer = layerNumber;
    }

    void findAllActorsInBox()
    {

    }

    void findLocalDirections()
    {
        gravityDown = transform.up;
        up = transform.forward;
        right = transform.right;
    }

    void findLocalGravity()
    {
        gravity = gravityDown * Physics.gravity.y;
    }

    void ApplyGravity()
    {
        foreach (Actor a in containedActors)
        {
            a.getRigidbody().AddForce(gravity);
        }
    }

    void checkIfActor(Collider collider, TriggerAction action)
    {
        if (collider.TryGetComponent(out Actor newActor))
        {
            if (action == TriggerAction.ENTER)
            {
                tryAddActor(newActor);
            }
            else if (action == TriggerAction.EXIT)
            {
                tryRemoveActor(newActor);
            }
        }
    }

    public void tryAddActor(Actor newActor)
    {
        Debug.Log("trying to add actor: " + newActor.gameObject.name);
        print("and I am: " + this.gameObject.name + " of " + this.transform.parent.name);
        print("my contained actors: " + containedActors.Count);
        if (!containedActors.Contains(newActor))
        {
            containedActors.Add(newActor);
            newActor.setDimension(this);
        }
    }

    public void tryRemoveActor(Actor newActor)
    {
        if (containedActors.Contains(newActor))
        {
            containedActors.Remove(newActor);
        }
    }

    void drawDebugDirections()
    {
        Debug.DrawRay(transform.position, up, Color.red, 100);
        Debug.DrawRay(transform.position, right, Color.green, 100);
    }

    void OnTriggerEnter(Collider collider)
    {
        // Debug.Log("new trigger happened with: " + collider.gameObject.name);
        // checkIfActor(collider, TriggerAction.ENTER);
    }

    void OnTriggerExit(Collider collider)
    {
        // checkIfActor(collider, TriggerAction.EXIT);
    }

    void SetDimentionDither()
    {
        float cameraState = Camera.main.GetComponent<CameraController>().cameraState;
        float gravityDiff = Vector3.Distance(gravityDown, DimensionManager.dimensionDaddy.visableDimensionVector);
        gravityDiff = Mathf.Clamp(gravityDiff, 0, cameraState);


        if (Mathf.Abs(lastDiff - gravityDiff) > 0.01 & !(gravityDiff > 1 & lastDiff > 1))//Dont run if it did not change much or it would still be completly invisable 
        {

            UpdateChildRenders();

            foreach (MeshRenderer mesh in childMeshes)
            {
                if (0.9f > gravityDiff & gravityDiff > 0.1f)//If within this range have a random chance to not change for effect
                {
                    if (Random.Range(0f, 1f) > lagUpdateChance)
                    {
                        mesh.material.SetFloat("_AlphaClipValue", gravityDiff);
                    }

                }
                else
                {
                    mesh.material.SetFloat("_AlphaClipValue", gravityDiff);
                }

            }
            lastDiff = gravityDiff;
        }

    }

    void UpdateChildRenders()
    {
        childMeshes = transform.GetComponentsInChildren<MeshRenderer>();
    }

    public void SetBoundaryDirectionActive(Dimension.BoundaryDirection direction, bool active)
    {
        switch (direction)
        {
            case BoundaryDirection.North:
                boundaryColliderNorth.SetActive(active);
                break;
            case BoundaryDirection.South:
                boundaryColliderSouth.SetActive(active);
                break;
            case BoundaryDirection.East:
                boundaryColliderEast.SetActive(active);
                break;
            case BoundaryDirection.West:
                boundaryColliderWest.SetActive(active);
                break;
        }
    }

    public void SetAllBoundaryDirectionsActive(bool active)
    {
        boundaryColliderNorth.SetActive(active);
        boundaryColliderSouth.SetActive(active);
        boundaryColliderEast.SetActive(active);
        boundaryColliderWest.SetActive(active);
    }

}