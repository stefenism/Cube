using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlockManipulation : MonoBehaviour
{
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



    public bool isSelected = false;
    private Quaternion startingRotation;
    private Vector3 initialPosition;
    private Vector3 startingSlideLocation;
    private float addedXRotation;
    private float addedYRotation;
    private float addedZRotation;
    private float addedXLocation;
    private float addedYLocation;
    private float addedZLocation;

    private float slideClampXMin;
    private float slideClampYMin;
    private float slideClampZMin;

    public Vector3 rotationChange = Vector3.zero;
    Vector3 hitLocation;
    Vector3 movedRotation;
    Vector3 movedLocation;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        slideClampXMin = initialPosition.x - startingSlidePosition;
        slideClampYMin = initialPosition.y - startingSlidePosition;
        slideClampZMin = initialPosition.z - startingSlidePosition;
    }

    public Vector3 delta = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            
            delta = Input.mousePosition - lastPos;//Get change in mouse Location. 
            
            rotationChange += delta;//Rotate Block

            addedXRotation = 0;
            addedYRotation = 0;
            addedZRotation = 0;
            addedXLocation = 0;
            addedYLocation = 0;
            addedZLocation = 0;
            Vector3 newV = (delta.y * mainCamera.transform.right);


            newV += delta.x * mainCamera.transform.up;
            movedRotation += new Vector3(newV.y, newV.x, 0);
            movedLocation += delta.y * mainCamera.transform.up/10;
            movedLocation += delta.x * mainCamera.transform.right/20;
            //transform.LookAt(movedLocation);
            //transform.position = movedLocation;

            //transform.position = movedLocation;

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

           
            
            Vector3 moveLocation = startingSlideLocation + new Vector3(addedXLocation, addedYLocation, addedZLocation);
            transform.position = moveLocation;
           transform.position = new Vector3(Mathf.Clamp(transform.position.x, slideClampXMin, slideClampXMin + maxSlideDistance), Mathf.Clamp(transform.position.y, slideClampYMin, slideClampYMin + maxSlideDistance), Mathf.Clamp(transform.position.z, slideClampZMin, slideClampZMin + maxSlideDistance));
            transform.rotation = startingRotation * Quaternion.Euler(new Vector3(addedYRotation, addedXRotation, addedZRotation));
        }
        else//Move into  place
        {//TODO: Change to Invoke for performance
            var vec = transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(vec), 0.2f);//Do this but better
        }

        lastPos = Input.mousePosition;
    }

    


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
                hitLocation = hitPosition;
                movedRotation = Vector3.zero;
                movedLocation = Vector3.zero;
                mainCamera = Camera.main;
            }
        }
        else
        {
            rotationChange = Vector3.zero;
           
        }
        
        isSelected = selected;
    }


}
