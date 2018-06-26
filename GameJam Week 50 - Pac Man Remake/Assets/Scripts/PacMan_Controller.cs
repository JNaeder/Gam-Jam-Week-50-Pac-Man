using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan_Controller : MonoBehaviour {


	public float speed = 1f;
    [Range(0,1)]
	public float diagSpeedMult = 0.5f;
	public float ghostDistThreshold = 10f;
	public GameObject explosion, laserEnd, pelletBullet;
	public LayerMask layerMask;
	public int  ammoCapacity;
	public float laserCapacity;

    [FMODUnity.EventRef]
    public string pacMoveSound, pacShootSound, fireLaserSound;

    FMOD.Studio.EventInstance pacMoveInst, fireLaserInst;
    float isMovingNum, isFiringLaserNum;

	float moveThreshold = 0f;
	float startSpeed;

	float startLaserCapacity, laserPerc;


	float lockedGhostDist;

    //[HideInInspector]
    public bool laserEnabled;

	bool hasLockedOnToGhost;
	bool isUsingLaser = true;
	bool isUsingLaserEnd;
	bool lasershooting;

	Transform graphicTransform;
	Quaternion transRot;
	LineRenderer lineRend;
    Ghost newGhost;
    Brick newBrick;
	GameManager gM;
    Animator anim;
    SpriteRenderer sP;
	GameObject laserEndGO;
	// Use this for initialization
	void Start () {
        pacMoveInst = FMODUnity.RuntimeManager.CreateInstance(pacMoveSound);
        isMovingNum = 0;
        pacMoveInst.start();

        fireLaserInst = FMODUnity.RuntimeManager.CreateInstance(fireLaserSound);
        isFiringLaserNum = 0;
        fireLaserInst.start();


		graphicTransform = GetComponentInChildren<SpriteRenderer>().transform;
		transRot = graphicTransform.rotation;

		lineRend = GetComponentInChildren<LineRenderer>();
		gM = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        sP = GetComponentInChildren<SpriteRenderer>();

		startSpeed = speed;
		startLaserCapacity = laserCapacity;
	}
	
	// Update is called once per frame
	void Update () {
        pacMoveInst.setParameterValue("isMoving", isMovingNum);
        fireLaserInst.setParameterValue("isShootingLaser", isFiringLaserNum);
        anim.SetBool("isShootingLaser", lasershooting);

		Movement();
		WeaponSwitching();
		UpdateBars();

		if (isUsingLaser)
		{
            if (laserEnabled)
            {
                ShootingLaser();
            }
		}
		else
		{
			ShootingPellet();
		}
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "PacDots"){
            //Debug.Log("Got Dot!");
            gM.SetPelletScore();
			Destroy(collision.gameObject);

		}
	}




	void Movement(){
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

        anim.SetFloat("speed", (Mathf.Abs(h)) + (Mathf.Abs(v)));

		transform.position += new Vector3(h, v, 0) * Time.deltaTime * speed;

        if (h == 0 && v == 0)
        {
            isMovingNum = 0;
            //Debug.Log("Still");
        }
        else {
            isMovingNum = 1;
            //Debug.Log("Moving");
        }



        //Rotating
		if(h > moveThreshold && v == 0){
            sP.flipY = false;
            sP.flipX = false;
            transRot.eulerAngles = new Vector3(0, 0, 270);
			speed = startSpeed;
			// Right
		} else if(h > moveThreshold  && v > moveThreshold){
            sP.flipY = false;
            sP.flipX = false;
            transRot.eulerAngles = new Vector3(0, 0, 315);
			speed = startSpeed * diagSpeedMult;
			// Up and Right
		} else if(h == 0 && v > moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 0);
			speed = startSpeed;
			// Up
		} else if(h < -moveThreshold  && v > moveThreshold){
           // sP.flipY = true;
            sP.flipX = true;
            transRot.eulerAngles = new Vector3(0, 0, 45);
			speed = startSpeed * diagSpeedMult;
			// Up and Left
		} else if(h < -moveThreshold  && v == 0){
            //sP.flipY = true;
            sP.flipX = true;
            transRot.eulerAngles = new Vector3(0, 0, 90);
			speed = startSpeed;
			// Left
		} else if(h < -moveThreshold  && v < -moveThreshold){
           /// sP.flipY = true;
           sP.flipX = true;
            transRot.eulerAngles = new Vector3(0, 0, 135);
			speed = startSpeed * diagSpeedMult;
			// Left and Down
		} else if(h == 0 && v < -moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 180);
			speed = startSpeed;
			// Down
		} else if (h > moveThreshold  && v < -moveThreshold){
            sP.flipY = false;
            sP.flipX = false;
            transRot.eulerAngles = new Vector3(0, 0, 225);
			speed = startSpeed * diagSpeedMult;
			// Down and Right
		}
		graphicTransform.rotation = transRot;




	}



	void ShootingLaser(){
		if(Input.GetButtonDown("Fire1")){
			lasershooting = true;
		}

		if (Input.GetButton("Fire1"))
		{
			if (lasershooting){
				if (laserCapacity > 0)
				{
					lineRend.enabled = true;
                    
					laserCapacity -= Time.deltaTime;

					if (!isUsingLaserEnd)
					{
						laserEndGO = Instantiate(laserEnd, graphicTransform.position, graphicTransform.rotation);
						laserEndGO.transform.parent = graphicTransform;
						isUsingLaserEnd = true;
					}



					RaycastHit2D hit = Physics2D.Raycast(graphicTransform.position, graphicTransform.TransformDirection(Vector3.up), Mathf.Infinity, layerMask);
					if (hit != null)
					{
						if (hit.collider.gameObject.tag == "GhostTrigger" || hit.collider.gameObject.tag == "Brick")
						{
                            //Debug.Log("Ghost! " + hit.collider.transform.position);
                            isFiringLaserNum = 2;
                            lineRend.useWorldSpace = true;

							newGhost = hit.collider.gameObject.GetComponentInParent<Ghost>();
                            if (newGhost != null)
                            {
                                newGhost.DoDamage(1);
                            }
                            newBrick = hit.collider.gameObject.GetComponent<Brick>();
                            if (newBrick != null) {
                                newBrick.DoDamage(1);
                            }

							lineRend.SetPosition(0, graphicTransform.position);
							lineRend.SetPosition(1, hit.collider.transform.position);
							ParticleSystem laserEndPS = laserEndGO.GetComponent<ParticleSystem>();
							laserEndGO.transform.position = hit.collider.transform.position;
							if (laserEndPS.isPlaying == false)
							{
								laserEndPS.Play();
							}
							hasLockedOnToGhost = true;


						}
						else if (hasLockedOnToGhost == true)
						{
							if (newGhost != null)
							{
                                isFiringLaserNum = 2;
                                if (newGhost != null)
                                {
                                    newGhost.DoDamage(1);
                                }
                                if (newBrick != null)
                                {
                                    newBrick.DoDamage(1);
                                }
                                Vector3 ghostPos = newGhost.GetPosition();
								lockedGhostDist = Vector3.Distance(graphicTransform.position, ghostPos);
								lineRend.SetPosition(1, ghostPos);
								ParticleSystem laserEndPS = laserEndGO.GetComponent<ParticleSystem>();
								laserEndGO.transform.position = ghostPos;
								if (laserEndPS.isPlaying == false)
								{
									laserEndPS.Play();
								}
							}
							else
							{

								hasLockedOnToGhost = false;
							}

							lineRend.useWorldSpace = true;

							lineRend.SetPosition(0, graphicTransform.position);


							//Debug.Log("Distance to locked ghost is " + lockedGhostDist);
							if (lockedGhostDist > ghostDistThreshold)
							{
								hasLockedOnToGhost = false;
								newGhost.UnLocked();
							}
						}
						else
						{
                            isFiringLaserNum = 1;
                            lineRend.useWorldSpace = false;
							lineRend.SetPosition(0, Vector3.zero);
							lineRend.SetPosition(1, new Vector3(0, 5, 0));
							ParticleSystem laserEndPS = laserEndGO.GetComponent<ParticleSystem>();
							laserEndGO.transform.localPosition = new Vector3(0, 5, 0);
							if (laserEndPS.isPlaying == false)
							{
								laserEndPS.Play();
							}
						}
					}


				}
				else
				{
					lasershooting = false;
                    isFiringLaserNum = 0;
                    if (laserCapacity < startLaserCapacity)
					{
						laserCapacity += Time.deltaTime * 2;
					}
					hasLockedOnToGhost = false;
					lineRend.enabled = false;
					if (laserEndGO != null)
					{
						isUsingLaserEnd = false;
						Destroy(laserEndGO);
					}
				}
		}
		} 
		else {
			lasershooting = false;
            isFiringLaserNum = 0;
            if (laserCapacity < startLaserCapacity)
			{
				laserCapacity += Time.deltaTime * 2;
			}
			hasLockedOnToGhost = false;
			lineRend.enabled = false;
			if (laserEndGO != null)
			{
				isUsingLaserEnd = false;
				Destroy(laserEndGO);
			}
            
		}

        
		if(Input.GetButtonUp("Fire1")){
			if (newGhost != null)
            {
                newGhost.UnLocked();
                isFiringLaserNum = 0;
            }

		}

	}



	void ShootingPellet(){
		lasershooting = false;
		if (laserCapacity < startLaserCapacity)
        {
            laserCapacity += Time.deltaTime * 2;
        }
		hasLockedOnToGhost = false;
        lineRend.enabled = false;
        if (laserEndGO != null)
        {
            isUsingLaserEnd = false;
            Destroy(laserEndGO);
        }
		if (newGhost != null)
		{
			newGhost.UnLocked();
		}

		if(Input.GetButtonDown("Fire1")){
            
			//Debug.Log("Fire Pellet!");
			if (gM.ammo > 0)
			{
				Instantiate(pelletBullet, graphicTransform.position, graphicTransform.rotation);
                FMODUnity.RuntimeManager.PlayOneShot(pacShootSound, transform.position);
                gM.ammo--;
			}
		}


	}


	void WeaponSwitching(){
		if(Input.GetButtonDown("WeaponSwitch")){
			//Debug.Log("Switch Weapon");

			isUsingLaser = !isUsingLaser;
			gM.SetActiveWeaponButton(isUsingLaser);
		}


	}

	void UpdateBars(){
		laserPerc = laserCapacity / startLaserCapacity;
		gM.UpdateLaserBar(laserPerc);

	}





	public void Death(){
        pacMoveInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fireLaserInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        GameObject explos = Instantiate(explosion, transform.position, Quaternion.identity);
		ParticleSystem.MainModule explosPS = explos.GetComponent<ParticleSystem>().main;
		explosPS.startColor = Color.yellow;
		Destroy(gameObject);
		Destroy(explos, 2f);
		gM.PlayerDeath();


	}
}
