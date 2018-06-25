using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeSpriteRend : MonoBehaviour {

    SpriteRenderer sP;
    Color spriteColor;

    float hueValue;
    public float colorSpeedChange;
    public bool isStaticColor, isChangingColor;
    public Color staticColor;

    // Use this for initialization
    void Start () {
        sP = GetComponent<SpriteRenderer>();
        if (isStaticColor) {
            sP.color = staticColor;

        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isChangingColor)
        {
            if (hueValue > 1)
            {
                hueValue = 0;
            }
            else
            {
                hueValue += colorSpeedChange * Time.deltaTime * 0.1f;
            }
            spriteColor = Color.HSVToRGB(hueValue, 1, 1);
            sP.color = spriteColor;
        }
    }
}
