using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(MicControlC))]
public class VolumeBarC : Editor {
	
	private bool microphoneDeviceFound = false;
	
	public override void OnInspectorGUI () {
		if (!microphoneDeviceFound) {
			EditorGUILayout.HelpBox("No Microphone Device Was Found Connected To Your Computer.", MessageType.Warning);
			
			if (Microphone.devices.Length >= 1)
				microphoneDeviceFound = true;
		}
		
		MicControlC micCon  = (MicControlC) target;
		
		if (microphoneDeviceFound) {
			float micInputValue = micCon.loudness;    
			VolumeReader (micInputValue / 100, "Loudness " + micInputValue);  
			
			micCon.virual3D = EditorGUILayout.Toggle (new GUIContent("Enable Virtual 3D", "Uses a falloff system to simulate virtual 3D"), micCon.virual3D);
			micCon.volumeFallOff = EditorGUILayout.FloatField (new GUIContent("Volume Falloff", "Set the rate at wich audio volume gets lowered. A lower value will have a slower falloff and thus hearable from a greater distance, while a higher value will make the audio degrate faster and dissapear from a shorter distance"), micCon.volumeFallOff);
			micCon.GuiSelectDevice = EditorGUILayout.Toggle (new GUIContent("Gui Selection", "Select the microphone ingame using a GUI menu"), micCon.GuiSelectDevice);
			EditorGUI.BeginChangeCheck();
			micCon.ableToHearMic = EditorGUILayout.Toggle (new GUIContent("Audio Mute", "Select whether you can hear yourself talking or not"), micCon.ableToHearMic);
			if (EditorGUI.EndChangeCheck()) {
				micCon.GetComponent<AudioSource>().mute = micCon.ableToHearMic;
			}
			
			micCon.sensitivity = EditorGUILayout.FloatField(new GUIContent("Mic Sensitivity", "The sensitivity that the audio is recieved from the microphone"), micCon.sensitivity);
			//micCon.ramFlushSpeed = EditorGUILayout.FloatField(new GUIContent("Ram Flush Speed", "The interval time between when the microphone audioclip is reset"), micCon.ramFlushSpeed);
			micCon.sourceVolume = EditorGUILayout.FloatField(new GUIContent("Volume", "Volume of the audio that comes out of audiosouce, basically the volume of the microphone"), micCon.sourceVolume);
			micCon.micControl = (MicControlC.micActivation)EditorGUILayout.EnumPopup(new GUIContent("Mic Control Type", "Changes the type of method for using the microphone"), micCon.micControl);
			
			EditorUtility.SetDirty(target);
		}
	}
	
	void VolumeReader (float value, string label) {
		EditorGUILayout.Space ();
		Rect vRect = GUILayoutUtility.GetRect (18, 18, "TextField");      
		EditorGUI.ProgressBar (vRect, value, label);
		EditorGUILayout.Space ();
	}
}