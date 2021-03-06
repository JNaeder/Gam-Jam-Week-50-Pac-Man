﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    Transform[] checkpointPos;
    Checkpoint[] checkpointScripts;

	// Use this for initialization
	void Start () {
        checkpointScripts = GetComponentsInChildren<Checkpoint>();
        checkpointPos = new Transform[checkpointScripts.Length];
        for (int i = 0; i < checkpointScripts.Length; i++) {
            checkpointPos[i] = checkpointScripts[i].transform;
            //Debug.Log(checkpointPos[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public Vector3 GetRespawnPos()
    {
        if (GameManager.checkpointNum < 20)
        {
            Vector3 newSpawnPos = checkpointPos[GameManager.checkpointNum].position;
            return newSpawnPos;

        }
        else
        {
            return Vector3.zero;
        }
    }
}
