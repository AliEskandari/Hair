﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCreator : MonoBehaviour {

	enum Direction {North, East, South, West};

	private readonly float turn_probability = 0.3f;
	private readonly int levelsize = 80;
	private readonly int numdisplayed = 10;

	private List<GameObject> hallwayPrefabs;
	private List<GameObject> leftCornerPrefabs;
	private List<GameObject> rightCornerPrefabs;

	private List<GameObject> rooms;

	public OnRailsCameraController cameraController;
	
	private readonly string hallwayTag = "Hallway";
	private readonly string leftCornerTag = "Left Corner";
	private readonly string rightCornerTag = "Right Corner";

	
	private Direction lastStartDir = Direction.North;

	// Use this for initialization
	void Start () {
		rooms = new List<GameObject> ();
		hallwayPrefabs = new List<GameObject>();
		leftCornerPrefabs = new List<GameObject>();
		rightCornerPrefabs = new List<GameObject>();

		foreach (Object o in Resources.LoadAll("Rooms")) {
			GameObject b = o as GameObject;
			if (isHallway(b)) {
				hallwayPrefabs.Add(b);
			} else if (isLeftCorner(b)) {
				leftCornerPrefabs.Add(b);
			} else if (isRightCorner(b)) {
				rightCornerPrefabs.Add(b);
			}
		}

		PopulateRooms ();
	}

	void Update() {

		// Deactivate rooms not in view
		int index = cameraController.GetRoomIndex ();
		int min = index - 1;
		int max = index + numdisplayed - 2;
		
		for (int i = 0; i < rooms.Count; i++) {
			bool shouldDisplay = (i >= min && i <= max);
			rooms[i].SetActive(shouldDisplay);
		}
	}

	// Add one new room and delete the oldest room each time this is called
	void PopulateRooms() {
		for (int i = 0; i < levelsize; i++) {
			// Add the first hallway
			if (i == 0) {
				rooms.Add (GenerateHallway (lastStartDir, new Vector3 (0, 0, 0)));
				continue;
			}

			// Get information about the previous room added
			GameObject last = rooms [rooms.Count - 1];
			Vector3 lastPos = GetEndPoint (last);
			Direction lastDir = getExitDir (last, lastStartDir);
			lastStartDir = lastDir;

			GameObject room = GenerateNext (lastDir, lastPos);

			if (i > numdisplayed) {
				room.SetActive (false);
			}

			rooms.Add (room);
		}

		cameraController.RoomsAdded (rooms);
	}

	// Instantiate a new room that is positioned and rotated to attach to previous room
	// position = the position of the most recent room's endpoint
	// d = the direction of the most recent room's exit
	GameObject GenerateNext(Direction d, Vector3 position) {
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
		GameObject prefab;
		if (turnleft) {
			int index = (int) Mathf.Floor (Random.value * leftCornerPrefabs.Count);
			prefab = leftCornerPrefabs[index];
		} else {
			int index = (int) Mathf.Floor (Random.value * rightCornerPrefabs.Count);
			prefab = rightCornerPrefabs[index];
		}

		GameObject corner = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		
		translateAndRotate (corner, startDir, startPos);
		
		return corner;
	}
	
	// Generate a hallway
	// startDir = the direction of the most recent room's exit
	// startPos = the position of the most recent room's endpoint
	GameObject GenerateHallway(Direction startDir, Vector3 startPos) {
		int index = (int) Mathf.Floor (Random.value * hallwayPrefabs.Count);
		GameObject prefab = hallwayPrefabs[index];

		GameObject hallway = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		
		translateAndRotate (hallway, startDir, startPos);
		
		return hallway;
	}
	
	// Move and rotate o so that it's startpoint is located at startpos and its direction is startDir
	void translateAndRotate(GameObject o, Direction startDir, Vector3 startPos) {
		int yrot = ((int) startDir) * 90;
		o.transform.rotation = Quaternion.Euler(0, yrot, 0);
		Vector3 translation = startPos - GetStartPoint(o);
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
		Vector3 s = GetStartPoint(o);
		Vector3 e = GetEndPoint(o);
		float diffx = e.x - s.x;
		float diffz = e.z - s.z;
		
		if (isHallway(o)) {
			return start;
		} else {
			return Turn (start, isLeftCorner(o));
		}
	}
	
	// Returns the starting position of g's path
	public Vector3 GetStartPoint(GameObject g) {
		Vector3[] pathPoints = GetPathPoints (g);
		return pathPoints [0];
	}

	// Returns the final position of g's path
	public Vector3 GetEndPoint(GameObject g) {
		Vector3[] pathPoints = GetPathPoints (g);
		return pathPoints [pathPoints.Length - 1];
	}

	// Returns array of g's path positions (in order).
	public Vector3[] GetPathPoints(GameObject g) {
		Transform p = g.transform.FindChild ("Path");
		int numChildren = p.childCount;

		Vector3[] pathPoints = new Vector3[numChildren];
		for (int i = 0; i < numChildren; i++) {
			Transform child = p.GetChild(i);
			pathPoints[i] = child.position;
		}

		return pathPoints;
	}

	private bool isHallway(GameObject o) {
		return (o.tag == hallwayTag);
	}

	private bool isLeftCorner(GameObject o) {
		return (o.tag == leftCornerTag);
	}

	private bool isRightCorner(GameObject o) {
		return (o.tag == rightCornerTag);
	}

}
