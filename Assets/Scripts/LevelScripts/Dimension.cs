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
    float lagUpdateChance = 0.1f;



    public Collider[] colliders;

    CameraController cameraController;



    void Start()
    {
        initialize();
        sideDirectionAngle = (180 - mainDirectionAngle * 2) / 2;
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

        //get all colliders attached
        colliders = GetComponents<Collider>();

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

    //pinecone physics variables
    public float pineconeUp;
    public float pineconeForward;
    int pineconeLastUp = -1;
    int pineconeLastForward = -1;
    public int currentLayer;
    int pineconePositionUp;
    int pineconePositionForward;
    float mainDirectionAngle= 0.5f;//The angle that allows for collision to start with faces in the main direction
    float sideDirectionAngle;

    void HandelPineconeCollision()
    {
        pineconeUp = Vector3.Angle(gravity, Vector3.up);//0 to 180
        pineconeForward = Vector3.Angle(gravity, Vector3.forward); //-180 to 180
        if (Mathf.RoundToInt(Vector3.Angle(gravity, Vector3.right) / 45) < 2)//flips if over 180 degree rotation
        {
            pineconeForward -= 180;
        }

            //Converts angle of the dimention on the up axis to a position to determin layer - Hard to read to avoid hard code as much as posible  
            if (pineconeUp <= mainDirectionAngle-mainDirectionAngle/2){
            pineconePositionUp = 4;
        }else if(pineconeUp <= sideDirectionAngle+ mainDirectionAngle-mainDirectionAngle/2){
            pineconePositionUp = 3;
        }else if(pineconeUp <= sideDirectionAngle+ mainDirectionAngle*2-mainDirectionAngle/2){
            pineconePositionUp = 2;
        }else if(pineconeUp <= sideDirectionAngle*2+ mainDirectionAngle*2-mainDirectionAngle/2){
            pineconePositionUp = 1;
        }else{
            pineconePositionUp = 0;
        }

        //Converts angle of the dimention on the forward axis to a position to determin layer
        if(pineconeForward <= -180+mainDirectionAngle- mainDirectionAngle/2)
        {
            pineconePositionForward = 0;
        }else if(pineconeForward <= -180+mainDirectionAngle+sideDirectionAngle - mainDirectionAngle / 2)
        {
            pineconePositionForward = 1;
        }else if(pineconeForward <= -180+mainDirectionAngle*2+sideDirectionAngle - mainDirectionAngle / 2)
        {
            pineconePositionForward = 2;
        }else if(pineconeForward <= -180+mainDirectionAngle*2+sideDirectionAngle*2 - mainDirectionAngle / 2)
        {
            pineconePositionForward = 3;
        }else if(pineconeForward <= -180+mainDirectionAngle*3+sideDirectionAngle*2 - mainDirectionAngle / 2)
        {
            pineconePositionForward = 4;
        }else if(pineconeForward <= -180+mainDirectionAngle*3+sideDirectionAngle*3 - mainDirectionAngle / 2)
        {
            pineconePositionForward = 5;
        }else if(pineconeForward <= -180+mainDirectionAngle*4+sideDirectionAngle*3 - mainDirectionAngle / 2)
        {
            pineconePositionForward = 6;
        }else if(pineconeForward <= -180+mainDirectionAngle*4+sideDirectionAngle*4 - mainDirectionAngle / 2){
            pineconePositionForward = 7;
        }
        else
        {
            pineconePositionForward = 0;
        }



        //use calculated positions to find layer
        if (pineconePositionUp != pineconeLastUp || pineconePositionForward != pineconeLastForward)//Checks if it changed 
        {
            if (pineconePositionUp == 4)
            {
                SetDimensionLayer(31);
            }
            else if (pineconePositionUp == 0)
            {
                SetDimensionLayer(6);
            }
            else
            {
                SetDimensionLayer(31 - (pineconePositionForward + 1) - (pineconePositionUp-1)*8); 
            }

        }
        pineconeLastUp = pineconePositionUp;
        pineconeLastForward = pineconePositionForward;

    }

    void SetDimensionLayer(int layerNumber)
    {
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in allChildren )
        {
            if(t != transform.GetChild(0).transform){
                t.gameObject.layer = layerNumber;
            }
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
            if(a.applyGravity)
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

    public void enableAllColliders(){
        foreach(Collider collider in colliders){
            collider.enabled = true;
        }
    }

    public void disableAllColliders(){
        foreach(Collider collider in colliders){
            collider.enabled = false;
        }
    }

    public void tryAddActor(Actor newActor)
    {
        //Debug.Log("trying to add actor: " + newActor.gameObject.name);
        //print("and I am: " + this.gameObject.name + " of " + this.transform.parent.name);
        //print("my contained actors: " + containedActors.Count);
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

    public int getCurrentLayer(){
        return currentLayer;
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

    /// <summary>
    /// Calculates how visable objects in this dimension sould be
    /// </summary>
    void SetDimentionDither()
    {
        float cameraState = Camera.main.GetComponent<CameraController>().cameraState;
        Vector3 visableDimensionVector = DimensionManager.dimensionDaddy.visableDimensionVector;
        float gravityDiff = Vector3.Distance(gravityDown, visableDimensionVector);
        float distanceDiff = Vector3.SignedAngle(transform.position, DimensionManager.dimensionDaddy.currentDimesionPosition, visableDimensionVector);

        Vector3 difference = DimensionManager.dimensionDaddy.currentDimesionPosition - transform.position;
        float distance = Mathf.Abs(difference.x * visableDimensionVector.x) + Mathf.Abs(difference.y * visableDimensionVector.y) + Mathf.Abs(difference.z * visableDimensionVector.z);
        gravityDiff += distance / 6;
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



}