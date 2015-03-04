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
		Vector3 vectorToPlayer = new Vector3 (player.transform.position.x - transform.position.x, GetComponent<Rigidbody>().velocity.y, player.transform.position.z - transform.position.z);
		Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;
		Vector3 finalVector = vectorToPlayer - currentVelocity;
		
		GetComponent<Rigidbody>().AddForce (finalVector * speed * Time.deltaTime);
	}

}
