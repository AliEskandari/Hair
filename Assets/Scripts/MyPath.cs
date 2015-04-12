using UnityEngine;
using System.Collections;

public class MyPath {
	private Vector3[] path;
	private float[] distances;
	private float totalDistance = 0;

	public MyPath(Vector3[] p) {
		if (p == null || p.Length <= 1) {
			return;
		}

		path = p;
		InitPathLength ();
	}

	// Return the interpolated position at distance d along this path object
	// distance should be from [0, totalDistance]
	public Vector3 GetPositonAlongPath(float distance) {
		if (distance > distances [distances.Length - 1]) {
			return path[path.Length - 1];
		}

		int index = GetIndexAtDistance (distance);

		float segDist = distances [index + 1] - distances [index];
		float percent = (distance - distances [index]) / segDist;

		Vector3 s = path [index];
		Vector3 e = path [index + 1];

		float x = (e.x - s.x) * percent + s.x;
		float y = (e.y - s.y) * percent + s.y;
		float z = (e.z - s.z) * percent + s.z;

		return new Vector3 (x, y, z);
	}

	// Returns the total length of the path
	public float GetTotalDistance() {
		return totalDistance;
	}

	// Returns the start index of the path segment containing the given distance
	public int GetIndexAtDistance(float distance) {
		if (distance < 0.001) {
			return 0;
		}

		int min = 0;
		int max = distances.Length;
		while (min <=max)
		{
			int mid = (min + max) / 2;
			if (distance < distances[mid] && distance > distances[mid - 1]) {
				return mid - 1;
			} else if (distance < distances[mid]) {
				max = mid - 1;
			} else {
				min = mid + 1;
			}
		}

		return -1;
	}

	private void InitPathLength() {
		distances = new float[path.Length];

		Vector3 curr = path[0];
		float length = 0;
		distances [0] = length;
		for (int i = 1; i < path.Length; i++) {
			distances[i] = distances[i - 1] + Vector3.Distance(curr, path[i]);
			curr = path[i];
		}

		totalDistance = distances[distances.Length - 1];
	}
}
