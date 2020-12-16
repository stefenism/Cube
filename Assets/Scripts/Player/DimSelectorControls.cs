using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimSelectorControls : MonoBehaviour
{
    //controller Specific Vars
    float controllerHoriz;
    float controllerVert;
    //Mouse Vars
    Vector3 deltaMouse = Vector3.zero;
    Vector3 lastPosMouse  = Vector3.zero;
    Vector3 mouseMovedFromCenter = Vector3.zero;

    bool selectorReset = false;
    bool mouseControl = true;
    LevelBlockManipulation selectedManipulationScript;
    bool hasSelectedDimension = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        controllerHoriz = Input.GetAxis("Aim Horizontal");
        controllerVert = Input.GetAxis("Aim Vertical");



        if (!hasSelectedDimension)
        {
            
            if (Input.GetButtonDown("GrabDimension"))
            {
                SelectDimension();

            }
            else
            {
                MoveSelector();
            }


        }
        else
        {
            if (!selectedManipulationScript.isSelected)
            {
                UnselectDimension();
            }
            if (!Input.GetButton("GrabDimension"))
            {
                UnselectDimension();
            }
            else
            {
                GetMoveDirection();
            }
            if (directionIndicatorCurrent == null && selectedManipulationScript!=null)
            {
                
                if (!selectedManipulationScript.isMoving)
                {
                    HandleSelectorSquare();
                    SpawnDirectionIndicator();
                }
            }
        }

        lastPosMouse = Input.mousePosition;
    }

    float absHoriz;
    float absVert;

   

    void GetMoveDirection()
    {
        if (mouseControl)
        {
            deltaMouse = Input.mousePosition - lastPosMouse;
            mouseMovedFromCenter += deltaMouse;

            if(Mathf.Abs(mouseMovedFromCenter.x)>15 || Mathf.Abs(mouseMovedFromCenter.y) > 15)
            {
                if(Mathf.Abs(mouseMovedFromCenter.x)> Mathf.Abs(mouseMovedFromCenter.y))
                {
                    if (mouseMovedFromCenter.x > 0)
                    {
                        selectedManipulationScript.SnapMoveCube(-1, transform.forward, transform.up);
                        
                    }
                    else
                    {
                        selectedManipulationScript.SnapMoveCube(1, transform.forward, transform.up);
                    }
                }
                else
                {
                    if (mouseMovedFromCenter.y> 0)
                    {
                        selectedManipulationScript.SnapMoveCube(1, transform.right, transform.up);
                    }
                    else
                    {
                        selectedManipulationScript.SnapMoveCube(-1, transform.right, transform.up);

                    }
                }
                mouseMovedFromCenter = Vector3.zero;
            }

        }
        else
        {
            absHoriz = Mathf.Abs(controllerHoriz);
            absVert = Mathf.Abs(controllerVert);
            if ((absHoriz > 0.8f || absVert > 0.8f) && selectorReset)
            {

                if (absHoriz >= absVert)
                {
                    if (controllerHoriz > 0)
                    {
                        selectedManipulationScript.SnapMoveCube(-1, transform.forward, transform.up);
                    }
                    else
                    {
                        selectedManipulationScript.SnapMoveCube(1, transform.forward, transform.up);
                    }
                }
                else
                {
                    if (controllerVert > 0)
                    {
                        selectedManipulationScript.SnapMoveCube(1, transform.right, transform.up);
                    }
                    else
                    {
                        selectedManipulationScript.SnapMoveCube(-1, transform.right, transform.up);
                    }
                }
                DestroyDirectionIndicator();
                DestroySelectorSquare();



                selectorReset = false;
                if (directionIndicatorCurrent != null)
                {
                    directionIndicatorCurrent.GreySprites(true);
                }

                Invoke("doSetBoundaries", 1f);

            }
            else if (Mathf.Abs(controllerHoriz) + Mathf.Abs(controllerVert) < 0.5f && !selectorReset)
            {
                selectorReset = true;
                if (directionIndicatorCurrent != null)
                {
                    directionIndicatorCurrent.GreySprites(false);
                }
            }
        }
    }

    public void doSetBoundaries(){
        DimensionManager.dimensionDaddy.setNewBoundaries();
    }

    void SelectDimension()
    {
        Vector3 mouseMovedFromCenter = Vector3.zero;
        int layerMask = LayerMask.GetMask("Water");
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        
        if (Physics.Raycast(ray, out hit, 5, layerMask, QueryTriggerInteraction.Collide))
        {
            if (selectedManipulationScript != null) //make sure nothing is selected
            {
                UnselectDimension();
                
            }

            if (hit.transform.TryGetComponent(out selectedManipulationScript))//select a movable block if it can
            {
                selectedManipulationScript.Selected(true, hit.point);
                hasSelectedDimension = true;
                SpawnDirectionIndicator();
            }
            else if(highlightedManipulationScript !=null){
                selectedManipulationScript = highlightedManipulationScript;
                selectedManipulationScript.Selected(true, hit.point);
                hasSelectedDimension = true;
                SpawnDirectionIndicator();
            }
            else
            {
                MoveSelector();
            }

        }
        else if (highlightedManipulationScript != null){
            selectedManipulationScript = highlightedManipulationScript;
            selectedManipulationScript.Selected(true, hit.point);
            hasSelectedDimension = true;
            SpawnDirectionIndicator();
        }
        else
        {
            MoveSelector();
        }
    }


    public GameObject directionIndicatorPrefab;
    DirectionIndicatorScript directionIndicatorCurrent;

    /// <summary>
    /// Spawns the direction indicator on the current selected SuperCube
    /// </summary>
    void SpawnDirectionIndicator()
    {
        if (directionIndicatorCurrent != null)
        {
            DestroyDirectionIndicator();
        }
        directionIndicatorCurrent = Instantiate(directionIndicatorPrefab, selectedManipulationScript.transform.position+selectedManipulationScript.DirectionToClosestCardinal(transform.up)*3, transform.rotation).GetComponent<DirectionIndicatorScript>();
        selectedManipulationScript.DirectionToClosestCardinal(transform.up);

       directionIndicatorCurrent.GreySprites(!selectorReset);

        //finds arrow directions
        int forwardRot=0;
        int rightRot=0;
        Vector3 cardinalForward= selectedManipulationScript.DirectionToClosestCardinal(transform.forward);
        Vector3 cardinalRight = selectedManipulationScript.DirectionToClosestCardinal(transform.right);
        
        if (selectedManipulationScript.CanRotateOnCardinalAxis(cardinalForward))
        {
            forwardRot = 1;
        }else if (selectedManipulationScript.CanMoveOnCardinalAxis(cardinalForward))
        {
            forwardRot = 2;
        }
        if (selectedManipulationScript.CanRotateOnCardinalAxis(cardinalRight))
        {
            rightRot = 1;
        }
        else if (selectedManipulationScript.CanMoveOnCardinalAxis(cardinalRight))
        {
            rightRot = 2;
        }

        directionIndicatorCurrent.SetArrows(rightRot, rightRot, forwardRot, forwardRot);

    }

    void DestroyDirectionIndicator()
    {
        if (directionIndicatorCurrent != null)
        {
            Destroy(directionIndicatorCurrent.gameObject);
            directionIndicatorCurrent = null;
        }
    }

    void UnselectDimension()
    {
        if (selectedManipulationScript != null)
        {
            selectedManipulationScript.Selected(false);
            selectedManipulationScript = null;
            hasSelectedDimension = false;
            
        }
        selectorReset = false;
        DestroyDirectionIndicator();
    }

    void MoveSelector()
    {
        
        if (Mathf.Abs(controllerHoriz) + Mathf.Abs(controllerVert) > 0)
        {
            HandleSelectorSquare();
            transform.localPosition = new Vector3(controllerHoriz * 6, 0, controllerVert * 6);
        }
        else
        {
            DestroySelectorSquare();
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public GameObject selectorSquare;
    GameObject selectorSquareCurrent;
    LevelBlockManipulation highlightedManipulationScript;

    void HandleSelectorSquare()
    {

        int layerMask = LayerMask.GetMask("Water");
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, 5, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.TryGetComponent(out highlightedManipulationScript))//select a movable block if it can
            {
                if (selectorSquareCurrent == null)
                {
                    selectorSquareCurrent = Instantiate(selectorSquare, highlightedManipulationScript.transform.position + highlightedManipulationScript.DirectionToClosestCardinal(transform.up) * 3, transform.rotation);
                    selectorSquareCurrent.transform.SetParent(hit.transform);
                }
                else
                {
                    selectorSquareCurrent.transform.position = highlightedManipulationScript.transform.position + highlightedManipulationScript.DirectionToClosestCardinal(transform.up) * 3;
                    selectorSquareCurrent.transform.rotation = transform.rotation;
                    selectorSquareCurrent.transform.SetParent(hit.transform);
                }

            }

        }



    }

    void DestroySelectorSquare()
    {
        if (selectorSquareCurrent != null)
        {
            Destroy(selectorSquareCurrent);
            selectorSquareCurrent = null;
        }
    }
}
