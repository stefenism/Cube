using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public Animation vendor;
    public GameObject cape;
    bool moveCape = false;
    float capeAlpha = 0;
    GameObject player;
    Vector3 capeStart;

    public Transform startingCameraPoint;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.GetComponent<CameraController>().SetCameraView(startingCameraPoint);
        player = PlayerManager.playerDaddy.player.gameObject;
        //Invoke("Shrink", 1f);
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
                //Camera.main.GetComponent<CameraController>().SetPlayerView();
            }
        }
    }
}
