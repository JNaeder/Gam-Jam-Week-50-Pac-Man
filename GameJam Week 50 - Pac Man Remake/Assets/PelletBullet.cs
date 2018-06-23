using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBullet : MonoBehaviour {

	public bool playerPellet, enemyPellet;

	public float speed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(Vector3.up * Time.deltaTime * speed);
		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log("Pellet Hit Something");
		if (playerPellet)
		{
			if (collision.gameObject.tag == "Ghost")
			{
				Ghost ghost = collision.gameObject.GetComponent<Ghost>();
				ghost.DoPelletDamage(1);


			}
		} else if(enemyPellet){

			if (collision.gameObject.tag == "Player")
            {
				PacMan_Controller pacMan = collision.gameObject.GetComponent<PacMan_Controller>();
				pacMan.Death();
                


            }

		}


		Destroy(gameObject);
	}
}
