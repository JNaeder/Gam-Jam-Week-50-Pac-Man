using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttackTrigger : MonoBehaviour {


	Ghost ghost;

	// Use this for initialization
	void Start () {
		ghost = GetComponentInParent<Ghost>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			ghost.isAttacking = true;
			Destroy(gameObject);
		}
	}
}
