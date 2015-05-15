--------------------------
C# Version - By djfunkey
----------------------------------------------------------------------------------------------------------------------------------
WARNING THIS VERSION IS NOt UP TO DATE AND CAN BE UNSTABLE, FOR THE MOST STABLE BUILD USE THE Javascript version.
This C# version is held up to date by the community and it is up to them to integrate the new features of the Javascript version.
----------------------------------------------------------------------------------------------------------------------------------
//This C# documentation has a simplefied layout, Detailed information can be found in the javascript readme file: All example code can also be found there.
//The reason for this is that both script work in exactly the same way. Calling variables and functions is possible with both script, altough the java version 
//is more up to date and can also be called in C#, the only advantage in this script is that it has a push to talk function, so it could be used for ingame voice-chat.
//The javascript version does not have voice chat as it is the source code and the idea is just to give the user his microhpone input data. Voice chat and other functions
//are up to the developer to create them, as that is one of the reasons one would download this script.

~~ Functionality Overview ~~

1. This script allows you to call information from a selected microphone.
2. The script can detect every microphone attached.
3. There is a GUI function to easily view the GUI selection screen on the microphones.
4. The script finds the volume of sound going through ONLY the microphone.
5. The volume of the AudioSource directly affects the loudness variable.
6. If only 1 microphone is connected, that device is made default.
7. Option between push to talk and voice activated.
8. Can call the input loudness from outside the script.
9. Create a GUI from any script with a build in gui function.
10. Select use mode: hold to speak, push to talk, continious talk



-------------------------------
Variables & voids explanation
-------------------------------

~~ Variables ~~

public string selectedDevice -
This is just the Microphone device name, of the microphone you selected.

public int amountSamples -
This controls how many samples of data the loudness variable is averaged out by.
It’s hard to explain, don’t change it from 256 unless you know otherwise.

public float sensitivity -
This controls how sensitive the Loudness variable is.

public float loudness - 
Don’t modify this, it is an output variable, it determines how loud your speaking into the microphone.
*You can use this to alert enemies if the player speaks to loud into the mirophone.*

public float sourceVolume -
This is the same as the volume on the AudioSource.

public bool buttonToSpeak -
Toggle this to determine whether the player needs to push a button to talk or not.

// Private Variables \\

private int minFreq, maxFreq -
These are both output variables, which give you the minimum and maximum frequency your microphone is capable of.

private bool micSelected -
This determines whether a microphone has been selected yet.

// Voids \\

public void MicDeviceGUI (float left, float top, float width, float height, float buttonSpaceTop, float buttonSpaceLeft) - 
This lets you create a GUI element which you can use to give you a selection of the different microphones.
Both buttonSpaceTop and buttonSpaceLeft need to be, the left or top, add the space you want between each button.
*E.g. MicControl.MicDeviceGUI(400, 100, 300, 100, 110, 0);*

public void StartMicrophone () -
This starts the listening of audio from your microphone

public void StopMicrophone () - 
This stops the playing of audio from your microphone



//Call variables and functions. All these functions can be called from external C# scripts.

//Main function.
MicControl.loudness;
[This call's the volume data from your microphone and converts it to the loudness value.]

//Sub functions.
MicControl.StartMicrophone();
[This will force the microphone to start recording when called from an external script or a custom mod script that detects microphones.]

MicControl.StopMicrophone();
[This will force stop the microphone from anything recording/listening. This is usefull when your player dies and is not alowed to use the MicControl functions anymore.]



//This script will force shut down the selected microphone on start of the level and then proceeds to start it up again, this can be used on a respawn.

function Start(){
//stop current mic
MicControl.StopMicrophone();
//start fresh input.
MicControl.StartMicrophone();

}

-----------------------
Thank You For Reading
-----------------------

http://forum.unity3d.com/members/118660-djfunkey