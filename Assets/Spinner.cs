using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spinning)
            HandleSpin();
    }
    bool spinning = false;
    float spinSpeed = 0;
    public float spinLength = 10;
    float maxSpinSpeed = 15;
    float spinReductionMultiplier = 1;

    public GameObject axis;

    public WireScript wire;



    public void Push()
    {
        spinning = true;
        spinSpeed = maxSpinSpeed;
        spinReductionMultiplier = maxSpinSpeed / spinLength;
        wire.Power(true, 1);
    }

    void HandleSpin()
    {
        spinSpeed -= Time.deltaTime * spinReductionMultiplier;
        axis.transform.localRotation = Quaternion.Euler(axis.transform.localRotation.eulerAngles.x , axis.transform.localRotation.eulerAngles.y + spinSpeed * 0.5f, axis.transform.localRotation.eulerAngles.z);
        wire.ChangePowerLevel(spinSpeed / maxSpinSpeed);

        if (spinSpeed <= 0)
        {
            spinning = false;
            wire.Power(false, 1);
        }
        
    }
    
}
