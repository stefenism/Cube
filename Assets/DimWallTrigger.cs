using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimWallTrigger : MonoBehaviour
{
    Collider parentCollider;
    List<DimWallTrigger> validWalls= new List<DimWallTrigger>();
    
    // Start is called before the first frame update
    void Start()
    {
        parentCollider = transform.parent.GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        DimWallTrigger trigger;
        if(other.TryGetComponent<DimWallTrigger>(out trigger))
        {
            if (!validWalls.Contains(trigger) && trigger.gameObject.layer == gameObject.layer)
            {
                validWalls.Add(trigger);
                SetWalls();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DimWallTrigger trigger;
        if (other.TryGetComponent<DimWallTrigger>(out trigger))
        {
            if (validWalls.Contains(trigger))
            {
                validWalls.Remove(trigger);
                SetWalls();
            }

        }
    }

    void SetWalls()
    {
        if (validWalls.Count > 0)
        {
            parentCollider.enabled = false;
        }
        else
        {
            parentCollider.enabled = true;
        }
    }
}
