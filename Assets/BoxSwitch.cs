using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WireScript wire;

    void SetPower()
    {
        if (activeColliders.Count > 0)
        {
            wire.Power(true, 1);
        }
        else
        {
            wire.Power(false, 1);
        }
    }

    List<GameObject> activeColliders = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Actor>(out Actor a) )
        {
            activeColliders.Add(other.gameObject);
            
        }
        SetPower();
    }

    private void OnTriggerExit(Collider other)
    {
        if (activeColliders.Contains(other.gameObject))
        {
            activeColliders.Remove(other.gameObject);
        }
        SetPower();
    }
}
