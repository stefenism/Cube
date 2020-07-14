using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelBlockManipulation : MonoBehaviour
{
    //Setting State Enum
    public enum MovementEnum
    {
        None,
        Rotate,
        Move
    };
    //Setting variables
    public MovementEnum xAxisMovement = MovementEnum.None;  // this public var should appear as a drop down
    public MovementEnum yAxisMovement = MovementEnum.None;  // this public var should appear as a drop down
    public MovementEnum zAxisMovement = MovementEnum.None;  // this public var should appear as a drop down
    public float maxSlideDistance;//How far it can move
    public float startingSlidePosition;// The current position in that track
    public bool locked;//If its locked
    public int unlockCost = 1;//how much it costs to unlock
    public GameObject[] locks;//Just gameobjects that represent the locks. Will need to change eventually


    //Movement states
    [HideInInspector] public bool isSelected = false;
    bool addRotation = false;
    bool addMovement = false;
    [HideInInspector] public bool isMoving = false;


    void Start()
    {



    }


    // Update is called once per frame
    void Update()
    {

        if (addRotation)
        {
            HandleCubeRotation();
        }else if (addMovement)
        {
            HandleCubeMove();
        }

        else if (isMoving)
        {//TODO: Change to Invoke for performance
            var vec = transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(vec), 0.2f);//Do this but better
            if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(transform.rotation, Quaternion.Euler(vec))), 1.0f))
            {
                isMoving = false;
            }
        }

    }



    Vector3 selectorTransformDirection;
    Vector3 selectorMoveDirection;

    float moveDirection;
    float totalMoved;
    bool queuedMove = false;
    float queuedDir;
    Vector3 queuedTransformDir;
    Vector3 queuedUpDirection;

    /// <summary>
    /// Gives a cube a direction to move/rotate in
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="transformDirection"></param>
    public void SnapMoveCube(float direction, Vector3 transformDirection,Vector3 upDirection)
    {
        if (!isMoving)
        {

            selectorTransformDirection = DirectionToClosestCardinal(transformDirection);
            if (CanRotateOnCardinalAxis(selectorTransformDirection))//Stops if its not supose to rotate in that direction
            {
                moveDirection = direction * 10;//Sets forward or backwards movement multiplied by a factor of 90 for speed
                totalMoved = 0;//reset amount to move

                addRotation = true;//starts HandleCubeMove
                isMoving = true;
            }else if (CanMoveOnCardinalAxis(selectorTransformDirection))// For movement
            {
                moveDirection =direction;
                totalMoved = 0;//reset amount to move
                
                selectorMoveDirection =Quaternion.AngleAxis(90,upDirection)*selectorTransformDirection;
                addMovement = true;//starts HandleCubeMove
                isMoving = true;
                isSelected = false;
            }
            else
            {
                return;
            }

        }
        else
        {//This takes an input if the cube is already moving to be performed next
            queuedMove = true;
            queuedDir = direction;
            queuedTransformDir = transformDirection;
            queuedUpDirection = upDirection;
        }



    }


    /// <summary>
    /// Checks if the vector is a valid rotation axis. Only returns true for cardinal directions nomalized to 1.
    /// </summary>
    /// <param name="cardinalAxis"></param>
    /// <returns></returns>
    public bool CanRotateOnCardinalAxis(Vector3 cardinalAxis)
    {


        if (Mathf.Abs(Mathf.Abs(cardinalAxis.x)) == 1)
        {
            if (xAxisMovement == MovementEnum.Rotate)
            {
                return true;
            }

        }
        else if (Mathf.Abs(Mathf.Abs(cardinalAxis.y)) == 1)
        {

            if (yAxisMovement == MovementEnum.Rotate)
            {
                return true;
            }

        }
        else
        {
            if (Mathf.Abs(Mathf.Abs(cardinalAxis.z)) == 1)
            {

                if (zAxisMovement == MovementEnum.Rotate)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanMoveOnCardinalAxis(Vector3 cardinalAxis)
    {


        if (Mathf.Abs(Mathf.Abs(cardinalAxis.x)) == 1)
        {
            if (xAxisMovement == MovementEnum.Move)
            {
                return true;
            }

        }
        else if (Mathf.Abs(Mathf.Abs(cardinalAxis.y)) == 1)
        {

            if (yAxisMovement == MovementEnum.Move)
            {
                return true;
            }

        }
        else
        {
            if (Mathf.Abs(Mathf.Abs(cardinalAxis.z)) == 1)
            {
                if (zAxisMovement == MovementEnum.Move)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Converts a normal vector into its closest axis(x y or Z) negitive or positve.
    /// </summary>
    /// <param name="direction">The direction to convert</param>
    /// <returns></returns>
    public Vector3 DirectionToClosestCardinal(Vector3 direction)
    {
        float max3 = Mathf.Max(Mathf.Abs(direction.x), Mathf.Max(Mathf.Abs(direction.y), Mathf.Abs(direction.z)));
        if (Mathf.Abs(direction.x) == max3)
        {

            if (direction.x > 0)
            {
                return new Vector3(1, 0, 0);
            }
            else
            {
                return new Vector3(-1, 0, 0);
            }

        }
        else if (Mathf.Abs(direction.y) == max3)
        {


            if (direction.y > 0)
            {
                return new Vector3(0, 1, 0);
            }
            else
            {
                return new Vector3(0, -1, 0);
            }


        }
        else
        {

            if (direction.z > 0)
            {
                return new Vector3(0, 0, 1);
            }
            else
            {
                return new Vector3(0, 0, -1);
            }


        }
    }


    void HandleCubeRotation()
    {
        transform.RotateAround(transform.position, selectorTransformDirection, moveDirection);
        totalMoved += Mathf.Abs(moveDirection);
        if (totalMoved >= 90)
        {
            addRotation = false;
            if (queuedMove)
            {
                queuedMove = false;
                SnapMoveCube(queuedDir, queuedTransformDir,queuedUpDirection);
            }
        }
    }
    void HandleCubeMove()
    {
        transform.position += ( selectorMoveDirection) *-moveDirection;
        totalMoved += Mathf.Abs(moveDirection);
        if (totalMoved >= 6)
        {
            addMovement = false;
            if (queuedMove)
            {
                queuedMove = false;
                SnapMoveCube(queuedDir, queuedTransformDir, queuedUpDirection);
            }
        }
    }


    /// <summary>
    /// If locked, attempts to unlock. Otherwise selects or unselects supercube for rotation;
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="hitPosition"></param>
    public void Selected(bool selected, Vector3 hitPosition = new Vector3())
    {
        if (selected)
        {
            //Trys to unlock if locked
            if (locked)
            {
                if (DimensionManager.dimensionDaddy.player.GetComponent<PlayerManager>().PayKeys(unlockCost))
                {
                    locked = false;
                    foreach (GameObject l in locks)
                    {
                        l.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject l in locks)
                    {
                        l.transform.localScale = l.transform.localScale * UnityEngine.Random.Range(0.95f, 1.05f);
                    }
                }
            }
            else
            {
            }
        }
        else
        {


        }

        isSelected = selected;
    }


}
