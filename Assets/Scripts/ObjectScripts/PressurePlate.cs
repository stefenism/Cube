using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    List<Collider> onPlate = new List<Collider>();
    public GameObject plateButton;
    public GameObject[] gateObjects;
    bool platePressed  = false;
    float gatePosition = 1;
    public float animationSpeed = 0.05f;


    void Start()
    {

    }


    void Update()
    {
        //animates gates and plate
        if (platePressed)
        {
            if (gatePosition>0)//move gate dwon
            {
                gatePosition -= animationSpeed;
                plateButton.transform.localPosition = plateButton.transform.localPosition - new Vector3(0, 0.04f * animationSpeed, 0);
                foreach (GameObject g in gateObjects)
                {
                    g.transform.localPosition = g.transform.localPosition - new Vector3(0, 0.24f * animationSpeed, 0);
                }

            }
        }
        else
        {
            if (gatePosition < 1)//move gate up
            {
                gatePosition += animationSpeed;
                plateButton.transform.localPosition = plateButton.transform.localPosition + new Vector3(0, 0.04f*animationSpeed, 0);
                foreach (GameObject g in gateObjects)
                {
                    g.transform.localPosition = g.transform.localPosition + new Vector3(0, 0.24f * animationSpeed, 0);
                }
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
