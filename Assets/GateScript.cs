using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    Vector3 staringLocalPos;
    public Vector3 openOffsetPos;
    // Start is called before the first frame update
    void Start()
    {
        staringLocalPos = transform.localPosition;
    }

    bool moving = false;
    public bool open = false;

    public void Open()
    {
        moving = true;
        open = true;
    }

    public void Close()
    {
        moving = true;
        open = false;
    }

    float alpha = 0;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (open)
            {

                if (alpha < 1)
                {
                    alpha += Time.deltaTime;
                }
                else
                {
                    moving = false;
                }
            }
            else
            {

                if (alpha > 0)
                {
                    alpha -= Time.deltaTime;
                }
                else
                {
                    moving = false;
                }
            }

            transform.localPosition = Vector3.Lerp(staringLocalPos, openOffsetPos, alpha);
        }
    }
}
