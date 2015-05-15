using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StudentGenerator : MonoBehaviour {

	public GameObject student;
	private float[] studentLocations;
	private Dictionary<float, GameObject> created;
	private Dictionary<int, List<int>> studentRooms;
	private int numStudents = 150;
	private MyPath path;
	public GameObject studentsObject;

	// Use this for initialization
	void Start () {
		created = new Dictionary<float, GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
	}


	void FixedUpdate () {
	}

	public void addStudents (MyPath mypath, SortedDictionary<int, int> pathToRoomIndex) {
		path = mypath;

		float distance = path.GetTotalDistance ();
		float spacing = distance / numStudents;
		studentLocations = new float[numStudents];
		studentRooms = new Dictionary<int, List<int>> ();
		for (int studentIndex = 0; studentIndex < numStudents; studentIndex ++) {
			float studentDist = 9f + studentIndex * distance * 0.9f / numStudents;
			studentLocations[studentIndex] = studentDist;
			int room = pathToRoomIndex[path.GetIndexAtDistance(studentDist)];
			if (!studentRooms.ContainsKey(room)) {
				studentRooms.Add (room, new List<int>());
			}

			//Debug.Log("Adding student " + studentIndex + " to room " + room + " Studentdist: " + studentDist + " totalDist: " + distance );
			studentRooms[room].Add(studentIndex);
		}
	}

	public void changeStudentVisibility(int roomIndex, bool shouldDisplay) {
		if (!studentRooms.ContainsKey(roomIndex)) {
			return;
		}

		foreach (int studentIndex in studentRooms[roomIndex]) {
			if (shouldDisplay) {
				float loc = studentLocations[studentIndex];
				if (!created.ContainsKey(loc)) {
					Vector3 position = getStudentPosition(studentIndex);
					GameObject s = Instantiate(student, position, Quaternion.identity) as GameObject;
					s.transform.parent = studentsObject.transform;
					created.Add(loc, s);
				}
			} else {
				float loc = studentLocations[studentIndex];
				if (created.ContainsKey(loc)) {
					Destroy(created[loc]);
					created.Remove(loc);
				}
			}
		}
	}

	private Vector3 getStudentPosition(int studentIndex) {
		float loc = studentLocations[studentIndex];
		int pathIndex = path.GetIndexAtDistance(loc);
		Vector3 start = path.GetPoint (pathIndex);
		Vector3 end = path.GetPoint (pathIndex + 1);

		Vector3 studentPos = path.GetPositonAlongPath (loc);

		float dist = 0.5f + Random.value;
		if (Random.value > 0.5) {
			dist = -1 * dist;
		}

		Vector3 diff = end - start;
		Debug.Log ("Pos: " + studentPos);
		
		if (diff.x == 0) {
			studentPos.x = studentPos.x + dist;
		} else if (diff.z == 0) {
			studentPos.z = studentPos.z + dist;
		} else {
			float direction = diff.x / diff.z;
			
			studentPos = new Vector3 (studentPos.x + (1 / direction) * dist, studentPos.y, studentPos.z + (direction) * dist);
		}

		return studentPos;
	}

}
