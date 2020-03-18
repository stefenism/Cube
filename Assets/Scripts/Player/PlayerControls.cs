using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    public PlayerManager playerManager;
    private EdgeDetect edgeDetect;

    public float speed = 5f;
    public float climbSpeed = 2f;
    private bool touchingPickupableObject;
    private Pickup itemToPickUp;
    private bool holdingItem = false;
    private bool onLadder = false;

    private float horMov;
    private float vertMov;

    public GameObject playerModel;
    Animator playerAnimator;

    [Range(0, 1)]
    public float accelerationFactor;
    [Range(0, 1)]
    public float decelFactor;
    public float maxSpeed = 10;

    private PlayerActor player;

    Vector3 runForce;

    float oldEulerRotation = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerActor>();
        playerAnimator = playerModel.GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        edgeDetect = GetComponentInChildren(typeof(EdgeDetect)) as EdgeDetect;
    }

    void Update()
    {
        CheckInput();
        AnimatePlayerModel();
    }

    void CheckInput()
    {
        horMov = Input.GetAxisRaw("Horizontal");
        vertMov = Input.GetAxisRaw("Vertical");

        checkPickup();
    }

    void FixedUpdate()
    {

        //if (onLadder)
        //{
        //    climbLadder();
        //}
        //else
        //{
        //    checkMovement();
        //}
        checkMovement();
    }

    void checkPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (holdingItem)
            {
                DropItem();
            }
            else
            {
                if (touchingPickupableObject && !onLadder)
                    GrabItem();
            }

        }
    }

    void checkMovement(){

        if (player.getDimension() != null){

            runForce = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            float modifier = ((Mathf.Abs(horMov % 1) != 0) && (Mathf.Abs(vertMov % 1) != 0)) ? decelFactor : accelerationFactor;

            Vector3 verticalSpeed = vertMov * transform.forward * speed * modifier;
            Vector3 horizontalSpeed = horMov * transform.right * speed * modifier;
            Vector3 gravitySpeed = player.getDimension().gravity;

            if (vertMov != 0 || horMov != 0){
                runForce += (verticalSpeed + horizontalSpeed) * modifier;
            }
            else{
                runForce -= rb.velocity * modifier;
            }

            if (onLadder){
                //gravitySpeed = Vector3.zero;
                gravitySpeed = -player.getDimension().gravity;
            }

            if(!playerManager.isPlayerEdging()){
                rb.velocity += runForce + gravitySpeed;
            }

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    public void fullStop(){
        playerManager.setPlayerEdging();
        Vector3 newVelocity = Vector3.zero;
        newVelocity += player.getDimension().gravity;
        rb.velocity = newVelocity;
    }

    public void notOnEdge(){
        if(playerManager.isPlayerEdging()){
            playerManager.setPlayerLiving();
        }
    }

    void climbLadder(){
        rb.velocity = (vertMov * transform.up * climbSpeed);
    }

    void GrabItem(){
        holdingItem = true;
        itemToPickUp.transform.SetParent(transform);
        itemToPickUp.transform.localPosition = new Vector3(0, 0.65f, 0);
    }

    void DropItem(){
        if (holdingItem){
            holdingItem = false;
            itemToPickUp.transform.localPosition = Vector3.zero;
            itemToPickUp.transform.SetParent(transform.parent);
        }
    }

    void AnimatePlayerModel(){
        if (horMov + vertMov != 0){ //set run speed for animation
            playerAnimator.SetFloat("Speed", (rb.velocity).magnitude / 7);
        }
        else{
            playerAnimator.SetFloat("Speed", (rb.velocity).magnitude / 10);
        }

        if (runForce.magnitude > 0.1){ //rotate player model

            //Determins how it should be rotated based on the player rotation and force direction -- does not work with x rotation. must fix
            Quaternion target = Quaternion.Euler(playerModel.transform.localRotation.eulerAngles.x, Quaternion.LookRotation(transform.rotation * new Vector3(runForce.x, -runForce.y, runForce.z)).eulerAngles.y, playerModel.transform.localRotation.eulerAngles.z);
            playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, target, 0.2f);

            float turnForce = Quaternion.Angle(playerModel.transform.localRotation, target) / 40;

            if (oldEulerRotation - playerModel.transform.eulerAngles.y > 2){
                playerAnimator.SetFloat("Rotation", -turnForce);
            }
            else if (oldEulerRotation - playerModel.transform.eulerAngles.y < -2){
                playerAnimator.SetFloat("Rotation", turnForce);
            }
            else{
                playerAnimator.SetFloat("Rotation", 0);
            }
            oldEulerRotation = playerModel.transform.eulerAngles.y;
        }
        else{
            playerAnimator.SetFloat("Rotation", 0);
        }

    }

    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Pickup>() != null){ //checks if entered range of pickup
            touchingPickupableObject = true;
            itemToPickUp = other.GetComponent<Pickup>();
        }
    }

    void OnTriggerExit(Collider other){
        if (other.GetComponent<Pickup>() == itemToPickUp){ //Checks if out of range of pickup
            touchingPickupableObject = false;
        }
    }

    private void OnCollisionEnter(Collision collision){
        Collider other = collision.collider;
        if (other.GetComponent<Ladder>() != null){ //If Hit Ladder
            DropItem();
            onLadder = true;
        }
    }
    private void OnCollisionExit(Collision collision){
        Collider other = collision.collider;
        if (other.GetComponent<Ladder>() != null){ //If Hit Ladder
            DropItem();
            onLadder = false;

        }
    }
}