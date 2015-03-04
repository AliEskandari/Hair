using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public Rigidbody projectile;
	public float speed;

	void Update() 
	{
		if (Input.GetButtonUp ("Fire1")) {
			Rigidbody clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection(new Vector3 (0, 0, speed));

			Destroy(clone.gameObject, 5);
			GetComponent<AudioSource>().Play();
		}

	}

}
