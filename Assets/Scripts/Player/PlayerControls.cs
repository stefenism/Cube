using System.Collections.Generic;
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
    public CapeScript capeScript;

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

    public SpeechBubbleScript playerSpeechBubble;

    bool disableControls = false;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerActor>();
        edgeDetect = GetComponentInChildren(typeof(EdgeDetect)) as EdgeDetect;
        cameraController = Camera.main.GetComponent<CameraController>();


    }
    
    void Update()
    {
        if(!disableControls)
            CheckInput();
        CheckInteract();
        AnimatePlayerModel();
    }
    void FixedUpdate()
    {
        checkMovement();

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
        
        HandleAirPush();
    }

    void CheckDash() {
        if(!playerManager.isPlayerDashing() && Input.GetButtonDown(ProjectConstants.DASH_BUTTON)){
            playerManager.setPlayerDashing();
            dashDir = (horMov * transform.right)+(vertMov * transform.forward); 
            
            dashDir.Normalize();
            Invoke("StopDash", 0.2f);
        }
    }

    public void DisableControls(bool disable)
    {
        if (disable)
        {
            horMov = 0;
            vertMov = 0;
            disableControls = true;
        }
        else
        {
            disableControls = false;
        }
    }

    void StopDash()
    {
        playerManager.setPlayerWalking();
    }



    void CheckInteract()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (overlapingInteracts.Count > 0)
            {
                overlapingInteracts[0].Interact();
            }
        }
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

    public bool airPushing = false;
    void HandleAirPush()
    {
        
        if (Input.GetButtonDown("UseStick") && !airPushing)
        {
            airPushing = true;
            capeScript.CapePushAnimation(0.5f);
            Invoke("PushAir", 0.5f);


        }


    }

    void PushAir()
    {
        


        RaycastHit[] hits;
        LayerMask mask = 1 << gameObject.layer;
        hits = Physics.SphereCastAll(transform.position, 0.1f, direction, 0.9f, mask);

        foreach(RaycastHit hit in hits)
        {
            Actor actor;//Blocks
            if (actor = hit.transform.gameObject.GetComponent<Actor>())
            {
                actor.Push(direction);
            }
            Spinner spinner;
            if (spinner = hit.transform.gameObject.GetComponent<Spinner>())
            {
                spinner.Push();
            }
            AirSwitch airSwitch;
            if (airSwitch = hit.transform.gameObject.GetComponent<AirSwitch>())
            {
                airSwitch.Push(direction);
            }
        }
            
        
        airPushing = false;
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
        //holdingItem = true;
        //itemToPickUp.transform.SetParent(transform);
        //itemToPickUp.transform.localPosition = new Vector3(0, 0.65f, 0);
        //grabbedItem = itemToPickUp;
    }

    void DropItem(){
        //if (holdingItem){
        //    holdingItem = false;
        //    grabbedItem.transform.localPosition = Vector3.zero;
        //    grabbedItem.transform.SetParent(transform.parent);
        //}
    }

    void AnimatePlayerModel(){
        if (horMov + vertMov != 0){ //set run speed for animation
            //playerAnimator.SetFloat("Speed", (rb.velocity).magnitude / 7);
        }
        else{
            //playerAnimator.SetFloat("Speed", (rb.velocity).magnitude / 10);
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
                //playerAnimator.SetFloat("Rotation", -turnForce);
            }
            else if (oldEulerRotation - playerModel.transform.eulerAngles.y < -2){
                //playerAnimator.SetFloat("Rotation", turnForce);
            }
            else{
                //playerAnimator.SetFloat("Rotation", 0);
            }
            oldEulerRotation = playerModel.transform.eulerAngles.y;
        }
        else{
            //playerAnimator.SetFloat("Rotation", 0);
        }

    }

    List<InteractableObject> overlapingInteracts = new List<InteractableObject>();
    void OnTriggerEnter(Collider other){

        if (other.GetComponent<Pickup>() != null){ //checks if entered range of pickup
            touchingPickupableObject = true;
            itemToPickUp = other.GetComponent<Pickup>();
        }
        var inter = other.GetComponent<InteractableObject>();
        if (inter != null)
        {
            if(!overlapingInteracts.Contains(inter))
                overlapingInteracts.Add(inter);
        }

    }

    void OnTriggerExit(Collider other){
        if (other.GetComponent<Pickup>() == itemToPickUp){ //Checks if out of range of pickup
            touchingPickupableObject = false;
        }
        var inter = other.GetComponent<InteractableObject>();
        if (inter != null)
        {
            if (overlapingInteracts.Contains(inter))
                overlapingInteracts.Remove(inter);
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