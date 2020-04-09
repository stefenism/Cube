using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    List<Collider> onPlate = new List<Collider>();
    public GameObject plateButton;
    public GameObject[] gateObjects;
    public PressurePlatable[] platableObjects;
    bool platePressed  = false;
    float positionNormal = 1.0f;
    public bool ButtonLowersGate = true;
    public float animationSpeed = 0.05f;

    void Start()
    {

    }

    void Update()
    {
        //animates gates and plate
        if (platePressed)
        {
            if (ButtonLowersGate)
            {
                MoveGateDown(); 
            }
            else
            {
                MoveGateUp();
            }
            plateButton.SetActive(false);
        }
        else
        {
            if (ButtonLowersGate)
            {
                MoveGateUp();
            }
            else
            {
                MoveGateDown();
            }
            plateButton.SetActive(true);
        }
    }

    void MoveGateDown()
    {
        if (positionNormal > 0.0f)//move gate dwon
        {
            positionNormal -= animationSpeed;
            //plateButton.transform.localPosition = plateButton.transform.localPosition - new Vector3(0, 0.04f * animationSpeed, 0);
            // old way (doesn't work at different scales, for example)
            foreach (GameObject g in gateObjects)
            {
                g.transform.localPosition = g.transform.localPosition - new Vector3(0, 0.24f * animationSpeed, 0);
            }
            // new way
            foreach (PressurePlatable p in platableObjects)
            {
                p.UpdateWithNormalizedPosition(positionNormal);
            }
        }
    }
    void MoveGateUp()
    {
        if (positionNormal < 1.0f)//move gate up
        {
            positionNormal += animationSpeed;
            //plateButton.transform.localPosition = plateButton.transform.localPosition + new Vector3(0, 0.04f * animationSpeed, 0);
            // old hard-coded way
            foreach (GameObject g in gateObjects)
            {
                g.transform.localPosition = g.transform.localPosition + new Vector3(0, 0.24f * animationSpeed, 0);
            }
            // new way
            foreach (PressurePlatable p in platableObjects)
            {
                p.UpdateWithNormalizedPosition(positionNormal);
            }
        }
    }




    void OnTriggerEnter(Collider other)
    {
        print(other);
        if (!other.isTrigger)
        {
            print("Added " + other);
            onPlate.Add(other);
            UpdatePlate();
        }else if (other.TryGetComponent(out Pickup pickup))
        {
            print("Added " + other);
            onPlate.Add(other);
            UpdatePlate();
        }

    }

    void OnTriggerExit(Collider other)
    {

            print("Removed " + other);
            onPlate.Remove(other);
            UpdatePlate();


    }

    void UpdatePlate()
    {
        if (onPlate.Count > 0)
        {
            if (platePressed == false)//Presses Plate
            {
                platePressed = true;

            }
        }
        else
        {
            if (platePressed == true)//Releases Plate
            {
                platePressed = false;

            }

        }
    }
}
