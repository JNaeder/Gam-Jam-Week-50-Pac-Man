using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
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


	}



}
