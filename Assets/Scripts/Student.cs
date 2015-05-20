using UnityEngine;
using System.Collections;

public class Student : MonoBehaviour {
	private bool active = false;

	public readonly float TRIGGER_DIST = 5;
	private AudioSource audio;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();

		int i = 2 + (int)(Random.value * 9);
		audio.clip = Resources.Load("StudentVoices/" + i) as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - Camera.main.transform.position).sqrMagnitude < (TRIGGER_DIST * TRIGGER_DIST)) {
			Activate ();
		} else {
			Deactivate();
		}
	}

	void Activate () {
		if (active) {
			return;
		}

		audio.Play ();

		active = true;
	}

	void Deactivate () {
		if (!active) {
			return;
		}
		
		active = false;
	}

	void Speak () {

	}
}
