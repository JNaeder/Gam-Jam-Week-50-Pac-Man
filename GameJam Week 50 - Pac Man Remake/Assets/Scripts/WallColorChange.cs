using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallColorChange : MonoBehaviour {
    Tilemap outlineGrid;
    Color outlineColor;
    
    public float hueValue;
    public float colorSpeedChange;

    // Use this for initialization
    void Start() {
        outlineGrid = GetComponent<Tilemap>();

    }

    // Update is called once per frame
    void Update() {
        ChangeColor();
    }

    void ChangeColor() {
        if (hueValue > 1) {
            hueValue = 0;
        }
        else
        {
            hueValue += colorSpeedChange * Time.deltaTime * 0.1f;
        }
        outlineColor = Color.HSVToRGB(hueValue, 1, 1);
        outlineGrid.color = outlineColor;

    }
}
