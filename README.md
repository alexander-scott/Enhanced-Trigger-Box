Enhanced Trigger Box
=======

Enhanced Trigger Box is a free tool that be used within Unity. It allows developers to setup various responses to be executed when an object enters a specified area. You can also setup conditions that must be met before the responses get executed such as camera conditions where for example a player mustn't be looking at a specific object. Or player pref conditions such as progress through a level.  Responses are executed after all conditions have been met.These range from spawning GameObjects or disabling components to playing animations or altering material properties.

It has been designed in a way that allows you to easily extend the Enhanced Trigger Box yourself by adding more responses or conditions. This will be explained in more detail further down the page. 

*Current version: [v0.41]*

Getting started
---------------

Download the asset from the asset store and import it into your project. Or download the zip file from GitHub and place it in your project. From there you can open up the demo or examples scene and explore that (they both use the FPSController from the standard assets which will also need importing). To add a new Enhanced Trigger Box you can use the prefab located in the prefabs folder. From there you can add any conditions or responses using the drop down lists.

Please note that the Demo and Examples scenes in this asset require the Unity Standard Assets (specifically the FPSController) to be imported and are not supported in anything lower than Unity 5.4. However the script itself will work with Unity 5.0 and above. If you do not wish to view the demos, do not want to import the standard assets or are using anything under Unity 5.4 you should NOT import the demos folder. Just import the scripts folder and you can add the script to a GameObject and get going from there.

### Demo Scene Overview

This scene contains a mashup of a few conditions and responses. All you have to do is explore. The Enhanced Trigger Box is used a bit excessively here and you wouldn't normally use it this way as a lot of instantiating and destroying is happening but it gives you an idea of what you can do.

[Here's a video of the demo scene as of v0.1.0. >](https://youtu.be/MIJ6kTY1X4c)

### Examples Scene Overview

This scene contains a showcase of most of the conditions and responses one at a time giving you an idea of what they do. To activate a response just walk into the box. To test a condition walk into the box and meet the condition, for example the Looking Away condition don't look at the cube in front of you. The cube will turn green when the box gets successfully triggered and it will also send a message to the console.

[Here's a video of the examples scene as of v0.1.0. >](https://youtu.be/bjobfHm6cas)

How does it all work 
---------------
At the top level you have the Enhanced Trigger Box script. It has some base options and uses a box collider to represents the boundaries of the Enhanced Trigger Box. Beneath that you have Enhanced Trigger Box Components which are MonoBehaviours that you are able to add to the Enhanced Trigger Box. These come in the form of either a Condition or a Response and are located in the Scripts/TriggerBoxComponents folder.

When a Enhanced Trigger Box gets entered by another object with a collider (you can disable this entry check if you want and it will be treated as 'entered' on init), all the conditions get checked to see if each condition has been met. If all the conditions have been met, all the responses get executed. 

If you click on one of the Enhanced Trigger Boxes in the scene or drag the ETB prefab in from the prefabs folder (or just add the EnhancedTriggerBox script to a gameobject) you can see the what the script looks like in the inspector.

### Base Options Overview

![Test Image](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/BaseOptions.png)

Debug Trigger Box:- If true, the script will write to the console when certain events happen such as when the trigger box is triggered.

Hide Warnings:- If this is true, the script won't perform checks to notify you if you're missing any required references or if theres any errors.

Disable Entry Check:- If true, the entry check on the trigger box will be disabled, meaning it will go straight to the condition checking instead of waiting for something to enter the trigger box.

Trigger Tags:- Only gameobjects with tags listed here are able to trigger the trigger box. To have more than one tag, put a comma between them. If you leave this field blank any object will be able to trigger it.

TriggerBox Colour:- This is the colour the trigger box and it's edges will have in the editor.

Trigger Follow:- This allows you to choose if you want your trigger box to stay positioned on a moving transform or the main camera. If you pick Follow Transform a field will appear to set which transform you want the trigger box to follow. Or if you pick Follow Main Camera the trigger box will stay positioned on wherever the main camera currently is.

After Trigger:- This allows you to choose what happens to this gameobject after the trigger box has been triggered. Set Inactive will set this gameobject as inactive. Destroy trigger box will destroy this gameobject. Destroy parent will destroy this gameobject's parent. Do Nothing will mean the trigger box will stay active and continue to operate. ExecuteExitResponses allows you to set up additional responses to be executed after the object that entered the trigger box leaves it.

Condition Time:- This lets you set an additional time requirement on top of the conditions. This is the total time that the conditions must be met for in seconds.

Can Wander:- If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.

### Conditions Overview

After the base options there is a divider and then all the conditions added to this Enhanced Trigger Box will be listed. If you have added a blank copy from the prefabs folder you will not see any conditions.

Beneath the list of conditions (or if there's no conditions this is the only thing you can see) will be a drop down list called Add A New Condition. In this drop down list are all the available conditions that you can add. You can add as many conditions as you want.

Selecting a condition from this list will add it to the Enhanced Trigger Box and you will see it above the Add A New Condition drop down list. The structure of each component will be different and each will be explained in detail later down the page.

You have now added a condition. When this Enhanced Trigger Box gets entered this condition will be checked and will have to pass before any responses get executed. Each condition will have various options that will affect how the condition gets met. To remove a condition click the X in the top right of the component.

### Responses Overview

Below the conditions section is the responses section. Similarly, there is a drop down list that displays all available responses which you can add to this Enhanced Trigger Box. 

Each response will do something different and will only do it when all conditions have been met, in the order that they are listed. 

### Exit Responses Overview

If you set After Trigger to ExecuteExitResponses, you'll have access to a third section which displays all active Exit Responses and allows you to add more. Exit Responses are the same as responses, but instead of executing when a GameObject enters the trigger box, they get executed when the GameObject leaves the trigger box.

After the GameObject leaves the trigger box and the Exit Responses get executed, the trigger box will revert to untriggered, allowing it to be initally triggered by other GameObjects. Only the GameObject that entered the trigger box can trigger the leave event.

Creating a new Component
---------------

Creating a new Condition or Response is relatively painless. Open up NewComponentExample.cs in Scripts/TriggerBoxComponents. You can use this example as a template for new components.

All you need to do is inherit ConditionComponent or ResponseComponent (depending on whether the new component is a condition or response), make sure it's in the Enhanced Trigger Box namespace and then override some functions. There are 5 functions you can override. 1 is mandatory, 1 is recommended and the other three are optional. These will be explained in detail below.

If you want to view more advanced examples, go to Scripts/TriggerBoxComponents/Conditions or Scripts/TriggerBoxComponents/Responses and take a look at some of them.

#### Inherit ConditionComponent or ResponseComponent

The most important thing to do is to inherit ConditionComponent or ResponseComponent in the class definition and for the class to be within the EnhancedTriggerbox.Component namespace. It's also recommended to add "[AddComponentMenu("")]" above the class name. This attribute means you won't see it in the Add Component Menu and it unfortunately can't be passed down by inheritance so it must be added manually.

``` csharp
namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class NewComponentExample : ConditionComponent { } 
}
```

Below that you can declare the variables you will be using for your component like normal.

``` csharp
public GameObject exampleGameobject;
```

#### DrawInspectorGUI()

It is recommened that you override DrawInspectorGUI(). This function deals with drawing the GUI, aka what your component will look like in the inspector. If you do not override this function the base function will draw it for you with certain limitations. Those limitations being you will not be able to use any custom structs or enums and you won't be able to add your own tooltips. If you do choose to override this function make sure you encapsulate it within the '#if UNITY_EDITOR' tags as this is editor related code.

Here's how you would draw a gameobject to a inspector (first example in the codeblock). UnityEditor.EditorGUILayout.ObjectField is the typical object reference field you
always see in Unity. It returns the object which we will need to save as exampleGameObject so we do exampleGameObject = ObjectField.
Notice the (GameObject) before UnityEditor.EditorGUILayout? This is because the ObjectField returns a object not a GameObject so we must 
explicitly convert it to a GameObject. For the first bit of object field we'll create a new GUIContent which will hold the field name 
(label before the field) and field tooltip (text that is displayed on hover). After that we pass in exampleGameObject again as the ObjectField 
needs the current object there so it is displayed correctly. Then we set the type which is in this case gameobject. The final 'true' allows the 
user to use gameobjects currently in the scene which we want so set it to true.

Below the gameobject example are other examples of how you would add your variables to the inspector including, bools, ints, strings and enums. For more information about using the EditorGUILayout click [here.](https://docs.unity3d.com/ScriptReference/EditorGUILayout.html)

``` csharp
#if UNITY_EDITOR
	public override void DrawInspectorGUI()
	{
			exampleGameobject = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Example Game Object",
					"Example tooltip"), exampleGameobject, typeof(GameObject), true);

			exampleBool = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Example Bool",
					"Example tooltip"), exampleBool);

			exampleInt = UnityEditor.EditorGUILayout.IntField(new GUIContent("Example Int",
					"Example tooltip"), exampleInt);

			exampleString = UnityEditor.EditorGUILayout.TextField(new GUIContent("Example String",
					"Example tooltip"), exampleString);

			exampleEnum = (EnumName)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Example Enum",
					"Example tooltip"), exampleEnum);
	}
#endif
```

#### ExecuteAction()

ExecuteAction() MUST be overriden. If this component is a condition this function is called when the trigger box is entered (gameobject enters it) and must return
true or false depending on if the condition has been met. If this component is a response then this function is called when all conditions
have been met and returns true or false depending on if the response has executed correctly. In the first codeblock you can see a basic ExecuteAction() example
for a condition. If something has done something return true and then the responses can start executing. In the second codeblock you can see a basic
ExecuteAction() for a response.

``` csharp
public override bool ExecuteAction()
{
		// Basic conditional if statement
        if (exampleInt > 5)
			return true;
		else
			return false;
}
```

``` csharp
public override bool ExecuteAction()
{
		// Very basic response
        exampleInt = 0;
        return true;
}
```

If your response takes some time to fully execute you should use the 'duration' variable (from the EnhancedTriggerBoxComponent class) to tell the main script to wait for it to finish before deactivating/destroying the trigger box. For a usage example look in the lighting response at the ChangeLightColourOverTime() coroutine.

You can also ovveride an overload function for ExecuteAction: ExecuteAction(GameObject collisionGameObject). This overload provides you with access to the GameObject that collided with the trigger box, allowing you to manipulate that as you wish. 

However, you must tell the EnhancedTriggerBox that you are using the function with the collision data over the function without it. To do this you need to override the variable 'requiresCollisionObjectData' and set it to true. Below is an example of usage of the overload function as well as how you would override the variable.

``` csharp
public override bool requiresCollisionObjectData
{
    get
    {
        return true;
    }
}

public override bool ExecuteAction(GameObject collisionGameObject)
{
	collisionGameObject.GetComponent<MeshRenderer>().material.SetColor(propertyName, propertyColour);
}
```

#### OnAwake()

OnAwake() is an optional function that you can override. This function is called when the game first starts or when the Enhanced Trigger Box gets
initalised. You can place whatever you want in here and it will be executed when the game starts or when the Enhanced Trigger Box gets 
initalised. The most common uses will be for caching components or objects or getting updated values.

``` csharp
public override void OnAwake()
{
        exampleBoxCollider = targetObject.GetComponent<BoxCollider>();
		float.TryParse(PlayerPrefs.GetString("PlayerPrefKey"), out examplePlayerPref);
}
```

#### Validation()

Validation() is an optional function you can override. This function is called multiple times a frame in the editor after the Enhanced Trigger Box GUI gets drawn. 
You should use it for displaying warnings about your component such as missing references or invalid values. To display a warning use
ShowWarningMessage("Your warning"). This will take into account if the user has disabled warnings or not in the base options.

``` csharp
public override void Validation()
{
        if (!exampleGameobject)
        {
            ShowWarningMessage("WARNING: You haven't assigned an object to exampleGameobject.");
        }
}
```

Now your can add your new component as a condition or response in the editor! Because it's inherited ConditionComponent or ResponseComponent it will follow
all the same rules as the other components and functions will get called when they're supposed to. If you think your new component could be useful to others,
send it to me or create a pull request on GitHub and I'll add it to the asset.

#### ResetComponent()

ResetComponent() is an optional function you can override. AfterTrigger in Base Options needs to be set to DoNothing or ExecuteExitResponses for this function to be called.  It is called after all responses have finished executing. It can be used to reset variables or reacquire data before the trigger box starts checking for triggers again.

Individual Conditions
---------------

Below this, all of the current conditions will be listed and described in detail. Remember once the trigger box gets entered, all the conditions
get checked to see if they have been met. Once they all have been met, all the responses get executed.

### Camera Condition

The camera condition can be used to see if a camera is looking at or not looking at something. To use this condition, simply drag something onto Condition Object and select either Looking At or Looking Away.

#### How does the Looking At condition type work?

Once you supply an object for Condition Object you can select the component parameter which will be what the condition works with. For example, transform will mean the condition can only pass if the transform's position is within the camera planes or Full Box Collider will mean that the entire box collider must be in view.

For Transform it first transforms the condition object's position from world space into the selected camera's viewport space. It then checks that position to see if it's within the camera's planes by using the following if statement:

 ``` csharp
 if (viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
     viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1)
```

If this statement is true then it means the object is in the cameras view. It will then fire a raycast from the camera in the direction of the object to make sure no objects are blocking it's view (unless ignore obstacles is ticked). If that succeeds it means there was either nothing in the way or it hit our object and the condition has passed.

If the component parameter is set to Full Box Collider, all important points on a box collider must be within the cameras view. So instead of doing the above if statement on just the transform it does it on all of the box colliders points to ensure it is all in view. Minimum Box Collider is similar but only one point on the box collider needs to be visible for it to pass. 

One thing to note for the Full Box Collider is that if you're up close to large objects the condition could get met unintentionally. Because only important points on the box collider (such as corners and centers) are checked, if you're really close to a big objects so that none of those points are in your camera the condition will get met. A solution to this is to use the min distance field so that they have to be a certain distance away from the object.

Mesh Renderer uses the in-built isVisible function to work out if it is visible in a camera. Note that an object is considered visible when it's existence is visible for any reason in the editor. For example, it might not actually be visible by any camera but still need to be rendered for shadows. Also remember that if the object is visible in the scene window and not the game window it will still be classed as visible.

Raycast intensity allows you to customise the raycasts that get fired when checking if there's anything blocking the cameras view to the object. Ignore obstacles won't do any raycast checks at all, meaning you just have to look in the direction of the object and the condition will pass, even if there's something in the way. Very low does raycast checks at a maximum of once per second against the objects position. Low does raycast checks at a maximum of once per 0.1 secs against the objects position. Med does raycast checks once per frame against the objects position. High does raycast checks once per frame against every corner of the box collider.

#### How doing the Looking Away condition type work?

It is essentially the opposite of the Looking At condition. For example, with the transform component type, if the objects position is outside of the cameras view frustum the condition will pass. Full Box Collider will pass if the whole box collider is outside of the cameras view. Minimum Box Collider will pass if any part of the box collider is outside of the cameras view. Mesh Renderer will pass if the isVisible function is false.

One thing to note is that it doesn't do any raycast checks. This means that if an object is hidden behind an obstacle it won't pass if the camera is looking in it's direction. This will be added shortly. 

#### Component fields

Camera:- This is the camera that will be used for the condition. By default this is the main camera.

Condition Type:- This is the type of condition you want. The Looking At condition only passes when the user can see a specific transform or gameobject. The Looking Away condition only passes when a transform or gameobject is out of the users camera frustum.

Condition Object:- This is the object that the condition is based upon.

Component Parameter:- This is the type of component the condition will be checked against.  Either transform (a single point in space), minimum box collider (any part of a box collider), full box collider (the entire box collider) or mesh renderer (any part of a mesh). For example with the Looking At condition and Minimum Box Collider, if any part of the box collider were to enter the camera's view, the condition would be met.

Raycast Intesity:- When using the Looking At condition type raycasts are fired to make sure nothing is blocking the cameras line of sight to the object. Here you can customise how those raycasts should be fired. Ignore obstacles fires no raycasts and mean the condition will pass even if there is an object in the way. Very low does raycast checks at a maximum of once per second against the objects position. Low does raycast checks at a maximum of once per 0.1 secs against the objects position. Med does raycast checks once per frame against the objects position. High does raycast checks once per frame against every corner of the box collider.

Min Distance:- This field allows you to set a minimum distance between the selected camera and target object before the condition gets checked.

![Camera Condition](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/CameraCondition.png)

### Input Condition

This condition requires the user to either press or release a certain key to be met. Only keyboard inputs are currently supported.

#### Component fields

Input Key:- The key that needs to be interacted with to meet the condition.

Trigger Type:- The type of interaction required. OnPressed requires the user to simply press the key down. OnReleased requires the user to let go of the key after pressing it.

![Input Condition](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/InputCondition.png)

### Player Pref Condition

The player pref condition can be used to compare values stored in player prefs. For example, the player pref, LevelProgression, must have a value of over 5 before this condition gets met. To use this condition select the condition type, such as greater than, then enter the key of the player pref and finally the value that you want to compare that player pref with. 

#### How does it work?

It's quite simple. The value in the player pref with playerPrefKey gets compared against playerPrefValue using the condition type. If you tick refresh every frame the value in the player pref will be retrieved every time the condition is checked. If you untick refresh every frame it will retrieve it once and cache it when you first start the game.

 ``` csharp
playerPrefString = PlayerPrefs.GetString(playerPrefKey);
```

#### Component fields

Condition Type:- The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than.

Player Pref Key:- The key (ID) of the player pref that will be compared against the above value.

Player Pref Type:- This is the type of data stored within the player pref. Options are int, float and string.

Player Pref Value:- The value that will be used to compare against the value stored in the player pref.

Refresh Every Frame:- If true, the value in the player pref will be retrieved every time the condition check happens. If false, it will only retrieve the player pref value once, when the game first starts.

![Player Pref Condition](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/PlayerPrefCondition.png)

### Transform Condition

The transform condition can be used to compare position or rotation from a transform against a value. For example if a object's Y position is lower than a certain value.

#### Component fields

Target Transform:- The transform to apply the condition to.

Transform Component:- The transform component that will be used for the condition. Either position or rotation.

Target Axis:- The axis that the condition will be based on.

Condition Type:- The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than.

Value:- The value that will be compared against the value in the axis selected above.

![Transform Condition](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/TransformCondition.png)

Individual Responses
---------------

Below this, all of the current responses will be listed and described in detail. Remember once the trigger box gets entered, all the conditions
get checked to see if they have been met. Once they all have been met, all the responses get executed.

### Animation Response

The animation response can be used to set a mecanim trigger on a gameobject, stop all animations on a gameobject or play an animation clip on a gameobject. You can either pass in a reference for a GameObject, supply a GameObject's name or use the GameObject that triggered the box as a target for this response.

#### How does it work?

This response is very self-explanatory as it literally just calls Unity animator functions. One thing to note is that when playing the animation clip it will plays an animation clip on the target animation over 0.3 seconds and will fade other animations out.

 ``` csharp
if (stopAnim && animationTarget)
{
    animationTarget.GetComponent<Animation>().Stop();
}

if (animationClip && animationTarget)
{
    animationTarget.GetComponent<Animation>().CrossFade(animationClip.name, 0.3f, PlayMode.StopAll);
}

if (!string.IsNullOrEmpty(setMecanimTrigger) && animationTarget)
{
    animationTarget.GetComponent<Animator>().SetTrigger(setMecanimTrigger);
}
```

#### Component Fields

ReferenceType:- This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.

Animation Target:- The gameobject to apply the animation to.

Animation Target Name:- If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())

Set Mecanim Trigger:- The name of the trigger on the gameobject animator that you want to trigger.

Stop Animation:- Stops the current animation on the animation target.

Animation Clip:- The animation clip to play.

![Animation Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/AnimationResponse.png)

### Audio Response

The audio response can be used to play music, mute audio on the main camera and play sound effects at any position. There is also the option to loop the music and change the volume of the music and sound effect.

The Audio Response can be used to modify an Audio Source through actions such as playing an audio clip, stopping all audio, changing the volume or restarting an audio clip. It can also be used to play sound effects at a position.

#### Component Fields

Response Type:- The type of response that this component will be using. AudioSource allows you to modify an audio source and SoundEffect allows you to play positional sound effects.

Audio Source:- The audio source that will be modified.

Audio Source Action:- The type of The type of action to be performed on the audio source. Play allows you to play an Audio Clip, Stop stops an audio source's currently playing clip, Restart put the time of the audio clip back to 0 and ChangeVolume sets the volume of the audio source.

Audio Clip:- This is the audio clip that will be played on the audio source if the action selected is Play.

Music Volume:- The volume of the audio clip when played. Or if the action is change volume this is the volume that the audio source will be set to. Default is 1.

Change Duration:- The duration that the volume change will happen over in seconds. If you leave it as 0 it will perform the changes instantly. Only used if the action selected is Change Volume.

Loop Music:- If this is true, the audio clip will loop when played if the action selected is Play.

Play Sound Effect:- This is an audio clip, played at a certain position in world space as defined below.

Sound Effect Position:- The position the sound effect will be played at.

Sound Effect Volume:- The volume of the played sound effect. Default is 1.

![Audio Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/AudioResponse.png)

### Lighting Response

This response allows you to modify an individual light source or the scene's lighting settings. You can also make your changes apply over a period of time.

#### Component Fields

Edit Type:- Select whether you want to modify an indivdual light or the scene's lighting settings.

Target Light:- The light that will modified.

Change Colour:- Choose to change the colour of this light. Remain the same will not change the colour.

Set Colour:- The colour that the target light will be set to.

Set Intensity:- The intensity you want to set the target light to. If you leave this field blank the light intensity will not be changed.

Set Bounce Intensity:- The bounce intensity you want to set the target light to. If you leave this field blank the light bounce intensity will not be changed.

Set Range:- The range you want to set the target light's range to. If you leave this field blank the range will not be changed. Only displayed when a spot or point light is selected.

Set Skybox:- This is the material that you want to set the scene's skybox to. If you leave this field blank the skybox will not be changed.

Change Ambient Light Colour:- Choose to change the colour of the scene's ambient light. Remain the same will not change the colour.

Ambient Light Colour:- The colour that the scene's ambient light will be set to.

Change Duration:- The duration that the selected change will happen over in seconds. If you leave it as 0 it will perform the changes instantly.

![Lighting Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/LightingResponse.png)

### Load Level Response

This response allows you to load or unload as scene, as well as options to either asynchronously or additively load a scene.

#### Component Fields

Response Type:- The type of response that you want be executed. Either loading or unloading a scene.

Load Level Name:- The name of the scene you want to load. Depending on your Unity version this may be a the scene index instead of name.

Asynchronously:- Only available when loading a scene. If this is true, the scene will be loaded asynchronously. This means that the playing scene won't freeze as it's loading this new scene on a background thread. If this is false everything will freeze/wait until the new scene is loaded.

Additive:- Only available when loading a scene. If this is true the new scene will be displayed alongside/as well as the current scene. If this is false the current scene will be unloaded.

![Load Level Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/LoadLevelResponse.png)

### Modify GameObject Response

This response allows you to modify a gameobject by either disabling, enabling or destroying it. Or you can enable/disable specific Unity components on a gameobject. You can either pass in a reference for a GameObject, supply a GameObject's name or use the GameObject that triggered the box as a target for this response.

You are unable to enable a gameobject by name because GameObject.Find() cannot be used on inactive objects. You also cannot disable or enable a Unity component without supplying a gameobject reference. It is good practice to use object references instead of searching for objects anyway. 

``` csharp
gameObject.SetActive(true);
gameObject.SetActive(false);
Destroy(gameObject);
```

#### Component Fields

ReferenceType:- This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.

GameObject:- The gameobject that will modified.

GameObject Name:- If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())

Modify Type:- This is the type of modification you want to happen to the gameobject. Options are destroy, disable, enable, disable component and enable component.

Select Component:- This is the Unity component on the gameobject that you want to enable/disable.

![Modify GameObject Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/ModifyGameObjectResponse.png)

### Modify Rigidbody Response

This response allows you to modify any part of a rigidbody component on another gameobject.

#### Component Fields

Rigidbody:- The rigidbody that will be modified.

Set Mass:- Set the mass of this rigidbody. If left blank the value will not be changed.

Set Drag:- Set the drag of this rigidbody. If left blank the value will not be changed.

Set Angular Drag:- Set the angular drag of this rigidbody. If left blank the value will not be changed.

Change Gravity:- Choose to set whether this rigidbody should use gravity. Selecting remain the same will not change the value and selecting toggle will invert the value.

Change Kinematic:- Choose to set whether this rigidbody is kinematic. Selecting remain the same will not change the value and selecting toggle will invert the value.

Change Interpolate:- Choose to set this rigidbody to interpolate or extrapolate. Selecting remain the same will not change the value.

Change Collision Detection:- Choose to set this rigidbody's collision detection between discrete or continuous. Selecting remain the same will not change the value.

![Rigidbody Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/RigidbodyResponse.png)

### Modify Transform Response

This response allows you to modify any part of a transform component on another gameobject.

#### Component Fields

Target Transform:- The transform that will be modified.

Target Attribute:- What attribute of the transform will be modified. Choose from position, rotation or scale.

Target Axis:- What axis the selected attribute will be modified on. Choose from X, Y or Z.

Local Space:- If this is checked, the modifications will be done in local space instead of world space.

Target Value Type:- The type of value that the new value is. If it is set, the transform value will be set to that value. If it is additive, the transform value will be incremented by that value. 

Target Value:- The value you would like to set this attribute to.

Change Duration:- The duration you want this change to happen over. Leaving this at 0 will result in it happening instantly.  

![Modify Transform Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/ModifyTransformResponse.png)

### Player Pref Response

This response allows you to save a value to a player pref. The supported data types are int, float and string. If you're dealing with ints or floats you can choose to increment or decrement the value by 1 by entering '++' or '--' in the value field.

``` csharp
PlayerPrefs.SetString(setPlayerPrefKey, setPlayerPrefVal);
```
#### Component Fields

Player Pref Key:- This is the key (ID) of the player pref which will have its value set.

Player Pref Type:- This is the type of data stored within the player pref.

Player Pref Value:- This is the value that will be stored in the player pref. If you enter ++ or -- the value in the player pref will be incremented or decremented respectively.

![Player Pref Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/PlayerPrefResponse.png)

### Send Message Response

The send message response can be used to call a function on a select gameobject and pass in a parameter as well. Supported parameter types currently include int, float and string. You can either pass in a reference for a GameObject, supply a GameObject's name or use the GameObject that triggered the box as a target for this response.

#### How does it work?

The csend message response uses Unity's inbuilt GameObject.SendMessage() function to send a value to a function on another gameobject. If you select int or float it will parse the message value before sending it.

``` csharp
messageTarget.SendMessage(messageFunctionName, int.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
```

#### Component Fields

ReferenceType:- This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.

Message Target:- This is the gameobject on which the below function is called on.

Message Target Name:- If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())

Message Function Name:- This is the function which is called on the above gameobject.

Message Type:- This is the type of parameter that will be sent to the function. Options are int, float and string.

Message Value:- This is the value of the parameter that will be sent to the function.

![Send Message Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/SendMessageResponse.png)

### Set Material Property

This response allows you to change a property within a material. The material can either be a material from a gameobject or a material in the project browser. Properties that can be changed include float, int, color, vector4 and texture. You can either pass in a reference for a GameObject, supply a GameObject's name or use the GameObject that triggered the box as a target for this response.

#### Component Fields

ReferenceType:- This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.

GameObject:- The gameobject with the material you want to edit.

GameObject Name:- If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())

Material:-  A reference of the material that you want to set a properties value.

Clone Material:- If true, the script will work with a clone of the material on this gameobject. If false, it will use the original material in the project directory. WARNING: If false, it will permanently change that materials values.

Material Property Name:- The name of the property that you want to set.

Material Property Type:- The type of the property that you want to set. Float, Int, Colour, Vector4 or Texture.

Material Property Value:- The new value of this property.

Change Duration:- The duration you want this change to happen over. Leaving this at 0 will result in it happening instantly. 

![Set Material Property Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/SetMaterialPropertyResponse.png)

### Spawn GameObject Response

This response allows you to spawn a gameobject at any given position. You can also optionally change the name of the newly spawned gameobject.

#### Component Fields

Prefab to Spawn:- This is a reference to the prefab which will be instanstiated (spawned).

New instance name:- This field is used to set the name of the newly instantiated object. If left blank the name will remain as the prefab's saved name.

Custom Position / Rotation:- This is the position and rotation the prefab will be spawned with. If left blank it will use the prefab's saved attributes.

![Spawn Gameobject Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/SpawnGameObjectResponse.png)

### Teleport Response

The response simply allows you to move gameobjects from one point to another. You can either pass in a reference for a GameObject, supply a GameObject's name or use the GameObject that triggered the box as a target for this response.

#### Component fields

ReferenceType:- This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.

Target Gameobject:- This is the gameobject that you want to be moved

Target Gameobject Name:- If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())

Destination:- This is the position you want to move the gameobject to.

Copy Rotation:- If this checkbox is ticked then the target object's rotation will be set to the destination's rotation.

![Teleport Response](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/TeleportResponse.png)

Troubleshooting
---------------

#### System.Reflection.ReflectionTypeLoadException: The classes in the module cannot be loaded.

Enhanced Trigger Box uses .NET Reflection to obtain information about loaded assemblies and the types defined within, in this being the enhanced trigger box components. If you are seeing this error it means your Unity API is set to a .NET version which doesn't support Reflection. To fix this go to "Edit->Project Settings->Player-> Other settings" and set "Api Compatibility Level" to ".NET 2.0" instead of ".NET 2.0 Subset" and then reload your project.

#### Error building Player because scripts had compiler errors

This is a bug found when attempting to build a project which has been solved as of v0.1.3.

If you are upgrading from a version lower than v0.1.3 and have created any custom Enhanced Trigger Box Components you will need to make some changes to them. First off go to your new component and at the top remove 'using UnityEditor;'. Then scroll down to the DrawInspectorGUI() function. You will most likely have a few errors. These can be fixed by putting 'UnityEditor.' in front of them. Finally above the DrawInspectorGUI() function add: '#if UNITY_EDITOR' and beneath the function put: '#endif'. If you have imported the updated package you can look at the other updated components for guidance.

#### Variable values not being saved correctly in custom components

This is quite a vague issue so there could be a whole multitude of reasons why it isn't saving. One thing I would recommend checking is that all the variables are public and not private. This has solved a similar problem for me in the past and could be the solution to your problem.

#### Base ExecuteAction(GameObject collisionGameObject) being called instead of the overriden version

This is caused by requiresCollisionObjectData being set to false and the ExecuteAction() version without the GameObject references being called, which you most likely haven't implemented. To fix this simply set requiresCollisionObjectData to true in your component.

Misc
---------------

Audio file used in demo/examples scene obtained from [here](http://www.looperman.com/loops/detail/99733/piano-loop-reflections-of-life-70-by-designedimpression-free-70bpm-ambient-piano-loop) and is licensed under the Creative Commons 0 License.

The background image used on the asset store and this website is found [here](http://wonderfulengineering.com/37-programmer-code-wallpaper-backgrounds-free-download/).

The project itself is licensed under MIT license and you are free to do with it what you want.
