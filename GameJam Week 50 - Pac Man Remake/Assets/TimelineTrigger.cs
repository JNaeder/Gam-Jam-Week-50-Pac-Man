using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour {

    public  PlayableDirector dir;

    Collider2D coll;

	// Use this for initialization
	void Start () {
        coll = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            dir.Play();
            Destroy(coll);

        }
    }
}
