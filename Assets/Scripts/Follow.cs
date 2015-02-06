using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public GameObject player;
	public float speed;
	
	// Use this for initialization
	void Start () {

	}

	void FixedUpdate() 
	{
		rigidbody.velocity = new Vector3 (player.transform.position.x - transform.position.x, 0.0f, player.transform.position.z - transform.position.z) * speed * Time.deltaTime;
	}

}
