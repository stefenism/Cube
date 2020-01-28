using UnityEngine;

public class PlayerControls : MonoBehaviour {
    private Rigidbody rb;
    public float speed = 5f;
    private bool touchingPickupableObject;
    private Pickup itemToPickUp;
    private bool holdingItem = false;
    private bool onLadder= false;

    [Range(0,1)]
    public float accelerationFactor;
    [Range(0,1)]
    public float decelFactor;
    public float maxSpeed = 10;

    private PlayerActor player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerActor>();
    }

    void Update()
    {
        //This probably shold be moved 
        // if (Input.GetKey(KeyCode.W))
        // {
        //     rb.velocity = player.getDimension().up * speed;
        //     // rb.AddForce(transform.forward * speed);

        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     rb.AddForce(-transform.right * speed);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     rb.AddForce(-transform.forward * speed);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     rb.AddForce(transform.right * speed);
        // }

        //pickup code
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (touchingPickupableObject & !onLadder)
            {
                if (holdingItem)
                {
                    DropItem();
                }
                else
                {
                    GrabItem();
                }

            }
        }



    }

    void FixedUpdate() {

        checkMovement();
        if (onLadder)
            ManageLadder();
    }

    void checkMovement() {
        float vertMov = Input.GetAxis("Vertical");
        float horMov = Input.GetAxis("Horizontal");

        if(player.getDimension() != null){

            Vector3 runForce = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            float modifier = ((Mathf.Abs(horMov % 1) != 0) && (Mathf.Abs(vertMov % 1) != 0)) ? decelFactor : accelerationFactor;
            print("modifier: " + modifier);

            Vector3 verticalSpeed = vertMov * player.getDimension().up * speed * modifier;
            Vector3 horizontalSpeed = horMov * player.getDimension().right * speed * modifier;
            Vector3 gravitySpeed = player.getDimension().gravity;
            print("hormov: " + (horMov %1));

            runForce += verticalSpeed + horizontalSpeed;
            
            if (onLadder){
                gravitySpeed = Vector3.zero; // temporary ladder code. To be changed when we make ladders less garbage
                runForce = Vector3.zero;
            }


            // Debug.Log("inside setting velocity: " + verticalSpeed);

            rb.velocity += Vector3.ClampMagnitude(runForce + gravitySpeed, maxSpeed);
        }
    }

    void GrabItem()
    {

        holdingItem = true;
        itemToPickUp.transform.SetParent(transform);
        itemToPickUp.transform.localPosition = new Vector3(0, 1, 0);
        //itemToPickUp.GetComponent<Collider>().enabled = false;


    }

    void DropItem()
    {
        if (holdingItem)
        {
            holdingItem = false;
            itemToPickUp.transform.localPosition = Vector3.zero;
            itemToPickUp.transform.SetParent(transform.parent);
            //itemToPickUp.GetComponent<Collider>().enabled = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Pickup>() != null)//checks if entered range of pickup
        {
            touchingPickupableObject = true;
            itemToPickUp = other.GetComponent<Pickup>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pickup>() == itemToPickUp)//Checks if out of range of pickup
        {
            touchingPickupableObject = false;
        }
    }

    void ManageLadder()//pushes the player up when on a ladder
    {
        rb.AddForce(transform.up *2000.3f);
        rb.velocity = ((rb.velocity + transform.up) * speed);
    }

}