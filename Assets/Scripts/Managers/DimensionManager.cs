using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour {
    
    public static DimensionManager dimensionDaddy = null;

    public GameObject player;
    public Vector3 visableDimensionVector  = Vector3.zero;//The direction that is visable in the planer view

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

    void Update()
    {
        visableDimensionVector = player.transform.up;
    }

    public void addDimension(Dimension newDimension){dimensionList.Add(newDimension);}
}