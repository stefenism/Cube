using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour {
    
    public static DimensionManager dimensionDaddy = null;

    public GameObject player;
    public Vector3 visableDimensionVector  = Vector3.zero;//The direction that is visable in the planer view
    public Vector3 currentDimesionPosition = Vector3.zero;

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
        player = FindObjectOfType<PlayerManager>().gameObject;
    }

    void Start()
    {
        SetPineconeLayers();
    }

    void Update(){
        
        if(player.transform.parent != null)
        {
            visableDimensionVector = player.transform.parent.transform.up.normalized;
            currentDimesionPosition = player.transform.parent.transform.position;
        }
            
    }


    void SetPineconeLayers()
    {

        for (int i = 4; i < 32; i++)
        {
            for (int f = i+1; f < 32; f++)
            {
                Physics.IgnoreLayerCollision(i, f,true);
            }
            Physics.IgnoreLayerCollision(i, 5, true);

        }

    }

    public void addDimension(Dimension newDimension){dimensionList.Add(newDimension);}
}