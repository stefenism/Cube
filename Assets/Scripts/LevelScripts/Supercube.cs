using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supercube : MonoBehaviour
{
    public Dimension dimensionTop;
    public Dimension dimensionBottom;
    public Dimension dimensionForward;
    public Dimension dimensionBack;
    public Dimension dimensionLeft;
    public Dimension dimensionRight;

    public bool openTop;
    public bool openBottom;
    public bool openForward;
    public bool openBack;
    public bool openLeft;
    public bool openRight;

    private void OnValidate()
    {

    }

  
}
