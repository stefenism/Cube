using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendrilFader : MonoBehaviour
{
    Dimension parentDimension;
    PlayerControls player;
    MeshRenderer mesh;
    public GameObject wall;
    Collider wallCollider;
    // Start is called before the first frame update
    void Start()
    {
        parentDimension = transform.parent.parent.gameObject.GetComponent<Dimension>();
        mesh = GetComponent<MeshRenderer>();
        wallCollider = wall.GetComponent<Collider>();
        player = PlayerManager.playerDaddy.player;
    }

    bool on = false;
    float cuttoff = 1;
    // Update is called once per frame
    void Update()
    {

        
            if (V3AboutEqual(DimensionManager.dimensionDaddy.visableDimensionVector, parentDimension.gravityDown.normalized) && wallCollider.enabled)
            {
                on = true;
                if (cuttoff > 0)
                {
                    cuttoff -= 0.02f;
                    mesh.material.SetFloat("_Cuttoff", cuttoff);
                }
            }
            else
            {
                on = false;
                if (cuttoff < 1)
                {
                    cuttoff += 0.02f;
                    mesh.material.SetFloat("_Cuttoff", cuttoff);
                }
            }
        
       


        

    }


    public bool V3AboutEqual(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.01;
    }
}
