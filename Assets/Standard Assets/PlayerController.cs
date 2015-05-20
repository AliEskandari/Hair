using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Text coolText;
	public Text annoyingText;

	private int cool = 0;
	private int annoying = 0;
	
	// Use this for initialization
	void Start () {
		coolText.text = "cool=" + cool.ToString();
		annoyingText.text = "annoying=" + annoying.ToString();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void whipped(){
		cool++;
		coolText.text = "cool=" + cool.ToString ();
	}

	public void talked() {
		annoying++;
		annoyingText.text = "annoying=" + annoying.ToString ();
	}
}
