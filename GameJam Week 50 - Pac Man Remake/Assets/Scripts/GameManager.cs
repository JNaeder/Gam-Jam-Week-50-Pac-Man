using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameManager : MonoBehaviour {


	public Text ammoNumber, scoreNumber;
	public Image laserCapacityGreen, laserButton, pelletGunButton;
	public Image life1, life2, life3;

    [FMODUnity.EventRef]
    public string getPelletSound, pacManDeathSound;

    FMOD.Studio.EventInstance pelletSoundInst;

	FMOD.Studio.EventInstance currentSnapShot;

	[FMODUnity.EventRef]
	public string[] checkpointSnapshots;

    public GameObject pacManPlayer;

    public float waitTimeTillNextLevel;
	float newTime = 999999999999;


	[HideInInspector]
	public  int ammo;

    public static int checkpointNum = -1;

	public static  int score;

	public static int lives = 3;


	public int pelletPoints;
	public int killEnemyPoints;

	public Color buttonHightlightColor;

    CheckpointManager cPM;
    Camera cam;
    CameraFollow camFollow;

	// Use this for initialization
	void Start () {
		laserButton.color = buttonHightlightColor;
        pelletGunButton.color = Color.white;

        cPM = FindObjectOfType<CheckpointManager>();
        cam = Camera.main;


		currentSnapShot = FMODUnity.RuntimeManager.CreateInstance(checkpointSnapshots[0]);
		currentSnapShot.start();
	}
	
	// Update is called once per frame
	void Update () {
		ammoNumber.text = ammo.ToString() + "/30";
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
			Debug.Log("Game Over");
		}
	}



	public void SetPelletScore(){
		score += pelletPoints;
        pelletSoundInst = FMODUnity.RuntimeManager.CreateInstance(getPelletSound);
        pelletSoundInst.setParameterValue("Checkpoint", checkpointNum);
        pelletSoundInst.start();
        pelletSoundInst.release();


	}

	public void SetEnemyScore(){
		score += killEnemyPoints;


	}


	public void UpdateLaserBar(float laserPerc){
		Vector3 laserBarScale = laserCapacityGreen.rectTransform.localScale;
		laserBarScale.x = laserPerc;
		laserCapacityGreen.rectTransform.localScale = laserBarScale;

	}


	public void SetActiveWeaponButton(bool usingLaser){
		if(usingLaser){
			laserButton.color = buttonHightlightColor;
			pelletGunButton.color = Color.white;
		} else {
			laserButton.color = Color.white;
			pelletGunButton.color = buttonHightlightColor;

		}

	}


	public void PlayerDeath(){
		lives--;
		newTime = Time.time;
        FMODUnity.RuntimeManager.PlayOneShot(pacManDeathSound);


	}

	void WaitToLoadNewScene(){

		if (Time.time > newTime + waitTimeTillNextLevel)
        {
            //Respawn Function
            newTime = Mathf.Infinity;
            Vector3 spawnPos = cPM.GetRespawnPos();
            GameObject newPacMan = Instantiate(pacManPlayer, spawnPos, Quaternion.identity);
            camFollow = cam.GetComponent<CameraFollow>();
            camFollow.target = newPacMan.transform;
            
            
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
}
