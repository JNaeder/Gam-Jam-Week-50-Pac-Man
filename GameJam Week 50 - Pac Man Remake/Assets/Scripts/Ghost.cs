using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	
	public float health = 5f;
	public float speed = 1f;
	public Transform healthBarGreen;
    public ParticleSystem pS;
	public ParticleSystem explosion;

	SpriteRenderer sP;
	Color ghostColor;

	[HideInInspector]
	public bool isAttacking;

	float startHealthNum;
	float healthBarPerc;
	float startHealthBarGreenX;

	Vector3 healthBarScale;

	PacMan_Controller pacMan;

	bool pSIsPlaying;

	// Use this for initialization
	void Start () {
		startHealthNum = health;
		healthBarScale = healthBarGreen.localScale;
		startHealthBarGreenX = healthBarScale.x;

		sP = GetComponent<SpriteRenderer>();
		ghostColor = sP.color;
		pacMan = FindObjectOfType<PacMan_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
		healthBarPerc = health / startHealthNum;
		healthBarScale.x = startHealthBarGreenX * healthBarPerc;
		healthBarGreen.localScale = healthBarScale;

		Attacking();
	}

	public void DoDamage(){
		//Debug.Log("Doing Damage To Ghost!");
		health -= Time.deltaTime;

		if(health <= 0){
			Death();

		}



		if(!pSIsPlaying){
            pS.Play();
			pSIsPlaying = true;
		}

	}

	public Vector3 GetPosition(){
		return transform.position;

	}


	public void UnLocked(){
		//Debug.Log("Stop Particles!");
		pSIsPlaying = false;
		if (pS != null)
		{
			pS.Stop();
		}

	}


	void Death(){
		GameObject explosionInst = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
		ParticleSystem.MainModule explosionPS = explosionInst.GetComponent<ParticleSystem>().main;
		explosionPS.startColor = ghostColor;
		Destroy(explosionInst, 2f);
		Destroy(gameObject);

	}

	void Attacking(){

		if(isAttacking){
			if (pacMan != null)
			{
				//transform.LookAt(pacMan.transform);
				transform.position = Vector3.MoveTowards(transform.position, pacMan.transform.position, speed * Time.deltaTime);
			}
		}

	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Player"){
			pacMan.Death();
			UnLocked();
		}
	}
}
