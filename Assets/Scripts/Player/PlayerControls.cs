using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    public PlayerManager playerManager;
    private EdgeDetect edgeDetect;

    public float speed = 25f;
    public float climbSpeed = 2f;
    public float dashMultiplier = 3f;
    float currentDashMultipier = 1;
    private bool touchingPickupableObject;
    private Pickup itemToPickUp;
    private bool holdingItem = false;
    private bool onLadder = false;

    private float horMov;
    private float vertMov;
    private Vector3 dashDir;

    public GameObject playerModel;
    Animator playerAnimator;

    [Range(0, 1)]
    public float accelerationFactor;
    [Range(0, 1)]
    public float decelFactor;
    public float maxSpeed = 10;
    public float dashSpeed = 25;

    private PlayerActor player;

    Vector3 runForce;
    float characterRotation;

    float oldEulerRotation = 0;

    CameraController cameraController;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerActor>();
        playerAnimator = playerModel.GetComponent<Animator>();
        edgeDetect = GetComponentInChildren(typeof(EdgeDetect)) as EdgeDetect;
        cameraController = Camera.main.GetComponent<CameraController>();


    }

    void Update()
    {
        CheckInput();
        AnimatePlayerModel();
    }

    void CheckInput()
    {
        if (cameraController.playerMode)
        {
            horMov = Input.GetAxisRaw("Horizontal");
            vertMov = Input.GetAxisRaw("Vertical");

            CheckPickup();
        }
        else
        {
            horMov = 0;
            vertMov = 0;
        }

        CheckDash();
        HandleStickPush();
    }

    void CheckDash() {
        if(!playerManager.isPlayerDashing() && Input.GetButtonDown(ProjectConstants.DASH_BUTTON)){
            playerManager.setPlayerDashing();
            dashDir.x = horMov;
            dashDir.z = vertMov;
            dashDir.Normalize();
            Invoke("StopDash", 0.2f);
        }
    }

    void StopDash()
    {
        playerManager.setPlayerWalking();
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

    void CheckPickup()
    {
        if (Input.GetButtonDown("PickupDrop"))
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

    public bool pushingStick = false;
    void HandleStickPush()
    {
        
        if (Input.GetButtonDown("UseStick"))
        {
            
            pushingStick = true;


            RaycastHit hit;
            

            if (Physics.Raycast(transform.position, direction, out hit, 1.5f))
            {
                Actor actor;
                if (actor = hit.transform.gameObject.GetComponent<Actor>())
                {
                    actor.Push(direction);
                }
            }
        }

        if (pushingStick)
        {

        }
    }

    Vector3 verticalSpeed;
    Vector3 horizontalSpeed;
    Vector3 direction;
    void checkMovement(){

        if (player.getDimension() != null){

            runForce = rb.velocity;
            //float modifier = ((Mathf.Abs(horMov % 1) != 0) && (Mathf.Abs(vertMov % 1) != 0)) ? decelFactor : accelerationFactor;

            currentDashMultipier = 1;

            verticalSpeed = vertMov * transform.forward;
            horizontalSpeed = horMov * transform.right;


            Vector3 gravitySpeed = player.getDimension().gravity;

            if(playerManager.isPlayerWalking()){
                runForce = getPlayerWalkSpeed(verticalSpeed, horizontalSpeed);
            }
            else if(playerManager.isPlayerDashing()){
                runForce = getPlayerDashSpeed();
            }

            // if (vertMov != 0 || horMov != 0){
            //     runForce = (verticalSpeed + horizontalSpeed) * speed;
            // }
            // else{
            //     runForce -= runForce * decelFactor;
            // }

            if (onLadder){
                //gravitySpeed = Vector3.zero;
                gravitySpeed = -player.getDimension().gravity;
            }

           

            if(!playerManager.isPlayerEdging()){
                rb.velocity = runForce;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            if (!player.isGrounded)
            {
                rb.velocity +=  gravitySpeed*0.4f;
            }

            if (runForce.magnitude > 0.1)
            {
                direction = verticalSpeed + horizontalSpeed;
            }

                float clampSpeed = playerManager.isPlayerWalking() ? maxSpeed : dashSpeed;
            //rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampSpeed);
        }
    }

    Vector3 getPlayerWalkSpeed(Vector3 verticalSpeed, Vector3 horizontalSpeed){
        return (verticalSpeed + horizontalSpeed) * speed;
    }

    Vector3 getPlayerDashSpeed(){
        return dashDir * speed * dashMultiplier;
    }

    public void fullStop(){
        if (player.getDimension() != null)
        {
            playerManager.setPlayerEdging();
            Vector3 newVelocity = Vector3.zero;
            newVelocity += player.getDimension().gravity;
            rb.velocity = newVelocity;
        }
    }

    public void notOnEdge(){
        if(playerManager.isPlayerEdging()){
            playerManager.setPlayerLiving();
        }
    }

    void climbLadder(){
        rb.velocity = (vertMov * transform.up * climbSpeed);
    }

    Pickup grabbedItem;
    void GrabItem(){
        holdingItem = true;
        itemToPickUp.transform.SetParent(transform);
        itemToPickUp.transform.localPosition = new Vector3(0, 0.65f, 0);
        grabbedItem = itemToPickUp;
    }

    void DropItem(){
        if (holdingItem){
            holdingItem = false;
            grabbedItem.transform.localPosition = Vector3.zero;
            grabbedItem.transform.SetParent(transform.parent);
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
            if(vertMov != 0 || horMov != 0)
                characterRotation = Mathf.Atan2( horMov,vertMov) * Mathf.Rad2Deg;
            Quaternion target =  Quaternion.Euler(0,characterRotation-90,0);

            playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, target, 0.2f);

            //sets leaning
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