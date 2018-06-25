using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicScript : MonoBehaviour {

    [FMODUnity.EventRef]
    public string musicSound;

    FMOD.Studio.EventInstance musicInst;

    public static MusicScript musicScriptInst;


    private void Awake()
    {
        if (musicScriptInst != null)
        {
            Destroy(gameObject);
        }
        else {
            musicScriptInst = this;
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);

        musicInst = FMODUnity.RuntimeManager.CreateInstance(musicSound);
        musicInst.start();
	}
	
	// Update is called once per frame
	void Update () {
        musicInst.setParameterValue("Checkpoint", GameManager.checkpointNum);
	}
}
