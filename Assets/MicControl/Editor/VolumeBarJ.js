#pragma strict



@CustomEditor (MicControl)
class VolumeBarJ extends Editor  {

var ListenToMic = Selection.activeGameObject;
		
		
/////////////////////////////////////////////////////////////////////////////////////////////////		
		function OnInspectorGUI() {
var micInputValue=MicControl.loudness;
ProgressBar (micInputValue, "Loudness");


//show other variables

//Redirect 3D toggle
ListenToMic.GetComponent(MicControl).ThreeD=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).ThreeD,GUIContent ("3D sound","Should the streamed audio be a 3D sound? (Only enable this if you are using the controller to stream sound (VOIP) "));
//when 3D audio is enabled show the fall off settings
if(ListenToMic.GetComponent(MicControl).ThreeD){
ListenToMic.GetComponent(MicControl).VolumeFallOff = EditorGUILayout.FloatField(GUIContent ("Volume falloff","Set the rate at wich audio volume gets lowered. A lower value will have a slower falloff and thus hearable from a greater distance, while a higher value will make the audio degrate faster and dissapear from a shorter distance"), ListenToMic.GetComponent(MicControl).VolumeFallOff);
ListenToMic.GetComponent(MicControl).PanThreshold = EditorGUILayout.FloatField(GUIContent ("PanThreshold","Set the rate at wich audio PanThreshold gets switched between left or right ear. A lower value will have a faster transition and thus a faster switch, while a higher value will make the transition slower and smoothly switch between the ears. Don't go to smooth though as this will turn your audio to mono channel"), ListenToMic.GetComponent(MicControl).PanThreshold);
}


//Redirect select ingame
ListenToMic.GetComponent(MicControl).SelectIngame=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).SelectIngame,GUIContent ("Select in game","select the audio source through a GUI ingame"));

//Redirect Mute ingame
ListenToMic.GetComponent(MicControl).Mute=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).Mute,GUIContent ("Mute","when dissabled you can listen to a playback of the microphone"));

//Redirect debug ingame
ListenToMic.GetComponent(MicControl).debug=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).debug,GUIContent ("Debug","This will write the gathered Loudness value to the console during playmode. This is handy if you want if statements to listen at a specific value."));

//Redirect ShozDeviceName ingame
ListenToMic.GetComponent(MicControl).ShowDeviceName=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).ShowDeviceName,GUIContent ("Show Device name(s)","When selected all detected devices will be written to the console during play mode"));


EditorUtility.SetDirty(target);
		
	// Show default inspector property editor
	DrawDefaultInspector ();
	}

	
	
		// Custom GUILayout progress bar.
	function ProgressBar (value : float, label : String) {
		// Get a rect for the progress bar using the same margins as a textfield:
		var rect : Rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space ();
	}
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	