#pragma strict

@script RequireComponent (AudioSource);
//if true a menu will apear ingame with all the microphones
@HideInInspector
var SelectIngame:boolean=false;
//if false the below will override and set the mic selected in the editor
 //Select the microphone you want to use (supported up to 6 to choose from). If the device has number 1 in the console, you should select default as it is the first defice to be found.
enum Devices {DefaultDevice, Second, Third, Fourth, Fifth, Sixth}

@HideInInspector
var ThreeD:boolean=false;
@HideInInspector
var VolumeFallOff:float=1;
@HideInInspector
var PanThreshold:float=1;

var InputDevice : Devices;
private var selectedDevice:String;
var audioListener:Transform;
var audioSource:AudioSource;
//The maximum amount of sample data that gets loaded in, best is to leave it on 256, unless you know what you are doing. A higher number gives more accuracy but 
//lowers performance allot, it is best to leave it at 256.
var amountSamples:float=256;
static var loudness:float;
var sensitivity:float=0.4;
var sourceVolume:float=100;
private var minFreq: int;
 var maxFreq: int=44100;
 
@HideInInspector
var Mute:boolean=true;
@HideInInspector
var debug:boolean=false;
@HideInInspector
var ShowDeviceName:boolean=false;
private var micSelected:boolean=false; 

private var recording:boolean=true; 

private var ListenerDistance:float;
private var ListenerPosition:Vector3;

private var focused:boolean=false;
private var Initialised:boolean=false;

function Start() {
    // Request permission to use both webcam and microphone.
    if (Application.isWebPlayer) {
        yield Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone)) {
            InitMic();
            Initialised = true;
        } else {
            return;
        }
    } else {
        InitMic();
        Initialised = true;
    }
}
//apply the mic input data stream to a float;
function Update() {
    //pause everything when not focused on the app and then re-initialize.
    if (!focused) {
        StopMicrophone();
        Initialised = false;
    }
    if (!Application.isPlaying) {
        //don't stop the microphone if you are clicking inside the editor
        StopMicrophone();
        Initialised = false;
    } else {
        if (!Initialised) {
            InitMic();
            Initialised = true;
        }
    }
    if (Microphone.IsRecording(selectedDevice)) {
        loudness = GetDataStream() * sensitivity * (sourceVolume / 10);
    }
    if (debug) {
        Debug.Log(loudness);
    }
    //the source volume
    if (sourceVolume > 100) {
        sourceVolume = 100;
    }
    if (sourceVolume < 0) {
        sourceVolume = 0;
    }
    //when 3D is enabled adjust the volume based on distance.
    if (ThreeD) {
        ListenerDistance = Vector3.Distance(transform.position, audioListener.position);
        ListenerPosition = audioListener.InverseTransformPoint(transform.position);
        GetComponent. < AudioSource > ().volume = (sourceVolume / 100 / (ListenerDistance * VolumeFallOff));
        GetComponent. < AudioSource > ().panStereo = (ListenerPosition.x / PanThreshold);
    } else {
        GetComponent. < AudioSource > ().volume = (sourceVolume / 100);
    }
}

function GetDataStream() {
    if (Microphone.IsRecording(selectedDevice)) {
        var dataStream: float[] = new float[amountSamples];
        var audioValue: float = 0;
        GetComponent. < AudioSource > ().GetOutputData(dataStream, 0);
        for (var i in dataStream) {
            audioValue += Mathf.Abs(i);
        }
        return audioValue / amountSamples;
    }
}
//select device ingame
function OnGUI() {
    if (SelectIngame == true) {
        if (Microphone.devices.Length > 0 && micSelected == false) //If there is more than one device, choose one.
            for (var i: int = 0; i < Microphone.devices.Length; ++i)
                if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), Microphone.devices[i].ToString())) {
                    StopMicrophone();
                    selectedDevice = Microphone.devices[i].ToString();
                    GetMicCaps();
                    StartMicrophone();
                    micSelected = true;
                }
        if (Microphone.devices.Length < 1 && micSelected == false) { //If there is only 1 decive make it default
            selectedDevice = Microphone.devices[0].ToString();
            GetMicCaps();
            micSelected = true;
        }
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Initialize microphone
private
function InitMic() {
    //select audio source
    if (!audioSource) {
        audioSource = transform.GetComponent(AudioSource);
    }
    //only Initialize microphone if a device is detected
    if (Microphone.devices.Length >= 0) {
        var i = 0;
        //count amount of devices connected
        for (device in Microphone.devices) {
            i++;
            if (ShowDeviceName) {
                Debug.Log("Devices number " + i + " Name" + "=" + device);
            }
        }
        if (SelectIngame == false) {
            //select the device if possible else give error
            if (InputDevice == Devices.DefaultDevice) {
                if (i >= 1) {
                    selectedDevice = Microphone.devices[0];
                } else {
                    Debug.LogError("No device detected on this slot. Check input connection");
                }
            }
            if (InputDevice == Devices.Second) {
                if (i >= 2) {
                    selectedDevice = Microphone.devices[1];
                } else {
                    Debug.LogError("No device detected on this slot. Check input connection");
                }
            }
            if (InputDevice == Devices.Third) {
                if (i >= 3) {
                    selectedDevice = Microphone.devices[2];
                } else {
                    Debug.LogError("No device detected on this slot. Check input connection");
                    return;
                }
            }
            if (InputDevice == Devices.Fourth) {
                if (i >= 4) {
                    selectedDevice = Microphone.devices[2];
                } else {
                    Debug.LogError("No device detected on this slot. Check input connection");
                    return;
                }
            }
            if (InputDevice == Devices.Fifth) {
                if (i >= 5) {
                    selectedDevice = Microphone.devices[2];
                } else {
                    Debug.LogError("No device detected on this slot. Check input connection");
                    return;
                }
            }
            if (InputDevice == Devices.Sixth) {
                if (i >= 6) {
                    selectedDevice = Microphone.devices[2];
                } else {
                    Debug.LogError("No device detected on this slot. Check input connection");
                    return;
                }
            }
        }
        //detect the selected microphone
        GetComponent. < AudioSource > ().clip = Microphone.Start(selectedDevice, true, 10, maxFreq);
        //loop the playing of the recording so it will be realtime
        GetComponent. < AudioSource > ().loop = true;
        //if you only need the data stream values  check Mute, if you want to hear yourself ingame don't check Mute. 
        GetComponent. < AudioSource > ().mute = Mute;
        //don't do anything until the microphone started up
        while (!(Microphone.GetPosition(selectedDevice) > 0)) {
            if (debug) {
                Debug.Log("Awaiting connection");
            }
        }
        if (debug) {
            Debug.Log("Connected");
        }
        //Put the clip on play so the data stream gets ingame on realtime
        GetComponent. < AudioSource > ().Play();
        recording = true;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
//for the above control the mic start or stop
public
function StartMicrophone() {
    GetComponent. < AudioSource > ().clip = Microphone.Start(selectedDevice, true, 10, maxFreq); //Starts recording
    while (!(Microphone.GetPosition(selectedDevice) > 0)) {} // Wait until the recording has started
    GetComponent. < AudioSource > ().Play(); // Play the audio source!
}
public
function StopMicrophone() {
    GetComponent. < AudioSource > ().Stop(); //Stops the audio
    Microphone.End(selectedDevice); //Stops the recording of the device  
}

function GetMicCaps() {
    Microphone.GetDeviceCaps(selectedDevice, minFreq, maxFreq); //Gets the frequency of the device
    if ((minFreq + maxFreq) == 0) //These 2 lines of code are mainly for windows computers
        maxFreq = 44100;
}
//Create a gui button in another script that calls to this script
public
function MicDeviceGUI(left: float, top: float, width: float, height: float, buttonSpaceTop: float, buttonSpaceLeft: float) {
    if (Microphone.devices.Length > 1 && micSelected == false) //If there is more than one device, choose one.
        for (var i: int = 0; i < Microphone.devices.Length; ++i)
            if (GUI.Button(new Rect(left + (buttonSpaceLeft * i), top + (buttonSpaceTop * i), width, height), Microphone.devices[i].ToString())) {
                StopMicrophone();
                selectedDevice = Microphone.devices[i].ToString();
                GetMicCaps();
                StartMicrophone();
                micSelected = true;
            }
    if (Microphone.devices.Length < 2 && micSelected == false) { //If there is only 1 decive make it default
        selectedDevice = Microphone.devices[0].ToString();
        GetMicCaps();
        micSelected = true;
    }
}
//flush the date through a custom created audio clip, this controls the data flow of that clip
// Creates a 1 sec long audioclip, with a 440hz sinoid
private
var position: int = 0;
private
var sampleRate: int = 0;
private
var frequency: float = 440;

function OnAudioRead(data: float[]) {
    for (var count = 0; count < data.Length; count++) {
        data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / sampleRate));
        position++;
    }
}

function OnAudioSetPosition(newPosition: int) {
    position = newPosition;
}
//start or stop the script from running when the state is paused or not.
function OnApplicationFocus(focus: boolean) {
    focused = focus;
}

function OnApplicationPause(focus: boolean) {
    focused = focus;
}