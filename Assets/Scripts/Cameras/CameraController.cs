using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool playerMode = true;
    public GameObject player;
    public Transform playerCameraLocation;

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;
    float xAdded = 0.0f;
    float yAdded = 0.0f;
   
    public float cameraState;

    public StarParticles sParticles;

    private void Awake()
    {
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        player = pm.gameObject;
        playerCameraLocation = pm.playerCameraLocation.transform;
        sParticles = pm.playerStarParticles;
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
        MoveCamera();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Shift View"))
        {
            ChangeView();
        }

        HandleCameraPositionEase();
        CheckBlockClick();
    }

    LevelBlockManipulation selectedManipulationScript;

    /// <summary>
    /// Uses raycasts to select movable blocks
    /// </summary>
    void CheckBlockClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = LayerMask.GetMask("Water");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, layerMask, QueryTriggerInteraction.Collide))
            {
                if (selectedManipulationScript != null) //make sure nothing is selected
                {
                    selectedManipulationScript.Selected(false);
                    selectedManipulationScript = null;
                }

                if (hit.transform.TryGetComponent(out selectedManipulationScript))//select a movable block if it can
                {
                    selectedManipulationScript.Selected(true,hit.point);
                }
                
            }

        }
        if (Input.GetMouseButtonUp(0))//release movable block
        {
            if(selectedManipulationScript != null)
            {
                selectedManipulationScript.Selected(false);
                selectedManipulationScript = null;
            }
                
        }
    }




    public void ChangeView()//changes between camera views
    {
        if (playerMode)
        {
            sParticles.Fade(true);
        }
        else
        {
            sParticles.Fade(false);
        }
        playerMode = !playerMode;
    }

    void LateUpdate()
    {
        if (Input.GetButton("Rotate Camera"))
        {
            getMouseInput();

        }
        MoveCamera();
    }

    /// <summary>
    /// Gets mouse input for super camera movement
    /// </summary>
    void getMouseInput()
    {
        xAdded += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
        yAdded -= Input.GetAxis("Mouse Y") * ySpeed * distance * 0.02f;
    }

    /// <summary>
    /// Calculates where the camera should be and moves it there
    /// </summary>
    void MoveCamera()
    {
        if (player.transform)
        {
            //for smoothing mouse input
            x = Mathf.Lerp(xAdded, x, 0.7f);
            y = Mathf.Lerp(yAdded, y, 0.7f);

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = player.transform.rotation * Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + player.transform.position;
            
            transform.rotation = Quaternion.Lerp(rotation,playerCameraLocation.rotation,cameraState);
            transform.position = Vector3.Lerp(position,playerCameraLocation.position,cameraState);
        }

    }

    void HandleCameraPositionEase()//eventually needs to be invoked 
    {
        if (playerMode)
        {
            if (cameraState <= 1)
            {
                cameraState += 0.1f;
            }
            else
            {
                //reset superview
                x = -20;
                y = 50;
                xAdded = x;
                yAdded = y;

                //put cancel invoke here
            }
        }
        else
        {
            if (cameraState > 0)
            {
                

                cameraState -= 0.1f;
            }
            else
            {

                //put cancel invoke here
            }

        }
    }


    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        else if (angle > 360F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }
}