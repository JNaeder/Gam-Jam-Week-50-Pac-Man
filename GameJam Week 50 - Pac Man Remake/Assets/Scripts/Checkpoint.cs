using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    bool hasTriggered;
    CheckpointManager cPM;

	// Use this for initialization
	void Start () {
        cPM = GetComponentInParent<CheckpointManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {if (collision.gameObject.tag == "Player")
        {
            if (!hasTriggered)
            {

                cPM.UpdateCheckpoint();
                GameManager.checkpointNum++;
               // Debug.Log("Hit Checkpoint " + GameManager.checkpointNum + " " + gameObject.name);
                hasTriggered = true;
            }
        }
    }
}
