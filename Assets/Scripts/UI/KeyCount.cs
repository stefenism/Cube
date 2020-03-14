using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyCount : MonoBehaviour
{
    public TextMeshProUGUI text;
    public PlayerManager pm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = "Keycubes: " + pm.keyCountCurrent;

    }
}
