using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    private Rigidbody rb;
    private Collider actorCollider;

    public bool isGrounded = false;
    public float halfHeight = 0.69f;

    public bool ableToBePushed = false;
    public bool forcePushToGrid = false;
    public bool applyGravity = true;
    [SerializeField]
    protected Dimension dimension;

    void Awake()
    {
        initialize();
    }
    bool startDelay = true;
    float timeForDelay;
    void Update()
    {
        if (startDelay)
        {
            timeForDelay += Time.deltaTime;
            if (timeForDelay > 0.01)
            {
                startDelay = false;
            }
        }
        else
        {
            checkForParent();
        }
        
    }

    void initialize()
    {
        //figure out which dimension you're in...on...?
        rb = GetComponent<Rigidbody>();
        actorCollider = GetComponent<BoxCollider>();
    }

    void checkForParent()
    {
        LevelBlockManipulation lb = null;
        if(dimension != null)
            dimension.transform.parent.TryGetComponent<LevelBlockManipulation>(out lb);
        
        if (lb == null || lb.isMoving == false)
        {

            RaycastHit hit;
            LayerMask mask = 1 << gameObject.layer;

            // the player needs to be on the "Up" layer for this to work
            if (Physics.Raycast(transform.position, -transform.up, out hit, 10, mask, QueryTriggerInteraction.Ignore))
            {
                Transform currentObject = hit.transform;
                bool notFound = true;
                Transform g = currentObject;
                while (currentObject != null && notFound)
                {
                    if (currentObject.tag == "WallHolder" || currentObject == gameObject)
                        break;
                    if (currentObject.TryGetComponent<Dimension>(out Dimension dim))
                    {
                        if (dim.GetComponentInParent<LevelBlockManipulation>().isMoving == false && (dimension == null || dimension.transform.parent != dim.transform.parent))
                        {
                            transform.SetParent(dim.transform);
                            checkSetDimension(dim);
                            notFound = false;
                            print(g.gameObject);
                        }
                        else
                        {
                            break;
                        }

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
    }

    void checkSetDimension(Dimension newDimension)
    {
        if (dimension != newDimension)
        {
            if (dimension != null)
            {
                dimension.tryRemoveActor(this);
            }
            setDimension(newDimension);
            newDimension.tryAddActor(this);
        }
    }

    public Rigidbody getRigidbody()
    {
        return rb;
    }

    public void setDimension(Dimension newDimension)
    {
        dimension = newDimension;
        print("Dim set: " + newDimension + " " + gameObject);
    }

    public Dimension getDimension() { return dimension; }


    RaycastHit[] boxHits;
    RaycastHit[] lineHits;
    Vector3 newFinalLocation;
    Vector3 modifiedDirection;
    bool beingPushed = false;
    /// <summary>
    /// Called when something attempts to push this actor. Returns if successful.
    /// </summary>
    /// <param name="directionVector">The direction in a vector3 of the push</param>
    public bool Push(Vector3 directionVector)
    {
        if (ableToBePushed)
        {
            if (forcePushToGrid && !beingPushed)

            {
                //4 degrees
                //if (Mathf.Abs(directionVector.normalized.x) >= 0.5f)
                //{
                //    modifiedDirection = new Vector3(Mathf.Round(directionVector.normalized.x), 0, 0);
                //}
                //else if (Mathf.Abs(directionVector.normalized.y) >= 0.5f)
                //{
                //    modifiedDirection = new Vector3(0, Mathf.Round(directionVector.normalized.y), 0);
                //}
                //else if (Mathf.Abs(directionVector.normalized.z) >= 0.5f)
                //{
                //    modifiedDirection = new Vector3(0, 0, Mathf.Round(directionVector.normalized.z));
                //}
                //else
                //{
                //    return false;
                //}
                modifiedDirection = new Vector3(Mathf.Round(directionVector.normalized.x), Mathf.Round(directionVector.normalized.y), Mathf.Round(directionVector.normalized.z));

                startingPosition = transform.position;
                newFinalLocation = transform.position + modifiedDirection;
                LayerMask mask = 1 << gameObject.layer;
                boxHits = Physics.BoxCastAll(newFinalLocation, new Vector3(0.45f, 0.45f, 0.45f), transform.forward, transform.rotation, 0,mask);
                lineHits = Physics.SphereCastAll(transform.position, 0.1f, Vector3.Normalize(newFinalLocation- transform.position ),1f, mask);
                bool lineHitValid = true;
                foreach(RaycastHit h in lineHits)//Checks if line hit is ok
                {
                    if(h.collider.gameObject != gameObject && !h.collider.isTrigger )
                    {
                        lineHitValid = false;
                    }
                }
                bool boxhitValid = true;
                foreach (RaycastHit h in boxHits)//Checks if line hit is ok
                {
                    if (h.collider.gameObject != gameObject && !h.collider.isTrigger)
                    {
                        boxhitValid = false;
                    }
                }

                if (boxhitValid && lineHitValid)
                {
                    beingPushed = true;
                    positionAlpha = 0;
                    InvokeRepeating("HandleGridPushing", 0.01f, 0.01f);
                    return true;
                }
                else
                {
                    foreach (RaycastHit h in boxHits)
                    {
                        print("box hit " + h.transform.gameObject.name);

                    }
                    foreach (RaycastHit h in lineHits)
                    {
                        print("line hit " + h.transform.gameObject.name);

                    }
                    return false;
                }
            }
            else
            {

            }
        }
        return false;
    }

    Vector3 startingPosition;
    float positionAlpha = 0;
    void HandleGridPushing()
    {
        positionAlpha += 0.1f;//Speed the actor moves at
        transform.position = Vector3.Lerp(startingPosition, newFinalLocation, Mathf.Clamp(positionAlpha, 0, 1));
        if (positionAlpha >= 1)
        {
            beingPushed = false;
            CancelInvoke("HandleGridPushing");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(newFinalLocation, new Vector3(0.96f, 0.96f, 0.96f));
    }
}