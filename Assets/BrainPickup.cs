using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class BrainPickup : MonoBehaviour
{
    public GameObject portal;
    public Collider key;
    public VIDE_Assign vendor;
    public ResetBoxes resetBoxes;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Collected()
    {
        portal.SetActive(true);
        key.enabled = true;
        vendor.AssignNew("RamenAdventureChat");
        resetBoxes.on = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
