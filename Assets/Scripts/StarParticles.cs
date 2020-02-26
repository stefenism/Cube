using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticles : MonoBehaviour
{

    
    ParticleSystem ps;
    Gradient grad = new Gradient();
    // Start is called before the first frame update
    void Start()
    {

        ParticleSystem ps = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {


        //var col = ps.colorOverLifetime;
        
        //grad.SetKeys(new GradientColorKey[] { new GradientColorKey(col.color.color, 0.0f), new GradientColorKey(col.color.color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        //col.color = grad;
    }
}
