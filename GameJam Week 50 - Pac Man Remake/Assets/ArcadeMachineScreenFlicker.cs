using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachineScreenFlicker : MonoBehaviour {

	SpriteRenderer sP;

	Color newColor;

	bool goingDown;
	float newA;

	public float speed = 1;

	// Use this for initialization
	void Start () {
		sP = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		if (goingDown)
		{
			newA = Mathf.Lerp(newColor.a, 0, speed * Time.deltaTime);
			if (newA < 0.2f){
				goingDown = false;

			}

		} else {

			newA = Mathf.Lerp(newColor.a, 1, speed * Time.deltaTime);
			if (newA > 0.8f)
            {
                goingDown = true;

            }
		}



		newColor = new Color(1, 1, 1, newA);
		sP.color = newColor;
	}
}
