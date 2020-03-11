using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatable : MonoBehaviour
{
    private Vector3 startLocalPosition;
    public Vector3 targetPosition;

    private void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    public void UpdateWithNormalizedPosition(float positionNormal)
    {
        // this is a little awkward, because we want 0 to be our target, while 1 is our start
        float newNormal = 1.0f - positionNormal;
        //Debug.Log("newNormal is " + newNormal + ", new pos: " + Vector3.Lerp(startLocalPosition, targetPosition, newNormal));
        transform.localPosition = Vector3.Lerp(startLocalPosition, targetPosition, newNormal);
    }
}
