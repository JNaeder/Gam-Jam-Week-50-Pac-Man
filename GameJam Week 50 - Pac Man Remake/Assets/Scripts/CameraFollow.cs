using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float speed;

	Vector3 diff;

	// Use this for initialization
	void Start () {
		diff = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			transform.position = Vector3.Lerp(transform.position, target.position + diff, speed * Time.deltaTime);
		}


	}
}
