using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {


	public Text scoreNumber;
	public Image laserCapacityGreen;
	public Image life1, life2, life3;

    [FMODUnity.EventRef]
    public string getPelletSound, pacManDeathSound;

	FMOD.Studio.EventInstance currentSnapShot;

	[FMODUnity.EventRef]
	public string[] checkpointSnapshots;

    public GameObject pacManPlayer;

    public float waitTimeTillNextLevel;
	float newTime = 999999999999;



	public GameObject winScreen, loseScreen;


	[HideInInspector]
	public  int ammo;

    public static int checkpointNum = -1;

	public static  int score;

	public static int lives = 3;

    public static PacMan_Controller pacMan;

	public int pelletPoints;
	public int killEnemyPoints;

	public Color buttonHightlightColor;


    int PelletScaleNum = 0;

    CheckpointManager cPM;
    Camera cam;
    CameraFollow camFollow;
	EventSystem eS;

	// Use this for initialization
	void Start () {

        cPM = FindObjectOfType<CheckpointManager>();
        cam = Camera.main;
		eS = FindObjectOfType<EventSystem>();

		currentSnapShot = FMODUnity.RuntimeManager.CreateInstance(checkpointSnapshots[0]);
		currentSnapShot.start();

        pacMan = FindObjectOfType<PacMan_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
		scoreNumber.text = score.ToString();
		WaitToLoadNewScene();

		if(lives == 3){
			life1.enabled = true;
			life2.enabled = true;
			life3.enabled = true;
		} else if(lives == 2){
			life1.enabled = true;
            life2.enabled = true;
            life3.enabled = false;
		} else if(lives == 1){
			life1.enabled = true;
            life2.enabled = false;
            life3.enabled = false;
		} else if(lives == 0){
			life1.enabled = false;
            life2.enabled = false;
            life3.enabled = false;
			
		}
	}



	public void SetPelletScore(){
		score += pelletPoints;
        FMODUnity.RuntimeManager.PlayOneShot(getPelletSound, transform.position);


	}
	public void SetCherryScore()
    {
		score += 1000;
        FMODUnity.RuntimeManager.PlayOneShot(getPelletSound, transform.position);


    }

	public void SetEnemyScore(){
		score += killEnemyPoints;


	}


	public void UpdateLaserBar(float laserPerc){
		Vector3 laserBarScale = laserCapacityGreen.rectTransform.localScale;
		laserBarScale.x = laserPerc;
		laserCapacityGreen.rectTransform.localScale = laserBarScale;

	}





	public void PlayerDeath(){
		lives--;
        if (lives <= 0) {
           checkpointNum = 10;
			LoseScreen();
        }
		newTime = Time.time;
        FMODUnity.RuntimeManager.PlayOneShot(pacManDeathSound);


	}

	void WaitToLoadNewScene(){

		if (Time.time > newTime + waitTimeTillNextLevel)
        {
            //Respawn Function
            newTime = Mathf.Infinity;
            if (lives > 0)
            {
                Vector3 spawnPos = cPM.GetRespawnPos();
                GameObject newPacMan = Instantiate(pacManPlayer, spawnPos, Quaternion.identity);
                pacMan = newPacMan.GetComponent<PacMan_Controller>();
                camFollow = cam.GetComponent<CameraFollow>();
                camFollow.target = newPacMan.transform;
            }
            else {
                Debug.LogWarning("Game Over!");

            }
            
            
        }
	}



	public void ChangeFMODSnapShot(int checkpointNum){
		//Debug.Log("Checkpoint Number is " + checkpointNum);
		if (checkpointNum <= checkpointSnapshots.Length -1)
		{
			currentSnapShot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			currentSnapShot = FMODUnity.RuntimeManager.CreateInstance(checkpointSnapshots[checkpointNum]);
			currentSnapShot.start();
		}



	}


	public void WinGame(){
		winScreen.SetActive(true);
		pacMan.isPaused = true;
	}

	public void LoseScreen(){

		loseScreen.SetActive(true);
		pacMan.isPaused = true;
	}
}
