using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class HairController : MonoBehaviour {
	public GameObject HairCamera;
	public float hairRadius;
	public int numHairs;

	private GameObject[] hairObjects;
	private float hairAngle	= Mathf.PI;

	void Start () {
		hairObjects = new GameObject[numHairs];
		Object hairPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Hair/Hair.prefab", typeof(GameObject));

		float viewportHeight = getViewportHeight ();

		float baseScale = -1f;

		// Create and place each strand of hair.
		for (int i = 0; i < numHairs; i++) {
			float angle = (((float) i) / numHairs * hairAngle) - (hairAngle / 2);
			Vector3 p = HairCamera.gameObject.transform.position;
			Vector3 hairPosition = new Vector3(p.x + hairRadius * Mathf.Sin(angle), p.y/2, p.z + hairRadius * Mathf.Cos(angle));
			
			GameObject o = Instantiate(hairPrefab, hairPosition, Quaternion.identity) as GameObject;

			o.transform.parent = HairCamera.transform;

			if (baseScale < 0) {
				SkinnedMeshRenderer rend = o.transform.FindChild("Cone").GetComponent<SkinnedMeshRenderer>();
				float heightDifference = Mathf.Abs ((rend.bounds.max - rend.bounds.min).y);

				baseScale = viewportHeight / heightDifference;

				Debug.Log(baseScale);
				Debug.Log(viewportHeight);
				Debug.Log(heightDifference);
			}

			hairObjects[i] = o;
		}
	}
	
	// Get the difference (in world coordinates) between the top and the bottom of the camera frame at specified hair radius.
	float getViewportHeight() {
		Camera c = HairCamera.GetComponent<Camera> ();

		Vector3 start = c.ViewportToWorldPoint(new Vector3(0.5f, 1, hairRadius));
		Vector3 end = c.ViewportToWorldPoint(new Vector3(0.5f, 0, hairRadius));
		Vector3 result = start - end;
		Debug.Log (result);
		
		return Mathf.Abs(result.y);
	}

	// Get world coordinates for the top of the viewport at the specified hair radius
	float getViewportTop() {
		// (Another way to do this)
		// Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, hairRadius)).
		Camera c = HairCamera.GetComponent<Camera> ();
		
		Vector3 topMiddle = c.ViewportToWorldPoint(new Vector3(0.5f, 1, hairRadius));

		return topMiddle.y;
	}
}
