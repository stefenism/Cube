using UnityEngine;

public class PlayerControls : MonoBehaviour {
    private Rigidbody rb;
    public float speed = 5f;
    public float climbSpeed = 2f;
    private bool touchingPickupableObject;
    private Pickup itemToPickUp;
    private bool holdingItem = false;
    private bool onLadder= false;

    private float horMov;
    private float vertMov;

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
        checkInput();
    }

    void checkInput(){
        horMov = Input.GetAxisRaw("Horizontal");
        vertMov = Input.GetAxisRaw("Vertical");

        checkPickup();
    }

    void FixedUpdate() {

        if(onLadder){
            climbLadder();
        }
        else{
            checkMovement();
        }
    }

    void checkPickup(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(touchingPickupableObject && !onLadder){
                if(holdingItem){
                    DropItem();
                }
                else{
                    GrabItem();
                }
            }
        }
    }

    void checkMovement() {

        if(player.getDimension() != null){

            Vector3 runForce = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            float modifier = ((Mathf.Abs(horMov % 1) != 0) && (Mathf.Abs(vertMov % 1) != 0)) ? decelFactor : accelerationFactor;

            Vector3 verticalSpeed = vertMov * player.getDimension().up * speed * modifier;
            Vector3 horizontalSpeed = horMov * player.getDimension().right * speed * modifier;
            Vector3 gravitySpeed = player.getDimension().gravity;

            if(vertMov != 0 || horMov != 0){
                runForce += verticalSpeed + horizontalSpeed;
            }
            else{
                runForce -= rb.velocity * modifier;
            }
            
            if (onLadder){
                gravitySpeed = Vector3.zero;
            }

            rb.velocity += Vector3.ClampMagnitude(runForce + gravitySpeed, maxSpeed);
        }
    }

    void climbLadder(){
        rb.velocity = (vertMov * transform.up * climbSpeed);
    }

    void GrabItem()
    {
        holdingItem = true;
        itemToPickUp.transform.SetParent(transform);
        itemToPickUp.transform.localPosition = new Vector3(0, 1, 0);
    }

    void DropItem()
    {
        if (holdingItem){
            holdingItem = false;
            itemToPickUp.transform.localPosition = Vector3.zero;
            itemToPickUp.transform.SetParent(transform.parent);
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
}