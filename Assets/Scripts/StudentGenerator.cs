using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StudentGenerator : MonoBehaviour {

	public GameObject student;
	public float numberOfStudents;
	public int count;
	public float width;
	
	private int level;
	private Random r;
	private List<GameObject> created;

	// Use this for initialization
	void Start () {

		r = new Random ();
		created = new List<GameObject> ();

		for (int i = 0; i < count; i++) {
			created.Add(getRandomPosGameObjectAtLevel(i));
		}
	}
	
	// Update is called once per frame
	void Update () {
	}


	void FixedUpdate () {
		if (transform.position.z > created [0].transform.position.z) {
			GameObject g = created[0];

			created.RemoveAt(0);
			Destroy(g, 1f);

			created.Add(getRandomPosGameObjectAtLevel(count - 1));
		}
	}


	GameObject getRandomPosGameObjectAtLevel (int level) {
		GameObject g = Instantiate (student);

		g.transform.position = new Vector3 
				((Random.Range(-1, 1) < 0 ? -1 : 1) * width,
				 student.transform.position.y,
				 transform.position.z + (level * numberOfStudents) +  Random.Range(1, numberOfStudents));

		return g;
	}

}
