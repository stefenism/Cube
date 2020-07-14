using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicatorScript : MonoBehaviour
{
    public Material normalArrowMat;
    public Material curvedArrowMat;

    public SpriteRenderer forwardRend;
    public SpriteRenderer backwardRend;
    public SpriteRenderer leftRend;
    public SpriteRenderer rightRend;
    // Start is called before the first frame update
    void Start()
    {
        //SetArrows(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetArrows(int forwardState, int backwardState,int leftState,int rightState)
    {
        ChangeSprite(forwardRend, forwardState);
        ChangeSprite(backwardRend, backwardState);
        ChangeSprite(leftRend, leftState);
        ChangeSprite(rightRend, rightState);

    }

    public void GreySprites(bool greyed)
    {
        if (greyed)
        {
            normalArrowMat.color = new Color(0.5f, 0.5f, 0.4f);
            curvedArrowMat.color = new Color(0.4f, 0.5f, 0.5f);
        }
        else
        {
            normalArrowMat.color = new Color(1f, 1f, 0.7f);
            curvedArrowMat.color = new Color(0.8f, 1f, 1f);
        }

    }


    void ChangeSprite (SpriteRenderer renderer, int state)
    {
        switch (state)
        {
            case 0:
                renderer.enabled = false;
                break;
            case 1:
                renderer.material = curvedArrowMat;
                break;
            case 2:
                renderer.material = normalArrowMat;
                break;

        }
    }
}
