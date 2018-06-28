using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {
    MusicScript mS;


	// Use this for initialization
	void Start () {
        mS = FindObjectOfType<MusicScript>();
	}
	
	// Update is called once per frame
	void Update () {



		
	}


	public void StartGame(){
		SceneManager.LoadScene(1);

	}


	public void Quit(){
		Application.Quit();

	}

	public void BackToMainMenu(){
		SceneManager.LoadScene(0);
        mS.StopMusic();

	}



}
