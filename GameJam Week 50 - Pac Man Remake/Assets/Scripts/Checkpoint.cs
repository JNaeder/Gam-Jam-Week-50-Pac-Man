using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    bool hasTriggered;
    CheckpointManager cPM;
	GameManager gM;

	public int checkpointNum;

	// Use this for initialization
	void Start () {
        cPM = GetComponentInParent<CheckpointManager>();
		gM = FindObjectOfType<GameManager>();

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Player")
        {
            if (!hasTriggered)
            {
				GameManager.checkpointNum = checkpointNum;
                hasTriggered = true;
				gM.ChangeFMODSnapShot(checkpointNum);
            }
        }
    }
}
