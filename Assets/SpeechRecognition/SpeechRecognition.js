#pragma strict

import UnityEngine.UI;

private var micControl:MicControl;
public var playerController:PlayerController;
private var talked:boolean=false;

function Start () {
	micControl = GetComponent(MicControl);
}

function Update () {
	if (micControl.loudness > 50) {
		if (!talked)  {
			playerController.talked();
			talked = true;
		}
	} else {
		talked = false;
	}
}