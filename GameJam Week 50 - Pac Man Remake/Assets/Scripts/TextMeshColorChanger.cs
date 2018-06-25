using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshColorChanger : MonoBehaviour {

    TextMeshPro text;
    Color textColor;

    float hueValue;
    public float colorSpeedChange;

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMeshPro>();
	}
	
	// Update is called once per frame
	void Update () {
        if(hueValue > 1) {
            hueValue = 0;
        }
        else
        {
            hueValue += colorSpeedChange * Time.deltaTime * 0.1f;
        }
        textColor = Color.HSVToRGB(hueValue, 1, 1);
        text.color = textColor;
    }
}
