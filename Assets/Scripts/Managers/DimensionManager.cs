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

            print("#######BLASH:" + Vector3.Distance(new Vector3(0,18,-30), new Vector3(0,24,-36)));
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
            print("***********************before looping touchinglist");
            if(touchingList.Count > 1){
                foreach(Dimension printDimension in touchingList){
                    print("prinet demension is:" + printDimension.gameObject.name);
                    print("print dimension parent:" + printDimension.transform.parent.gameObject.name);
                    print("print dimension layer:" + printDimension.getCurrentLayer());
                    print("touching list length:" + touchingList.Count);
                    print("transform.position of dimension:" + printDimension.transform.rotation * printDimension.transform.position);
                    if(touchingList.Count == 2){
                        print("distance between two touching doodles:" + Vector3.Distance(touchingList[0].transform.rotation * touchingList[0].transform.position, touchingList[1].transform.rotation * touchingList[1].transform.position));
                    }
                    print("-----------------------");
                }
            }
            print("*********************after looping touchinglist");
        }
    }

    private bool isAdjacentDimension(Dimension samePlaneDimension, Dimension dimension){

        // multiplying a quaternion by a vector finds the position of a point rotated about tha tquaternion
        Vector3 samePlaneDimensionRotatedPosition = samePlaneDimension.transform.rotation * samePlaneDimension.transform.position;
        Vector3 dimensionRotatedPosition = dimension.transform.rotation * dimension.transform.position;

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

        // float dimensionDistance = Vector3.Distance(loopDimensionRotatedPosition, testDimensionRotatedPosition);

        return Vector3.Distance(samePlaneDimensionRotatedPosition, dimensionRotatedPosition) < 6.5f;

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

    public Vector3 rotateAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation){
        return rotation * (point - pivot) + pivot;
    }
}