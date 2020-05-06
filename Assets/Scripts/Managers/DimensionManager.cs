using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        // setBoundaries();
    }

    void Update(){
        
        if(player.transform.parent != null)
        {
            visableDimensionVector = player.transform.parent.transform.up.normalized;
            currentDimesionPosition = player.transform.parent.transform.position;
        }
        
        if(Input.GetKeyDown(KeyCode.Z)){
            setBoundaries();
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

    public void setBoundaries(){
        print("first...the dimensionList length:" +  dimensionList.Count);
        foreach(Dimension dimension in dimensionList){
            List<Dimension> samePlaneList = dimensionList.Where(loopDimension => loopDimension.getCurrentLayer() == dimension.getCurrentLayer()).ToList();
            print("same plane list length:" + samePlaneList.Count);
            List<Dimension> touchingList = samePlaneList.Where(loopDimension => isAdjacentDimension(loopDimension, dimension)).ToList();
            foreach(Dimension printDimension in touchingList){
                print("prinet demension is:" + printDimension);
                print("print dimension layer:" + printDimension.getCurrentLayer());
                print("touching list length:" + touchingList.Count);
            }
        }
    }

    public bool isAdjacentDimension(Dimension loopDimension, Dimension testDimension){
        float dimensionDistance = Vector3.Distance(loopDimension.transform.position, testDimension.transform.position);
        return dimensionDistance < 8 && loopDimension != testDimension;
    }
}