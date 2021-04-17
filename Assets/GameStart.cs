using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public Animation vendor;
    public GameObject cape;

    public Animation customer;

    bool moveCape = false;
    float capeAlpha = 0;
    GameObject player;
    Vector3 capeStart;


    public Transform startingCameraPoint;

    public IntroSpeechBubbleScript introScript;
    public bool skipIntro = true;
    // Start is called before the first frame update
    public GameObject portal;
    void Start()
    {
        if (skipIntro)
        {
            portal.SetActive(true);
            key.SetActive(true);
            Camera.main.GetComponent<CameraController>().SetCameraView(startingCameraPoint);
            Camera.main.GetComponent<CameraController>().SetPlayerView();
        }
        else
        {
            player = PlayerManager.playerDaddy.player.gameObject;
            player.SetActive(false);
            Camera.main.GetComponent<CameraController>().SetCameraView(startingCameraPoint);

            //Invoke("Shrink", 5f);
            Invoke("StartSpeech", 3f);
        }


    }

    void StartSpeech()
    {
        introScript.runSpeech = true;
        InvokeRepeating("WaitForSpeechDone", 0.1f, 0.1f);
    }




    void Shrink()
    {
        cape.transform.parent = null;
        capeStart = cape.transform.position;
        moveCape = true;
        vendor.Stop();
        vendor.PlayQueued("InCape");
        Invoke("SetIdle", 1f);
    }

    void SetIdle()
    {
        vendor.Stop();
        vendor.PlayQueued("Idle");

    }

    void WaitForSpeechDone()
    {
        if (introScript.speechDone)
        {
            BrainExplodeEffect();
            CancelInvoke("WaitForSpeechDone");
        }
    }

    public GameObject BrainPoint;
    public GameObject BrainParticle;
    public GameObject key;

    void BrainExplodeEffect()
    {
        Instantiate(BrainParticle, BrainPoint.transform);
        Invoke("KeyEffect", 0.7f);
        Invoke("CustomerGrab", 1.2f);
    }
    void KeyEffect()
    {
        key.SetActive(true);
        Instantiate(BrainParticle, key.transform);
    }

    void CustomerGrab()
    {
        //introScript.ForceText("Hopefully, I won't have to be back again.");
        customer.Stop();
        customer.PlayQueued("Grab");
        Invoke("GrabBowl", 1.2f);
        Invoke("CustomerLeave", 3f);
    }
    public GameObject bowl;
    public GameObject handPoint;

    void GrabBowl()
    {
        bowl.transform.SetParent(handPoint.transform);
    }

    void CustomerLeave()
    {

        customer.Stop();
        customer.PlayQueued("Leave");
        Invoke("Shrink", 3f);
        Invoke("RemoveCustomer", 6f);

    }


    void RemoveCustomer()
    {
        Destroy(customer.gameObject);
    }
    public SpeechBubbleScript ramenSpeech;

    // Update is called once per frame
    void Update()
    {
        if (moveCape)
        {
            capeAlpha += 0.01f;
            cape.transform.position = Vector3.Lerp(capeStart, player.transform.position, capeAlpha);
            if (capeAlpha >= 1)
            {
                moveCape = false;
                cape.SetActive(false);
                player.SetActive(true);
                Camera.main.GetComponent<CameraController>().SetPlayerView();
                Invoke("StartNextTalk", 1f);
                
            }
        }
    }

    void StartNextTalk()
    {
        
        ramenSpeech.NextDialog();
        Invoke("Idontunderstandwhyineedthis", 0.1f);
    }
    void Idontunderstandwhyineedthis()
    {
        introScript.ResetUI();
    }
}


