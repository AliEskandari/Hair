# Hair

Hair is a video game and experience for the Oculus Rift. As a teenage girl, you play within your own dream where others admire your beautiful hair. As you walk down your highschool hallway, students shower you with compliments. Your job is to thank each admirer. For each admirer you must whip your hair and verbally respond to their complement with specific phrases such as "Thank you" or "I love your shoes". These verbal responses are verified with in-game speech recognition.

## Components

### Speech Recognition

Initially, we worked on obtaining an in-game speech recognition service using Carnegie Mellon's open-sourced speech recognition package, CMUSphinx. Using PocketSphinx, a lightweight version of CMU's larger, robust speech recognition package, we would be able to the service on and off at the appropriate times during gameplay. Integrating an external C-based library into Unity became an issue and we chose to integrate a temporary, less sophisticated alternative. Using MicControl, from the Unity Asset Store, we are able to turn microphone input into a float value that represents the player's vocal intensity. This allows us to determine when the player has given a verbal response in the game (determining what they said is to be implemented later).

Related scripts:
* SpeechRecognition.cs
* MicControl.cs 

### Head Whip Detection

To determine when the play whipped their head, a script uses one of the Oculus cameras to detect whip motions. When a player meets the whip detection requirements the game updates its UI.

Related scripts:
* OVRHeadWhipDetection.cs

### Hair design

The hair basically acts as a ragdoll body hanging in front of the player. The hair strand mesh was created in Blender. We added bones to the mesh and used Blenders auto weighting to assign weights to each section of the hair. In Unity, we use the Dynamic Bone asset from the Unity Asset store to move the hair realistically with the player.

To do:
* Texture the hair.
* Make more varied hair meshes.
* Position the hair in a more realistic way too look like an actual head of hair.
* Reduce the hair’s jitteriness when the player moves.

### World creator

The WorldCreator.cs script builds a randomly generated path of hallways from the assorted hallway prefabs in the Resources/Rooms directory. These rooms may overlap with one another, but players will never see this overlapping, since only the closest 6 rooms are set as active. This script is very modifiable, and any room prefabs placed in the Rooms directory will work properly, as long as they follow this contract:
* Room prefab must have a gameobject called “Path” with a list of points inside.
* The Room prefab must be tagged with either “LeftCorner”, “RightCorner”, or “StraightHallway”
* Rules for the points in "Path"
* Points must all have the transform component
* Points must be in order from start...end
* First point must be placed at the room's entrance, with centered width and height
* Last point must be placed at the room's exit, with centered width and height

The OnRailsController.cs script actually moves the players through these rooms along a path created by adding each path point from the contract above to a list. This controller also turns the player’s body when they reach a corner.

To Do:
* Add new room models.

Related Scripts:
* WorldCreator.cs
* OnRailsController.cs
* MyPath.cs

### Students

The StudentGenerator.cs script places student objects along the path created in WorldCreator.cs. Each student gameobject contains the Student.cs script that is triggered when the player is close. Right now, the script causes the student to glow and say a random sentence (“I love your hair”).

To Do:
* Only trigger one cool/annoying point per student and only trigger those points when the player is in the vicinity of the student.
* Add more student models.
* Add more student voices.
* Fix bug where students sometimes do not appear along long stretches of hallway.

Related Scripts:
* StudentGenerator.cs
* Student.cs
* Stare.cs

## Bugs

### MicControl Crash
When previewing the game in the editor, leaving MicControl-related component active will crash the game and Unity. To prevent this, in the RailBody GameObject, turn the following components inactive:
* MicControl
* Audio Source
* Speech Recognition
* When building, however, these issues are not present so the components can be active.

### Students
Students sometimes do not appear along long stretches of hallway.
Student voices sometimes stutter and repeat.

## External Resources:
* MicControl
* DynamicBone


