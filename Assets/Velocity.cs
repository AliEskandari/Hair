using UnityEngine;
using System.Collections;

public class Velocity : MonoBehaviour {

	private Vector3 prevPos;
	[HideInInspector]
	public Vector3 velocity;

	void Start ()
	{
		StartCoroutine( CalcVelocity() );
	}
	
	IEnumerator CalcVelocity()
	{
		while( Application.isPlaying )
		{
			// Position at frame start
			prevPos = transform.position;
			// Wait till it the end of the frame
			yield return new WaitForEndOfFrame();
			// Calculate velocity: Velocity = DeltaPosition / DeltaTime
			velocity = (transform.position - prevPos) / Time.deltaTime;
			Debug.Log( velocity );
		}
	}
}
