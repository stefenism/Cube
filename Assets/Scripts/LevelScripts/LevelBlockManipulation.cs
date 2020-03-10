using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlockManipulation : MonoBehaviour
{
    public bool rotateXAxis = true;
    public bool rotateYAxis = false;
    public bool rotateZAxis = false;

    public bool isSelected = false;
    private Quaternion startingRotation;
    private float addedXRotation;
    private float addedYRotation;
    private float addedZRotation;
    public Vector3 rotationChange = Vector3.zero;
    Vector3 hitLocation;
    Vector3 movedLocation;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Vector3 newV = (delta.y * mainCamera.transform.right);


            newV += delta.x * mainCamera.transform.up;
            movedLocation += new Vector3(newV.y, newV.x, 0);
            //transform.LookAt(movedLocation);
            //transform.position = movedLocation;

            //transform.position = movedLocation;

            if (rotateXAxis)
            {
                addedXRotation = -movedLocation.x;
            }
            if (rotateYAxis)
            {
                addedYRotation = movedLocation.y;
            }
            if (rotateZAxis)
            {
                addedZRotation = -movedLocation.x;
            }

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
            startingRotation = transform.rotation;
            hitLocation = hitPosition;
            movedLocation = hitPosition;
            mainCamera = Camera.main;
        }
        else
        {
            rotationChange = Vector3.zero;
        }
        
        isSelected = selected;
    }


}
