using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoFadeUI : MonoBehaviour
{
    Image image;
    float alpha = 7;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= Time.deltaTime;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}
