using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

	bool isPaused;
    PacMan_Controller pacMan;

    public GameObject pauseScreen;

	// Use this for initialization
	void Start () {
		pacMan = FindObjectOfType<PacMan_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {


            isPaused = !isPaused;
        }



        if (isPaused)
        {
            Time.timeScale = 0;
            //Debug.Log("Pause");
            pacMan.isPaused = true;
            //pacMan.enabled = false;
            pauseScreen.SetActive(true);

        }
        else
        {
            Time.timeScale = 1;
            //Debug.Log("Unpause");
            if (pacMan != null)
            {
                pacMan.isPaused = false;
            }

            //pacMan.enabled = true;
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(false);
            }
        }
	}


	public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
       // Debug.Log("Unpause");
        pacMan.isPaused = false;
        //pacMan.enabled = true;
        pauseScreen.SetActive(false);


    }
}
