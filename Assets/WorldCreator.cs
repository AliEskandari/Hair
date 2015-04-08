using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCreator : MonoBehaviour {

	enum Direction {North, East, South, West};

	private readonly float turn_probability = 0.3f;
	private readonly int levelsize = 15;
	private readonly string hallwayName = "StraightHallway";
	private readonly string leftCornerName = "LeftCorner";
	private readonly string rightCornerName = "RightCorner";
	private readonly string endpointName = "Endpoint";
	private readonly string startpointName = "Startpoint";

	public GameObject hallwayPrefab;
	public GameObject leftCornerPrefab;
	public GameObject rightCornerPrefab;

	private Queue<GameObject> roomsloaded;
	private GameObject newestRoom;

	private Direction lastStartDir = Direction.North;

	// Use this for initialization
	void Start () {

		roomsloaded = new Queue<GameObject> ();

		GameObject room = GenerateHallway (lastStartDir,new Vector3(0,0,0));
		roomsloaded.Enqueue(room);
		newestRoom = room;

		InvokeRepeating("Step", 2, 0.2F);
	}

	// Add one new room and delete the oldest room each time this is called
	void Step() {
		// Get information about most recent room
		Vector3 lastPos = newestRoom.transform.Find (endpointName).transform.position;
		Direction lastDir = getExitDir(newestRoom, lastStartDir);
		Debug.Log (lastStartDir + "->" + lastDir + " " + newestRoom.name);
		lastStartDir = lastDir;

		// Add a new room connected to most recent room
		GameObject newRoom = GenerateNext (lastPos, lastDir);
		roomsloaded.Enqueue(newRoom);
		newestRoom = newRoom;

		if (roomsloaded.Count > 7) {
			// Remove the old room
			GameObject oldRoom = roomsloaded.Dequeue ();
			Destroy (oldRoom);
		}
	}

	// Instantiate a new room that is positioned and rotated to attach to previous room
	// position = the position of the most recent room's endpoint
	// d = the direction of the most recent room's exit
	GameObject GenerateNext(Vector3 position, Direction d) {
		bool shouldturn = Random.value < turn_probability;
		
		if (shouldturn) {
			return GenerateCorner (d, position, Random.value < 0.5);
		} else {
			return GenerateHallway (d, position);
		}
	}
	
	// Generate a corner
	// startDir = the direction of the most recent room's exit
	// startPos = the position of the most recent room's endpoint
	// if turnleft == true -> the corner will turn to the left
	// if turnleft != true -> the corner will turn to the right
	GameObject GenerateCorner(Direction startDir, Vector3 startPos, bool turnleft) {
		GameObject prefab = turnleft ? leftCornerPrefab : rightCornerPrefab;
		GameObject corner = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		
		translateAndRotate (corner, startDir, startPos);
		
		return corner;
	}
	
	// Generate a hallway
	// startDir = the direction of the most recent room's exit
	// startPos = the position of the most recent room's endpoint
	GameObject GenerateHallway(Direction startDir, Vector3 startPos) {
		GameObject hallway = Instantiate(hallwayPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		
		translateAndRotate (hallway, startDir, startPos);
		
		return hallway;
	}
	
	// Move and rotate o so that it's startpoint is located at startpos and its direction is startDir
	void translateAndRotate(GameObject o, Direction startDir, Vector3 startPos) {
		int yrot = ((int) startDir) * 90;
		o.transform.rotation = Quaternion.Euler(0, yrot, 0);
		Vector3 translation = startPos - o.transform.Find (startpointName).transform.position;
		o.transform.position = translation;
	}

	// Returns the new direction after making a 90 degree turn starting from startDir
	// if turnleft == true -> turn 90 degrees to the left
	// if turnleft != true -> turn 90 degrees to the right
	Direction Turn (Direction startdir, bool turnleft) {
		if (startdir == Direction.North && turnleft) {
			return Direction.West;
		} else if (startdir == Direction.West && !turnleft) {
			return Direction.North;
		} else {
			int turnint = turnleft ? -1 : 1;
			return (Direction) startdir + turnint;
		}
	}

	// Find the exit direction of some room given start direction
	Direction getExitDir(GameObject o, Direction start) {
		Vector3 s = o.transform.Find (startpointName).transform.position;
		Vector3 e = o.transform.Find (endpointName).transform.position;
		float diffx = e.x - s.x;
		float diffz = e.z - s.z;
		
		if (o.name.Contains(hallwayName)) {
			Debug.Log ("Made it here");
			return start;
		} else {
			Debug.Log (o.name);
			return Turn (start, o.name.Contains(leftCornerName));
		}
	}

	// Find the exit direction of some room
	Direction getExitDir(GameObject o) {
		Vector3 s = o.transform.Find (startpointName).transform.position;
		Vector3 e = o.transform.Find (endpointName).transform.position;
		float diffx = e.x - s.x;
		float diffz = e.z - s.z;

		if (Mathf.Abs (diffx) < 0.01f) {
			if (diffz > 0) {
				return Direction.North;
			} else {
				return Direction.South;
			}
		} else if (Mathf.Abs (diffz) < 0.01f) {
			if (diffx > 0) {
				return Direction.East;
			} else {
				return Direction.West;
			}
		} else if (o.name.Contains(leftCornerName)) {
			if (diffx > 0 && diffz > 0) {
				return Direction.North;
			} else if (diffx > 0 && diffz < 0) {
				return Direction.East;
			} else if (diffx < 0 && diffz > 0) {
				return Direction.West;
			} else {
				return Direction.South;
			}
		} else {
			if (diffx > 0 && diffz > 0) {
				return Direction.East;
			} else if (diffx > 0 && diffz < 0) {
				return Direction.South;
			} else if (diffx < 0 && diffz > 0) {
				return Direction.North;
			} else {
				return Direction.West;
			}
		}
	}
}
