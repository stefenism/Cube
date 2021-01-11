using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VIDE_Data;

public class SpeechBubbleScript : MonoBehaviour
{

    
    public TextController textC;
    public RectTransform bubble;
    VD2 dialogManager ;
    public GameObject UIHolder;
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
        if (textC.typing)
        {
            textC.SkipForward();
        }
        else if (dialogManager.isActive && dialogManager.nodeData.isEnd)
        {
            UIHolder.SetActive(false);
            PlayerManager.playerDaddy.player.playerSpeechBubble.UIHolder.SetActive(false);
            
        }
        else
        {


            if (!dialogManager.isActive)
            {
                Begin();
            }
            else
            {
                dialogManager.Next();
            }

            if (dialogManager.nodeData.isPlayer)
            {
                UIHolder.SetActive(false);
                PlayerManager.playerDaddy.player.playerSpeechBubble.UIHolder.SetActive(true);
                PlayerManager.playerDaddy.player.playerSpeechBubble.textC.SetText(  dialogManager.nodeData.comments[dialogManager.nodeData.commentIndex]);
            }
            else
            {
                UIHolder.SetActive(true);
                PlayerManager.playerDaddy.player.playerSpeechBubble.UIHolder.SetActive(false);
                textC.SetText(  dialogManager.nodeData.comments[dialogManager.nodeData.commentIndex]);
            }
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
        if(textC.tmpText != null)
        {
            //ajust the size of the bubble based on text
            //width
            bubble.sizeDelta = new Vector2(Mathf.Clamp((textC.tmpText.textBounds.size.x * 0.0058f) + 0.7f, 0.7f, 1000000), bubble.sizeDelta.y);
            //height
            bubble.offsetMax = new Vector2(bubble.offsetMax.x, (Mathf.Clamp(textC.tmpText.textBounds.size.y, 40, 100000) * 0.0056f - 1.55f)); //
        }
        
    }
}
