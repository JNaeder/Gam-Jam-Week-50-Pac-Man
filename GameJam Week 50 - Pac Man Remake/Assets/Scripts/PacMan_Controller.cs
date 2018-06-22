using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan_Controller : MonoBehaviour {


	public float speed = 1f;
    [Range(0,1)]
	public float diagSpeedMult = 0.5f;
	public float ghostDistThreshold = 10f;
	public GameObject explosion, laserEnd;
	public LayerMask layerMask;

	float moveThreshold = 0f;
	float startSpeed;

	float lockedGhostDist;

	bool hasLockedOnToGhost;

	Transform graphicTransform;
	Quaternion transRot;
	LineRenderer lineRend;
	Ghost newGhost;

	// Use this for initialization
	void Start () {
		graphicTransform = GetComponentInChildren<SpriteRenderer>().transform;
		transRot = graphicTransform.rotation;

		lineRend = GetComponentInChildren<LineRenderer>();

		startSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
		Shooting();
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "PacDots"){
			Debug.Log("Got Dot!");
			Destroy(collision.gameObject);

		}
	}




	void Movement(){
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		transform.position += new Vector3(h, v, 0) * Time.deltaTime * speed;





        //Rotating
		if(h > moveThreshold && v == 0){
			transRot.eulerAngles = new Vector3(0, 0, 270);
			speed = startSpeed;
			// Right
		} else if(h > moveThreshold  && v > moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 315);
			speed = startSpeed * diagSpeedMult;
			// Up and Right
		} else if(h == 0 && v > moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 0);
			speed = startSpeed;
			// Up
		} else if(h < -moveThreshold  && v > moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 45);
			speed = startSpeed * diagSpeedMult;
			// Up and Left
		} else if(h < -moveThreshold  && v == 0){
			transRot.eulerAngles = new Vector3(0, 0, 90);
			speed = startSpeed;
			// Left
		} else if(h < -moveThreshold  && v < -moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 135);
			speed = startSpeed * diagSpeedMult;
			// Left and Down
		} else if(h == 0 && v < -moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 180);
			speed = startSpeed;
			// Down
		} else if (h > moveThreshold  && v < -moveThreshold){
			transRot.eulerAngles = new Vector3(0, 0, 225);
			speed = startSpeed * diagSpeedMult;
			// Down and Right
		}
		graphicTransform.rotation = transRot;




	}



	void Shooting(){

		if(Input.GetButton("Fire1")){
			lineRend.enabled = true;


			RaycastHit2D hit = Physics2D.Raycast(graphicTransform.position, graphicTransform.TransformDirection(Vector3.up), Mathf.Infinity,layerMask);
			if (hit != null)
			{
				if (hit.collider.gameObject.tag == "GhostTrigger")
				{
					//Debug.Log("Ghost! " + hit.collider.transform.position);

					lineRend.useWorldSpace = true;

					newGhost = hit.collider.gameObject.GetComponentInParent<Ghost>();
					newGhost.DoDamage();

					lineRend.SetPosition(0, graphicTransform.position);
					lineRend.SetPosition(1, hit.collider.transform.position);
					hasLockedOnToGhost = true;


				}
				else if (hasLockedOnToGhost == true)
				{
					if (newGhost != null)
					{
						newGhost.DoDamage();
						Vector3 ghostPos = newGhost.GetPosition();
						lockedGhostDist = Vector3.Distance(graphicTransform.position,ghostPos);
						lineRend.SetPosition(1, ghostPos);                  
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

					lineRend.useWorldSpace = false;
					lineRend.SetPosition(0, Vector3.zero);
					lineRend.SetPosition(1, new Vector3(0, 5, 0));
				}
			}
			



		}
		else {
			
			hasLockedOnToGhost = false;
			lineRend.enabled = false;
		}


		if(Input.GetButtonUp("Fire1")){
			if (newGhost != null)
            {
                newGhost.UnLocked();
            }

		}

	}





	public void Death(){

		GameObject explos = Instantiate(explosion, transform.position, Quaternion.identity);
		ParticleSystem.MainModule explosPS = explos.GetComponent<ParticleSystem>().main;
		explosPS.startColor = Color.yellow;
		Destroy(gameObject);
		Destroy(explos, 2f);

	}
}
