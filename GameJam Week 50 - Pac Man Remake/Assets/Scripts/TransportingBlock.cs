using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportingBlock : MonoBehaviour {

	public Transform transporterTrans;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player"){
			Vector3 newPos = transporterTrans.position;
			collision.transform.position = newPos;


		}
	}
}
