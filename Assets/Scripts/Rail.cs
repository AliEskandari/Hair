using UnityEngine;
using System.Collections;

public class Rail : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		SetTransformZ (transform.position.z + (1.0f * speed * Time.deltaTime));
	}

	void SetTransformX(float n)
	{
		transform.position = new Vector3(n, transform.position.y, transform.position.z);
	}
	
	void SetTransformZ(float n)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, n);
	}
}
