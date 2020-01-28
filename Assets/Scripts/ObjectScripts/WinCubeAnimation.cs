using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCubeAnimation : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(Random.Range(0,3.1f), Random.Range(0, 2.1f), Random.Range(0, 1.1f))); 
    }
}
