using UnityEngine;

public class PlayerManager : MonoBehaviour {
    private Rigidbody r;
    public float speed = 5f;

    void Update()
    {
        //This probably shold be moved 
        if (Input.GetKey(KeyCode.W))
        {
            r.AddForce(transform.forward * speed);
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            r.AddForce(-transform.right * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            r.AddForce(-transform.forward * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            r.AddForce(transform.right * speed);
        }
    }
    
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }
}