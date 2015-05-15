#pragma strict

import UnityEngine.UI;

private var micControl:MicControl;
public var playerController:PlayerController;

function Start () {
	micControl = GetComponent(MicControl);
}

function Update () {
	if (micControl.loudness > 50) {
		playerController.talked();
	}
}