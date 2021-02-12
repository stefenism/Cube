using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateWires();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
            AnimateSwitch();
    }


    bool right = false;
    
    bool moving = false;

    public GameObject switchPivot;

    public WireScript leftWire;
    public WireScript rightWire;

    public void Push(Vector3 Direction)
    {
        if (Mathf.Abs(Vector3.Angle(Direction, -transform.right)) < 75)
        {
            if (right)
            {
                right = false;
                moving = true;
            }

        }
        else if(Mathf.Abs(Vector3.Angle(Direction, transform.right)) < 75)
        {
            if (!right)
            {
                right = true;
                moving = true;
            }
        }

    }
    public GameObject leftRot;
    public GameObject rightRot;
    
    void AnimateSwitch()
    {
        if (right)
        {
            switchPivot.transform.rotation=Quaternion.RotateTowards(switchPivot.transform.rotation, rightRot.transform.rotation, 4f);
            if (Quaternion.Angle(switchPivot.transform.rotation, rightRot.transform.rotation)<1)
            {
                moving = false;

                UpdateWires();

            }
        }
        else
        {
            switchPivot.transform.rotation=Quaternion.RotateTowards(switchPivot.transform.rotation, leftRot.transform.rotation, 4f);
            if (Quaternion.Angle(switchPivot.transform.rotation, leftRot.transform.rotation) < 1)
            {
                moving = false;
                UpdateWires();
            }
        }

        
    }

    void UpdateWires()
    {
        if (rightWire != null)///swap power
        {
            rightWire.Power(right, 1);
        }
        if (leftWire != null)
        {
            leftWire.Power(!right, 1);
        }
    }

}
