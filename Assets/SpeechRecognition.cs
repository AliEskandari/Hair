using UnityEngine;
using System.Collections;

public class SpeechRecognition : MonoBehaviour {

	// Use this for initialization
	void Start () {

		System.Environment.SetEnvironmentVariable("DBname","hello", System.EnvironmentVariableTarget.User);

		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.UseShellExecute = false;
		startInfo.WorkingDirectory = "./SpeechRecognition/";
//		startInfo.RedirectStandardOutput = true;
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "/usr/local/bin/bash";
		startInfo.Arguments = "-c 'pocketsphinx_continuous -inmic yes -logfn \"log.txt\" -kws \"keyphrase.list\"'";
		process.StartInfo = startInfo;
		process.Start();
		// Do not wait for the child process to exit before
		// reading to the end of its redirected stream.
		// p.WaitForExit();
		// Read the output stream first and then wait.
//		string output = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
//		Debug.Log ("output: " + output);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
