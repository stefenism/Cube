using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlockManipulation : MonoBehaviour
{
    public bool rotateXAxis = true;
    public bool rotateYAxis = false;

    public bool isSelected = false;
    private Quaternion startingRotation;
    private float addedXRotation;
    private float addedYRotation;
    public Vector3 rotationChange = Vector3.zero;


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

            //Get change in mouse Location. THIS NEEDS TO GET MOVED
            delta = Input.mousePosition - lastPos;


            //Rotate Block
            rotationChange += delta;
            //startingTransform.rotation.eulerAngles +rotationChange
            addedXRotation = 0;
            addedYRotation = 0;
            if (rotateXAxis)
                addedXRotation = -rotationChange.x;
            if (rotateYAxis)
                addedYRotation = rotationChange.y;
            transform.rotation = startingRotation * Quaternion.Euler(new Vector3 (addedYRotation, addedXRotation, 0));



        }
        else
        {
            var vec = transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(vec), 0.2f);//Do this but better
        }


        lastPos = Input.mousePosition;

        CheckBlockClick();
    }

    
    //super bad to do this here. Reallly gotta move this
    void CheckBlockClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask =  LayerMask.GetMask("BlockHandle");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,100,layerMask))
            {
                if (hit.transform == transform)
                    Selected();
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            Unselected();
        }
    }

    void Selected()
    {
        startingRotation = transform.rotation;
        isSelected = true;
    }

    void Unselected()
    {
        isSelected = false;
        rotationChange = Vector3.zero;
    }
}
