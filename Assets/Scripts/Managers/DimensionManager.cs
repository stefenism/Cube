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

            RaycastHit hit; 
            if(Physics.Linecast(new Vector3(18.0f, 0, -30), new Vector3(18.0f, 0, -36), out hit, boundaryCheckLayer )){
                print("hit is:" + hit.collider.gameObject.name);
        
            }
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
            List<Dimension> samePlaneList = dimensionList.Where(loopDimension => loopDimension.getCurrentLayer() == dimension.getCurrentLayer() && loopDimension != dimension).ToList();
            List<Dimension> touchingList = samePlaneList.Where(samePlaneDimension => isAdjacentDimension(samePlaneDimension, dimension)).ToList();
            if(touchingList.Count > 0){
                print("***********************before looping touchinglist");
                print("dimension that's being tested is:" + dimension.gameObject.name);
                print("dimension rotated position:" + dimension.transform.rotation * dimension.transform.position);
                print("dimension daddy is:" + dimension.transform.parent.gameObject.name);
                foreach(Dimension printDimension in touchingList){
                    print("prinet demension is:" + printDimension.gameObject.name);
                    print("print dimension parent:" + printDimension.transform.parent.gameObject.name);
                    print("print dimension layer:" + printDimension.getCurrentLayer());
                    print("touching list length:" + touchingList.Count);
                    print("transform.position of dimension:" + printDimension.transform.rotation * printDimension.transform.position);
                    print("distance between this and testing dimension: " + Vector3.Distance(dimension.transform.rotation * dimension.transform.position, printDimension.transform.rotation * printDimension.transform.position));
                    print("-----------------------");
                }
                print("*********************after looping touchinglist");
            }
        }
    }

    private bool isAdjacentDimension(Dimension samePlaneDimension, Dimension dimension){

        // multiplying a quaternion by a vector finds the position of a point rotated about tha tquaternion
        Vector3 samePlaneDimensionRotatedPosition = samePlaneDimension.transform.rotation * samePlaneDimension.transform.position;
        Vector3 dimensionRotatedPosition = dimension.transform.rotation * dimension.transform.position;

        Vector3 samePlaneAnchorPosition = samePlaneDimension.transform.GetChild(samePlaneDimension.transform.childCount - 1).transform.position;
        Vector3 dimensionAnchoPosition = dimension.transform.GetChild(dimension.transform.childCount - 1).transform.position;

        // if(dimension.gameObject.name == "Side_Left"){
        //     print("##########################");
        //     print("loop dimension name:" + samePlaneDimension.gameObject.name);
        //     print("loop dimension parent name:" + samePlaneDimension.transform.parent.gameObject.name);
        //     print("loop dimension rotated position:" + samePlaneDimensionRotatedPosition);
        //     print("loop dimension parent position:" + samePlaneDimension.transform.parent.transform.position);
        //     print("test dimension name:" + dimension.gameObject.name);
        //     print("test dimension parent name:" + dimension.transform.parent.gameObject.name + " HERE");
        //     print("testDimension rotated position:" + dimensionRotatedPosition);
        //     print("test dimension parent position: " + dimension.transform.parent.transform.position);
        //     print("distance between the two:" + Vector3.Distance(samePlaneDimensionRotatedPosition, dimensionRotatedPosition));
        //     print("#############################");
        // }
        // print("##########################");
        // print("loop dimension name:" + loopDimension.gameObject.name);
        // print("loop dimension parent name:" + loopDimension.transform.parent.gameObject.name);
        // print("loop dimension rotated position:" + loopDimensionRotatedPosition);
        // print("loop dimension parent position:" + loopDimension.transform.parent.transform.position);
        // print("test dimension name:" + testDimension.gameObject.name);
        // print("test dimension parent name:" + testDimension.transform.parent.gameObject.name + " HERE");
        // print("testDimension rotated position:" + testDimensionRotatedPosition);
        // print("test dimension parent position: " + testDimension.transform.parent.transform.position);
        // print("distance between the two:" + Vector3.Distance(loopDimensionRotatedPosition, testDimensionRotatedPosition));
        // print("#############################");

        // Vector3 parentRotatedPosition = loopDimension.transform.parent.transform.rotation * loopDimension.transform.parent.transform.position;
        // print("loop dimension rotated position:" + loopDimensionRotatedPosition);
        // print("loop dimension:" + loopDimension.gameObject.name);
        // print("loop dimension dad:" + loopDimension.transform.parent.gameObject.name);
        // print("loop dimension dad rotated position:" + parentRotatedPosition);

        RaycastHit hit;

        float dimensionDistance = Vector3.Distance(samePlaneAnchorPosition, dimensionAnchoPosition);
        bool anythingInBetween = Physics.Linecast(dimensionAnchoPosition, samePlaneAnchorPosition, out hit, boundaryCheckLayer);
        // bool anythingInBetween = Physics.Raycast(dimensionAnchoPosition, dimensionAnchoPosition - samePlaneAnchorPosition, dimensionDistance, boundaryCheckLayer);
        Collider[] linecastOriginColliders = Physics.OverlapSphere(dimensionAnchoPosition, .2f, boundaryCheckLayer);
        bool insideCollider = linecastOriginColliders.Length > 0;
        // if(hit.collider.gameObject.GetComponent<Dimension>() != null){
        //     anythingInBetween = true;
        // }
        bool shareTwoPositionValues = checkSharingPositionValues(samePlaneDimensionRotatedPosition, dimensionRotatedPosition);


        return (dimensionDistance < 6.5f && !anythingInBetween && !insideCollider);

        // if(dimensionDistance < 7){
        //     print("##########################");
        //     print("loop dimension name:" + loopDimension.gameObject.name);
        //     print("loop dimension parent name:" + loopDimension.transform.parent.gameObject.name);
        //     print("loop dimension rotated position:" + loopDimensionRotatedPosition);
        //     print("loop dimension parent position:" + loopDimension.transform.parent.transform.position);
        //     print("------------------------");
        //     print("test dimension name:" + testDimension.gameObject.name);
        //     print("test dimension parent name:" + testDimension.transform.parent.gameObject.name);
        //     print("testDimension rotated position:" + testDimensionRotatedPosition);
        //     print("test dimension parent position: " + testDimension.transform.parent.transform.position);
        //     print("distance between the two:" + Vector3.Distance(loopDimensionRotatedPosition, testDimensionRotatedPosition));
        //     print("#############################");
        // }

        // return dimensionDistance < 6.5f;
    }

    bool checkSharingPositionValues(Vector3 samePlaneDimensionPosition, Vector3 dimensionPosition){
        bool exes = (Mathf.Abs(samePlaneDimensionPosition.x) - Mathf.Abs(dimensionPosition.x) < 1);
        bool whys = (Mathf.Abs(samePlaneDimensionPosition.y) - Mathf.Abs(dimensionPosition.y) < 1);
        bool zees = (Mathf.Abs(samePlaneDimensionPosition.z) - Mathf.Abs(dimensionPosition.z) < 1);

        return ((exes && whys) || (exes && zees) || (whys && zees) );
    }
}