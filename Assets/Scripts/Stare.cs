using UnityEngine;
using System.Collections;

public class Stare : MonoBehaviour {

	private bool played;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform);
	}

	void FixedUpdate () {
		if (played != true && transform.position.z - Camera.main.transform.position.z < 3) {
			GetComponent<AudioSource>().Play();
			played = true;
		}
	}
}
