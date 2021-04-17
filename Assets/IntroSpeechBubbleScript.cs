using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VIDE_Data;

public class IntroSpeechBubbleScript : MonoBehaviour
{


    public TextController textC;
    public RectTransform bubble;

    VD2 dialogManager;
    public GameObject UIHolder;

    public TextController playerTextC;
    public RectTransform playerBubble;
    public GameObject playerUIHolder;
    // Start is called before the first frame update
    void Start()
    {
        dialogManager = new VD2();




    }

    void Begin()
    {

        dialogManager.BeginDialogue(GetComponent<VIDE_Assign>());
    }



    public void NextDialog()
    {


        invoked = false;


        if (!dialogManager.isActive)
        {
            Begin();
        }
        else
        {
            dialogManager.Next();

        }

        if (dialogManager.nodeData.isEnd)
        {
            UIHolder.SetActive(false);
            playerUIHolder.SetActive(false);
            speechDone = true;
            runSpeech = false;

            playerUIHolder.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            UIHolder.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        else if (dialogManager.nodeData.isPlayer)
        {
            UIHolder.SetActive(false);
            playerUIHolder.SetActive(true);
            playerTextC.SetText(dialogManager.nodeData.comments[dialogManager.nodeData.commentIndex]);
        }
        else
        {
            UIHolder.SetActive(true);
            playerUIHolder.SetActive(false);
            textC.SetText(dialogManager.nodeData.comments[dialogManager.nodeData.commentIndex]);

        }


    }

    public void ForceText(string text)
    {
        UIHolder.SetActive(true);
        textC.SetText(text);
    }

    public void ResetUI()
    {
        playerUIHolder.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UIHolder.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }



    public bool runSpeech = false;
    public bool speechDone = false;
    bool invoked = false;
    // Update is called once per frame
    void Update()
    {

        if (textC.tmpText != null)
        {
            //ajust the size of the bubble based on text
            //width

            bubble.sizeDelta = new Vector2(Mathf.Clamp((textC.widthFull * 0.0058f) + 0.7f, 0.7f, 1000000), bubble.sizeDelta.y);
            //height

            bubble.offsetMax = new Vector2(bubble.offsetMax.x, (Mathf.Clamp(textC.heightFull, 40, 100000) * 0.0056f - 1.55f)); //
        }
        if (!textC.typing && !playerTextC.typing && !invoked && runSpeech)
        {

            invoked = true;
            Invoke("NextDialog", 6f);


        }

    }
}
