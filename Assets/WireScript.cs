using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireScript : MonoBehaviour
{

    public bool powered;
    public float powerLevel = 1;
    public MeshRenderer[] wireMeshes;

    public GameObject[] connectedObjects;

    public void Power(bool on , float startingPowerLevel)
    {
        powered = on;
        powerLevel = startingPowerLevel;
        MaterialUpdate();


        if (on)
        {
            foreach(GameObject g in connectedObjects)
            {
                GateScript gate;
                if(g.TryGetComponent<GateScript>(out gate))
                {
                    gate.Open();
                }
            }  
        }
        else
        {
            foreach (GameObject g in connectedObjects)
            {
                GateScript gate;
                if (g.TryGetComponent<GateScript>(out gate))
                {
                    gate.Close();
                }
            }
        }
    }

    public void ChangePowerLevel(float level)
    {
        powerLevel = level;
        MaterialUpdate();
    }


    void MaterialUpdate()
    {
        foreach(MeshRenderer meshr in wireMeshes)
        {
            meshr.material.SetFloat("_PowerLevel", powerLevel);
            meshr.material.SetInt("_Powered", System.Convert.ToInt32(powered));
        }
        
    }
}
