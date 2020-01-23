using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour {
    
    public static DimensionManager dimensionDaddy = null;

    [SerializeField]
    private List<Dimension> dimensionList = new List<Dimension>();

    void Awake() {
        if(dimensionDaddy == null){
            dimensionDaddy = this;
        }
        else if(dimensionDaddy != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void addDimension(Dimension newDimension){dimensionList.Add(newDimension);}
}