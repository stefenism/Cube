using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoxes : MonoBehaviour
{
    public GameObject[] boxes;
    Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[boxes.Length];
        int x = 0;
        foreach(GameObject g in boxes)
        {
            positions[x] = g.transform.position;
            x++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool on = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && on)
        {

            int x = 0;
            foreach (GameObject g in boxes)
            {
                  g.transform.position = positions[x];
                x++;
            }
        }
    }
}
