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

	public float widthFull;
	public float heightFull;
	void Awake()
	{
		tmpText = GetComponent<TMP_Text>();


		
		
		
	}

	public void SkipForward()
    {
		ClearText();
		tmpText.text = txt;
		tmpText.maxVisibleCharacters = 100000;

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
		tmpText.maxVisibleCharacters = 100000;

		tmpText.text = txt;
		transform.parent.transform.position += new Vector3(1000, 0, 0);
		yield return new WaitForSeconds(0.0001f);
		transform.parent.transform.position += new Vector3(-1000, 0, 0);
		widthFull = tmpText.textBounds.size.x;
		heightFull = tmpText.textBounds.size.y;
		tmpText.maxVisibleCharacters = 0;
		foreach (char c in txt)
		{
			tmpText.maxVisibleCharacters += 1;
			yield return new WaitForSeconds(0.03f);
		}
		typing = false;
	}
	//IEnumerator PlayText()
	//{
	//	typing = true;
	//	foreach (char c in txt)
	//	{
	//		if (c.ToString() == "<")
	//		{
	//			richTextNext = true;
	//		}

	//		tmpText.text += c;



	//		if (!richTextNext)
	//			yield return new WaitForSeconds(0.04f);

	//		if (c.ToString() == ">")
	//		{
	//			richTextNext = false;
	//		}
	//	}
	//	typing = false;
	//}

}
