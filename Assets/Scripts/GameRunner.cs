using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameRunner : MonoBehaviour {
	public GameObject title;
	public GameObject cameraRig;
	public GameObject body;
	public OnRailsCameraController railsController;
	public GameObject scoreText;

	private GameObject[] hairs;

	public enum State {
		menu,
		transition,
		playing
	}

	private State gamestate;

	// Use this for initialization
	void Start () {
		gamestate = State.menu;
		hairs = GameObject.FindGameObjectsWithTag("Hair");

		foreach (GameObject hair in hairs) {
			hair.SetActive(false);
		}
	}

	private float startRotation;
	// Update is called once per frame
	void Update () {
		if (OVRManager.isHSWDisplayed) {
			return;
		} else if (gamestate == State.menu && !title.activeInHierarchy) {
			title.SetActive(true);

			foreach (GameObject hair in hairs) {
				hair.SetActive(true);
			}

			cameraRig.transform.LookAt (title.transform.position);
			return;
		}

		if (gamestate == State.menu && Input.anyKeyDown) {
			gamestate = State.transition;
			startRotation = 0;
		}

		if (gamestate == State.transition) {
			float deltaRotation = Time.deltaTime * 45f;
			startRotation += deltaRotation;
			body.transform.RotateAround(body.transform.position, body.transform.up, deltaRotation);

			if (startRotation >= 180) {
				scoreText.SetActive(true);
				gamestate = State.playing;
			}
		}
	}

	public State getGameState() {
		return gamestate;
	}
}
