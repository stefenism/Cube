using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DimensionManager : MonoBehaviour {
    
    public static DimensionManager dimensionDaddy = null;

    public LayerMask boundaryCheckLayer;

    public GameObject player;
    public Vector3 visableDimensionVector  = Vector3.zero;//The direction that is visable in the planer view
    public Vector3 currentDimesionPosition = Vector3.zero;
    private float adjacentDimensionCheckDistance = 8f;

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
        player = FindObjectOfType<PlayerControls>().gameObject;
    }

    void Start()
    {
        SetPineconeLayers();
        setNewBoundaries();
    }

    void Update(){
        
        if(player.transform.parent != null)
        {
            visableDimensionVector = player.transform.parent.transform.up.normalized;
            currentDimesionPosition = player.transform.parent.transform.position;
        }
        
        // if(Input.GetKeyDown(KeyCode.Z)){
        //     setBoundaries();
        // }
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

    public void setNewBoundaries(){
        //turnOnAllBoundaries();
        //setBoundaries();
    }

    public void turnOnAllBoundaries(){
        foreach(Dimension dimension in dimensionList){
            dimension.enableAllColliders();
        }
    }

    //Stuffs code, not going to delete but wanted to try something different. - Love, Zack

    //public void setBoundaries(){
    //    // print("first...the dimensionList length:" +  dimensionList.Count);
    //    foreach(Dimension dimension in dimensionList){
    //        List<Dimension> samePlaneList = dimensionList.Where(loopDimension => loopDimension.getCurrentLayer() == dimension.getCurrentLayer() && loopDimension != dimension).ToList();
    //        List<Dimension> touchingList = samePlaneList.Where(samePlaneDimension => isAdjacentDimension(samePlaneDimension, dimension)).ToList();
    //        if(touchingList.Count > 0){
    //            // print("***********************before looping touchinglist");
    //            // print("dimension that's being tested is:" + dimension.gameObject.name);
    //            // print("dimension position:" + dimension.transform.GetChild(dimension.transform.childCount -1).transform.position);
    //            // print("dimension daddy is:" + dimension.transform.parent.gameObject.name);
    //            foreach(Dimension printDimension in touchingList){
    //                if(printDimension.transform.parent.gameObject.name == "testes"){
    //                    print("touching list length: " + touchingList.Count);
    //                    foreach(Dimension test in touchingList){
    //                        print("test.gameobjectparent.name is: " + test.transform.parent.gameObject.name);
    //                    }
    //                }
    //                // print("prinet demension is:" + printDimension.gameObject.name);
    //                // print("print dimension parent:" + printDimension.transform.parent.gameObject.name);
    //                // print("print dimension layer:" + printDimension.getCurrentLayer());
    //                // print("touching list length:" + touchingList.Count);
    //                // print("transform.position of adjacent dimension:" + printDimension.transform.GetChild(printDimension.transform.childCount -1).transform.position);
    //                // print("distance between this and testing dimension: " + Vector3.Distance(dimension.transform.rotation * dimension.transform.position, printDimension.transform.rotation * printDimension.transform.position));
    //                // print("-----------------------");
    //                // print("starting disabling");
    //                disableCollidersBetween(dimension, printDimension);
    //            }
    //            // print("*********************after looping touchinglist");
    //        }
    //    }
    //}

    //private bool isAdjacentDimension(Dimension samePlaneDimension, Dimension dimension){

    //    Vector3 samePlaneAnchorPosition = samePlaneDimension.transform.GetChild(0).transform.position;
    //    Vector3 dimensionAnchoPosition = dimension.transform.GetChild(0).transform.position;

    //    RaycastHit hit;

    //    //boundaryCheckLayer =  1 << dimension.gameObject.layer;

    //    float dimensionDistance = Vector3.Distance(samePlaneAnchorPosition, dimensionAnchoPosition);
    //    bool anythingInBetween = Physics.Linecast(dimensionAnchoPosition, samePlaneAnchorPosition, out hit, boundaryCheckLayer);
    //    Collider[] linecastOriginColliders = Physics.OverlapSphere(dimensionAnchoPosition, .2f, boundaryCheckLayer);

    //    bool insideCollider = linecastOriginColliders.Length > 0;

    //    if(insideCollider){
    //        dimension.disableAllColliders();
    //    }

    //    return (dimensionDistance < adjacentDimensionCheckDistance && !anythingInBetween && !insideCollider);
    //}

    private void disableCollidersBetween(Dimension dimension, Dimension adjacentDimension){
        Vector3 startPoint = dimension.transform.GetChild(0).transform.position;
        Vector3 endPoint = adjacentDimension.transform.GetChild(0).transform.position;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(startPoint, (endPoint - startPoint).normalized, Vector3.Distance(startPoint, endPoint) );

        foreach(RaycastHit hitted in hits){
            // print("hitted in hits loop: " + hitted.collider.gameObject.name);
            Dimension hitDimension = hitted.collider.gameObject.GetComponent<Dimension>();
            if(hitDimension != null){
                hitted.collider.enabled = false;
            }
        }
    }
}