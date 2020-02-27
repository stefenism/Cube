using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticles : MonoBehaviour
{


    ParticleSystem ps;
    ParticleSystemRenderer psR;

    float opac = 0;
    bool fadeDirection;

    public float fadeInSpeed = 0.15f;
    public float fadeOutSpeed = 0.15f;
    // Start is called before the first frame update
    void Start()
    {

        ps = GetComponent<ParticleSystem>();
        psR = GetComponent<ParticleSystemRenderer>();

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void Fade(bool fadeIn)
    {
        if (fadeIn)
            ps.Play();
        opac = psR.material.GetFloat("_Opacity");
        fadeDirection = fadeIn;
        InvokeRepeating("HandleFade", 0.05f, 0.05f);
    }



    void HandleFade()
    {
        if (fadeDirection)//add or subtract opac
        {
            opac += fadeInSpeed;
        }
        else
        {
            opac -= fadeOutSpeed;
        }

        psR.material.SetFloat("_Opacity", opac);//Change the Opacity

        if (fadeDirection)
        {
            if (opac >= 1)
            {
                //Finish Fade in
                CancelInvoke("HandleFade");
            }
        }
        else
        {
            if (opac <= 0)
            {
                //Finish Fade Out
                ps.Clear();
                CancelInvoke("HandleFade");

            }
        }
    }
}
