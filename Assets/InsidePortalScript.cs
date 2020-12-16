using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsidePortalScript : MonoBehaviour
{
    public InsideController ic;
    public GameObject darkness;
    Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            ic.OpenPortal(other.gameObject);

            darkness.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {

            ic.TeleportPlayer(other.gameObject);

        }
    }
}
