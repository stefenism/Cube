using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundColorFade : MonoBehaviour
{


    public Camera Camera { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Camera = GetComponent<Camera>();
    }


    float amplitudeX = 7.0f;
    float amplitudeY = 3.0f;
    float omegaX = 0.25f;
    float omegaY = 0.5f;
    float index;
    // Update is called once per frame
    void Update()
    {
        index += Time.deltaTime/10;
        float x = amplitudeX * Mathf.Cos(omegaX * index)*0.01f;
        float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * index)*0.02f);

        Camera.backgroundColor = new Color(x, y, Mathf.Abs(x - y));
    }
}
