using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlockManipulation : MonoBehaviour
{
    //settings
    public bool rotateXAxis = true;
    public bool rotateYAxis = false;
    public bool rotateZAxis = false;
    public bool slideXAxis = false;
    public bool slideYAxis = false;
    public bool slideZAxis = false;
    public float maxSlideDistance;
    public float startingSlidePosition;
    public bool locked;
    public int unlockCost = 1;
    public GameObject[] locks;


    //public for debugging
    public bool isSelected = false;

    //position calculation
    private Quaternion startingRotation;
    private Vector3 initialPosition;
    private Vector3 startingSlideLocation;
    private float addedXRotation;
    private float addedYRotation;
    private float addedZRotation;
    private float addedXLocation;
    private float addedYLocation;
    private float addedZLocation;
    public Vector3 delta = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;

    //movement clamps
    private float slideClampXMin;
    private float slideClampYMin;
    private float slideClampZMin;


    //current total rotation and movement from its starting location
    Vector3 movedRotation;
    Vector3 movedLocation;

    Camera mainCamera;

    //Variables for reseting position on overlaps
    private Vector3 lastWorldPositon;
    private Quaternion lastWorldRotation;
    private Vector3 lastMovedRotation;
    private Vector3 lastMovedLocation;
    

    LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Water");
        initialPosition = transform.position;
        slideClampXMin = initialPosition.x - startingSlidePosition;
        slideClampYMin = initialPosition.y - startingSlidePosition;
        slideClampZMin = initialPosition.z - startingSlidePosition;
    }



    // Update is called once per frame
    void Update()
    {
        if (isSelected)//Rotates and moves if you have this supercube selected. Code for selection is at time of writing in camera controls
        {
            //Saves current setup if the new position is invalid.
            lastWorldPositon = this.transform.position;
            lastWorldRotation = this.transform.rotation;
            lastMovedLocation = movedLocation;
            lastMovedRotation = movedRotation;

            //gets the change in the mouses location
            delta = Input.mousePosition - lastPos;
            

            //Resets the added location just rotation axis changes.
            addedXRotation = 0;
            addedYRotation = 0;
            addedZRotation = 0;
            addedXLocation = 0;
            addedYLocation = 0;
            addedZLocation = 0;

            //Converts mouse movement into movement rotation based on camera orientation
            Vector3 newV = (delta.y * mainCamera.transform.right);
            newV += delta.x * mainCamera.transform.up;
            
            //Swaps the X and Y of newV. Clamps magnitude to cap max rotation speed.
            movedRotation += Vector3.ClampMagnitude( new Vector3(newV.y, newV.x, 0),10);
            //converts mouse movements into new position location. Clamps magnitude to cap max movement speed.
            movedLocation += Vector3.ClampMagnitude(delta.y * mainCamera.transform.up/10,2);
            movedLocation += Vector3.ClampMagnitude(delta.x * mainCamera.transform.right/20,2);

            //Determins what part of the last calculated variables should actually be applyed to the supercube based on its settings
            if (rotateXAxis)
            {
                addedXRotation = -movedRotation.x;
            }
            if (rotateYAxis)
            {
                addedYRotation = movedRotation.y;
            }
            if (rotateZAxis)
            {
                addedZRotation = -movedRotation.x;
            }
            if (slideXAxis)
            {
                addedXLocation =movedLocation.x;
                
            }
            if (slideYAxis)
            {
                addedYLocation = movedLocation.y;
            }
            if (slideZAxis)
            {
                addedZLocation = movedLocation.z;
            }

            
            // Find Desired Supercube Location and applys it to the cube
            Vector3 moveLocation = startingSlideLocation + new Vector3(addedXLocation, addedYLocation, addedZLocation);
            transform.position = moveLocation;
            //checks if the current position is within its bounds and clamps if not
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, slideClampXMin, slideClampXMin + maxSlideDistance), Mathf.Clamp(transform.position.y, slideClampYMin, slideClampYMin + maxSlideDistance), Mathf.Clamp(transform.position.z, slideClampZMin, slideClampZMin + maxSlideDistance));
            //sets supercubes disired rotation
            transform.rotation = startingRotation * Quaternion.Euler(new Vector3(addedYRotation, addedXRotation, addedZRotation));


            //makes a boxs to find overlapping supercubes. 
            Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale*0.65f, transform.rotation,mask,QueryTriggerInteraction.Collide);
            //if supercubes are found(not including self) resets the position to last frames
            if (hitColliders.Length > 1)
            {
                transform.position = lastWorldPositon;
                transform.rotation = lastWorldRotation;
                movedRotation = lastMovedRotation;
                movedLocation = lastMovedLocation;
            }
        }
        else//If not selected, snaps the supercube to the closesed 90 degree angle
        {//TODO: Change to Invoke for performance
            var vec = transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(vec), 0.2f);//Do this but better
        }
        
        lastPos = Input.mousePosition;
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, transform.localScale*1.30f);
        
    //}


    /// <summary>
    /// If locked, attempts to unlock. Otherwise selects or unselects supercube for rotation;
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="hitPosition"></param>
    public void Selected(bool selected,Vector3 hitPosition = new Vector3())
    {
        if (selected)
        {
            if (locked)
            {
                if (DimensionManager.dimensionDaddy.player.GetComponent<PlayerManager>().PayKeys(unlockCost))
                {
                    locked = false;
                    foreach(GameObject l in locks)
                    {
                        l.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject l in locks)
                    {
                        l.transform.localScale = l.transform.localScale * Random.Range(0.95f,1.05f);
                    }
                }
            }
            else
            {
                startingRotation = transform.rotation;
                startingSlideLocation = transform.position;
                movedRotation = Vector3.zero;
                movedLocation = Vector3.zero;
                mainCamera = Camera.main;
            }
        }
        else
        {

           
        }
        
        isSelected = selected;
    }


}
