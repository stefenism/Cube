using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
public class SpeechStingChanger : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDialog(string dialog)
    {
        GetComponent<VIDE_Assign>().AssignNew(dialog);
    }
}
