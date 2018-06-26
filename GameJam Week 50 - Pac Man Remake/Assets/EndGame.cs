using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	public GameObject[] ghosts;
	int amountAlive;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		amountAlive = 0;
		foreach(GameObject g in ghosts){
			if(g != null){
				amountAlive++;

			}

		}
		if(amountAlive == 0){
			Debug.Log("Win!!");

		}

	}
}
