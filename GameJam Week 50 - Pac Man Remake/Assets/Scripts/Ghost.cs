using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	
	public float health = 5f;
	public float shield = 5f;
	public float speed = 1f;
	public float fireRate = 1f;
	public Transform healthBarGreen, shieldBarBlue, muzzle;
    public ParticleSystem pS;
	public ParticleSystem explosion;
	public GameObject enemyPellet;
	public bool isFollowPlayer, isTurret, isGate;
	public Transform wayPointFolder;

	SpriteRenderer sP, muzzleSP;
	Color ghostColor;

	[HideInInspector]
	public bool isAttacking;

	float startHealthNum, healthBarPerc, startHealthBarGreenX;
	float startShieldNum, shieldBarPerc, startShieldBarBlueX;
	float newFireTime;
	float waypointDist;

	int waypointNum;
    
	Vector3 healthBarScale, shieldBarScale;
   
	Transform[] wayPoints, wayPointFolderPoints;
    
	PacMan_Controller pacMan;
	GameManager gM;

	bool pSIsPlaying;
	bool isBlue;
	bool isBeingAttacked;

	// Use this for initialization
	void Start () {
		startHealthNum = health;
		healthBarScale = healthBarGreen.localScale;
		startHealthBarGreenX = healthBarScale.x;
		startShieldNum = shield;
		shieldBarScale = shieldBarBlue.localScale;
		startShieldBarBlueX = shieldBarScale.x;

		gM = FindObjectOfType<GameManager>();
		sP = GetComponent<SpriteRenderer>();
		if (muzzle != null)
		{
			muzzleSP = muzzle.GetComponentInParent<SpriteRenderer>();
		}
		ghostColor = sP.color;
		pacMan = FindObjectOfType<PacMan_Controller>();


		if(wayPointFolder != null){
			wayPointFolderPoints = wayPointFolder.GetComponentsInChildren<Transform>();
			wayPoints = new Transform[wayPointFolderPoints.Length - 1];
			for (int i = 0; i < wayPoints.Length; i++){
				wayPoints[i] = wayPointFolderPoints[i + 1];
			}



		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBars();      
		ShieldRegenerate();
		CheckHealth();

		if (isFollowPlayer)
		{
			AttackingFollowPlayer();
		}
		if(isTurret){
			AttackTurret();
		}
		if(isGate){
			FollowWayPoints();
		}


	}


	void UpdateBars(){
		healthBarPerc = health / startHealthNum;
        healthBarScale.x = startHealthBarGreenX * healthBarPerc;
        healthBarGreen.localScale = healthBarScale;

		shieldBarPerc = shield / startShieldNum;
		shieldBarScale.x = startShieldBarBlueX * shieldBarPerc;
		shieldBarBlue.localScale = shieldBarScale;

	}


	void ShieldRegenerate(){
		if (!isBeingAttacked)
		{
			if (shield > 0 && shield < startShieldNum)
			{
				shield += Time.deltaTime * 0.75f;
			}
		}

	}


	public void DoDamage(float damage){
		//Debug.Log("Doing Damage To Ghost!");
		isBeingAttacked = true;
		if (isBlue)
		{

			health -= Time.deltaTime * damage * 0.5f;


		} else {
			shield -= Time.deltaTime;
            
		}



		if(!pSIsPlaying){
            pS.Play();
			pSIsPlaying = true;
		}

	}


	public void DoPelletDamage(float damage){
		if(!isBlue){
			shield -= (damage * 0.25f);
		} else {
			health -= damage;
		}

	}


	public Vector3 GetPosition(){
		return transform.position;

	}


	public void UnLocked(){
		//Debug.Log("Stop Particles!");
		pSIsPlaying = false;
		isBeingAttacked = false;
		if (pS != null)
		{
			pS.Stop();
		}

	}


	void CheckHealth(){
		if(shield <= 0){
			isBlue = true;
			sP.color = Color.blue;
			if (muzzle != null)
			{
				muzzleSP.color = Color.blue;
			}
		}
		if (health <= 0)
        {
            Death();
        }


	}


	void Death(){
		GameObject explosionInst = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
		ParticleSystem.MainModule explosionPS = explosionInst.GetComponent<ParticleSystem>().main;
		explosionPS.startColor = Color.blue;
		Destroy(explosionInst, 2f);
		gM.SetEnemyScore();
		Destroy(gameObject);

	}

	void AttackingFollowPlayer(){

		if(isAttacking){
			if (pacMan != null)
			{
				if (isFollowPlayer)
				{
					//transform.LookAt(pacMan.transform);
					transform.position = Vector3.MoveTowards(transform.position, pacMan.transform.position, speed * Time.deltaTime);
				}
			}
		}

	}


	void AttackTurret(){
		if (isAttacking)
		{
			if (pacMan != null)
			{
				Vector3 diff = pacMan.transform.position - transform.position;
				diff.Normalize();
				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 190);


				if (Time.time > newFireTime + (1 / fireRate))
				{
					newFireTime = Time.time;
					Instantiate(enemyPellet, muzzle.position, muzzle.rotation);

				}
			}
		}

	}


	void FollowWayPoints(){
		waypointDist = Vector3.Distance(transform.position, wayPoints[waypointNum].position);

		//Debug.Log(waypointDist);
		transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointNum].position, speed * Time.deltaTime);

		if(waypointDist < 1f){
			waypointNum++;
			if(waypointNum > wayPoints.Length -1){
				waypointNum = 0;
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
