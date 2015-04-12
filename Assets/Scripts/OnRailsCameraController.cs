using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnRailsCameraController : MonoBehaviour {

	private MyPath path;
	public GameObject railCamera;
	private bool pathcreated = false;
	private float distance;

	public WorldCreator worldCreator;

	private readonly float speed = 10;
	private readonly float amountToLookAhead = 3;

	private SortedDictionary<int, int> pathIndexToRoom;

	void Start () {
		distance = 0;
	}

	void FixedUpdate () {
		if (path == null) {
			return;
		}

		distance += speed * Time.deltaTime;

		if (distance > path.GetTotalDistance ()) {
			distance = 0;
		}


		Vector3 position = path.GetPositonAlongPath (distance);
		Vector3 ahead = path.GetPositonAlongPath (distance + amountToLookAhead);

		railCamera.transform.position = position;
		railCamera.transform.LookAt (ahead);
	}

	// Called when all rooms have been added to the scene
	// Given list of connected rooms in order
	public void RoomsAdded(List<GameObject> rooms) {
		List<Vector3> p = new List<Vector3> ();
		pathIndexToRoom = new SortedDictionary<int, int> ();

		if (rooms == null || rooms.Count == 0) {
			return;
		}

		for (int roomIndex = 0; roomIndex < rooms.Count; roomIndex++) {
			Vector3[] temp = worldCreator.GetPathPoints (rooms [roomIndex]);

			if (roomIndex == 0) {
				p.Add(temp[0]);
				pathIndexToRoom.Add (0, 0);
			}

			for (int pathIndex = 1; pathIndex < temp.Length; pathIndex++) {
				pathIndexToRoom.Add (p.Count, roomIndex);
				p.Add(temp[pathIndex]);
			}
		}

		path = new MyPath (p.ToArray());
	}

	// Returns the index of the room the camera is currently in
	public int GetRoomIndex() {
		if (path == null) {
			return 0;
		}
		int v;
		pathIndexToRoom.TryGetValue(path.GetIndexAtDistance (distance), out v);

		return v;
	}
}
