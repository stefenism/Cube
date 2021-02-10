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
    public void Push()
    {
        spinning = true;
        spinSpeed = maxSpinSpeed;
        spinReductionMultiplier = maxSpinSpeed / spinLength;
    }

    void HandleSpin()
    {
        spinSpeed -= Time.deltaTime * spinReductionMultiplier;
        transform.RotateAroundLocal(transform.up, spinSpeed* Time.deltaTime);
        if (spinSpeed <= 0)
        {
            spinning = false;
        }
    }
    
}
