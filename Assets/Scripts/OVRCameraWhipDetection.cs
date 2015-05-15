using UnityEngine;
using System.Collections;
using System;

/*
 * Attach this script to an object that can be used to track the players head movements.
 * For OVR, the LeftEyeAnchor or RightEyeAnchor in the OVRCameraRig should be used.
 */

public class OVRCameraWhipDetection : MonoBehaviour {

	public float minUpTime = 0.08f;
	public float minUpAcc = 2.0f;
	public float maxDownTime = 0.4f;
	public float stopVel = 0.05f;
	public float minYVel = 0.0f;
	
	public bool whipped { get; set; }

	public PlayerController playerController;
	
	private float currUpTime;
	private float currDownTime;

	private bool detected;

	private Camera cam;
	private Vector3 camVelCopy;

	private float currYVel;
	private float currVelMag;
	private float prevVelMag;
	private float currAcc;

	public float deltaTime = 0.06f;
	private float timer;


	// Use this for initialization
	void Start () {

		cam 				= GetComponent<Camera> ();
		camVelCopy 			= new Vector3 ();
		 						
		currUpTime 			= 0f;
		currDownTime 		= 0f;

		currYVel 			= 0f;
		currVelMag 			= 0f;
					
		timer 				= 0.0f;

	}
	
	// Update is called once per frame, FixedDeltaTime = .02 seconds
	void FixedUpdate () {

		if (timer >= deltaTime) {

			camVelCopy.x 	= cam.velocity.x;
			camVelCopy.y 	= cam.velocity.z;
			
			currYVel 		= cam.velocity.y;
			currVelMag 		= camVelCopy.magnitude;
			currAcc 		= Mathf.Abs((currVelMag - prevVelMag) / deltaTime);
			prevVelMag 		= currVelMag;

			if (!detected) {
				StartCoroutine (WhipDetect (ResetWhipTimesCallback, SetWhipDetectedCallback));
			}

			timer = 0.0f;
		} 
		else {
			timer += Time.fixedDeltaTime;
		}
	}
	 
	void ResetWhipTimesCallback () {
		this.currUpTime = 0.0f;
		this.currDownTime = 0.0f;
	}

	void SetWhipDetectedCallback(bool detected) {
		this.detected = detected;
	}
	
	IEnumerator WhipDetect(Action resetTimes, Action<bool> setDetected) {

		// if current acceleration meets up acceleration req...
		if (currAcc >= minUpAcc && currYVel >= minYVel) {
			Debug.Log("Checkpoint 1");
			setDetected (true);

			// potential whip started, time the build up
			while (currAcc >= minUpAcc && currYVel >= minYVel) {
				currUpTime += Time.fixedDeltaTime;
				yield return null;
			}

			// if build is long enough...
			if (currUpTime >= minUpTime) {
				Debug.Log("Checkpoint 2");

				// time deacceleration to stopVel...
				while (currVelMag >= stopVel) {
					currDownTime += Time.fixedDeltaTime;
					yield return null;
				}

				// if deacceleration was fast enough...
				if (currDownTime <= maxDownTime) {
					Debug.Log("WHIPPED");
					playerController.whipped();
				}
			}
		}

		setDetected (false);
		resetTimes ();
	}

}
