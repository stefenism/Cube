using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextController : MonoBehaviour
{

	string txt ="";
	public bool typing = false;
	
	public TMP_Text tmpText;
	void Awake()
	{
		tmpText = GetComponent<TMP_Text>();
	
		
	}

	public void SkipForward()
    {
		ClearText();
		tmpText.text = txt;
    }

	public void SetText(string speechText)
    {
		ClearText();
		txt = speechText;
		StartCoroutine("PlayText");
	}

	public void ClearText()
    {
		typing = false;
		StopCoroutine("PlayText");
		tmpText.text = "";
	}

	bool richTextNext=false;
	IEnumerator PlayText()
	{
		typing = true;
		foreach (char c in txt)
		{
			if(c.ToString() == "<")
            {
				richTextNext = true;
            }

			tmpText.text += c;

			if(!richTextNext)
				yield return new WaitForSeconds(0.04f);

			if (c.ToString() == ">")
			{
				richTextNext = false;
			}
		}
		typing = false;
	}


}
