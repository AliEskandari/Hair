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
		Vector3 vectorToPlayer = new Vector3 (player.transform.position.x - transform.position.x, rigidbody.velocity.y, player.transform.position.z - transform.position.z);
		Vector3 currentVelocity = rigidbody.velocity;
		Vector3 finalVector = vectorToPlayer - currentVelocity;
		
		rigidbody.AddForce (finalVector * speed * Time.deltaTime);
	}

}
