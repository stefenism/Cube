using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public SpeechBubbleScript speechBubbleScript;
    
    // Start is called before the first frame update
    public void Interact()
    {
        speechBubbleScript.NextDialog();
    }
}
