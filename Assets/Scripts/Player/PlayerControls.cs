using UnityEngine;

public class PlayerControls : MonoBehaviour {
    private Rigidbody rb;
    public float speed = 5f;
    private bool touchingPickupableObject;
    private Pickup itemToPickUp;
    private bool holdingItem = false;
    private bool onLadder= false;

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
            Vector3 verticalSpeed = vertMov * player.getDimension().up * speed;
            Vector3 horizontalSpeed = horMov * player.getDimension().right * speed;
            Vector3 gravitySpeed = player.getDimension().gravity;
            if (onLadder)
                gravitySpeed = Vector3.zero; // temporary ladder code. To be changed when we make ladders less garbage


            Debug.Log("inside setting velocity: " + verticalSpeed);

            rb.velocity = verticalSpeed + horizontalSpeed + gravitySpeed;
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

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        if (other.GetComponent<Ladder>() != null)//If Hit Ladder
        {
            DropItem();
            onLadder = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Collider other = collision.collider;
        if (other.GetComponent<Ladder>() != null)//If Hit Ladder
        {
            DropItem();
            onLadder = false;
            
        }
    }

    void ManageLadder()//pushes the player up when on a ladder
    {
        rb.AddForce(transform.up *2000.3f);
    }

}