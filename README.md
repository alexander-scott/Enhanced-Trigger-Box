Enhanced Trigger Box
=======

Enhanced Trigger Box is a free tool that be used within Unity. It allows developers to setup various responses to be executed when a player walks into a certain area. You can also setup conditions that must be met before the responses get executed such as camera conditions where for example the player musn't be looking at a specific object. Or player pref conditions such as progress through a level. Responses are executed after all conditions have been met. These range from spawning or destroying objects to playing animations or alterings materials. 

You can easily extend the Enhanced Trigger Box yourself by adding more responses or conditions. This will be explained in more detail further down the page. 

*Current version: [v0.2.0]*

Getting started
---------------

Download the asset from the asset store and import it into your project. Or download the zip file from GitHub and place it in your project. From there you can open up the demo scene and explore that (the demo scene uses the RigidBodyFPSController from the standard assets which will also need importing).

The demo scene shows various conditions and responses in action. To add a new Enhanced Trigger Box you can use the prefab located in the prefabs folder. From there you can add any conditions or responses using the drop down lists.

How it works
---------------
At the top level you have the Enhanced Trigger Box script. It has some base options and a box collider that represents the boundaries of the Enhanced Trigger Box. Beneath that you have Enhanced Trigger Box Components that you are able to add to the Enhanced Trigger Box. These come in the form of either a Condition or a Response and are located in the Scripts/TriggerBoxComponents folder.

When a Enhanced Trigger Box gets triggered (another gameobject collides with it's box collider), all the conditions get checked to see if the condition has been met. If all the conditions have been met, all the responses get executed. 

If you click on one of the Enhanced Trigger Boxes in the scene or drag one in from the prefabs folder you can see the what the script looks like in the inspector.

### Base Options Overview

![Test Image](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/BaseOptions.png)

Trigger Tags:- Only tags listed here are able to trigger the trigger box. To have more than one string, put a comma between them. By default the Player tag is used here.

Debug Trigger Box:- If true, the script will write to the console when certain events happen such as when the trigger box is triggered.

Hide Warnings:- If this is true, the script won't perform checks when the scene is run to notify you if you're missing any required references.

Draw Wire:- If this is true, the script won't perform checks when the scene is run to notify you if you're missing any required references.

TriggerBox Colour:- This is the colour the trigger box and it's edges will have in the editor.

After Trigger:- This allows you to choose what happens to this gameobject after the trigger box has been triggered. Set Inactive will set this gameobject as inactive. Destroy trigger box will destroy this gameobject. Destroy parent will destroy this gameobject's parent. Do Nothing will mean the trigger box will stay active and continue to operate.

Trigger Follow:- This allows you to choose if you want your trigger box to stay positioned on a moving transform or the main camera. If you pick Follow Transform a field will appear to set which transform you want the trigger box to follow. Or if you pick Follow Main Camera the trigger box will stay positioned on wherever the main camera currently is.

Condition Time:- This lets you set an additional time requirement on top of the conditions. This is the total time that the conditions must be met for in seconds.

Can Wander:- If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.

### Conditions Overview

After the base options there is a divider and then all the conditions added to this Enhanced Trigger Box will be listed. 

If you have added a blank copy from the prefabs folder you will not see any conditions.

Beneath the list of conditions (or if there's no conditions this is the only thing you can see) will be a drop down list called Add A New Condition. 

In this drop down list are all the available conditions that you can add. You can add as many conditions as you want.

Selecting a condition from this list will add it to the Enhanced Trigger Box and you will see if above the Add A New Condition drop down list. 

The structure of each component will be different and each will be explained in detail later down the page.

You have now added a condition. When this Enhanced Trigger Box gets triggered this condition will be checked and will have to pass before any responses get executed. Each condition will have various options that will affect how the condition gets met. 

To remove a condition click the X in the top right of the component.

### Responses Overview

Below the conditions section is the responses section. Similarly, there is a drop down list that displays all available responses which you can add to this Enhanced Trigger Box. 

Each response will do something different and will only do it when all conditions have been met, in the order that they are listed. 

Creating a new Component
---------------

Creating a new Condition or Response is relatively painless. Open up NewComponentExample.cs in Scripts/TriggerBoxComponents. You can use this example as a template for new components.

All you need to do in inherit the EnhancedTriggerBoxComponent, override some functions from it and then add it to a list. There are 4 functions you can override. 1 is mandatory, 1 is recommended and the other two are optional. These will be explained in detail below.

If you want to view more advanced examples, go to Scripts/TriggerBoxComponents/Conditions or Scripts/TriggerBoxComponents/Responses and take a look at some of them.

#### Inherit EnhancedTriggerBoxComponent

The most important thing to do is to inherit EnhancedTriggerBoxComponent in the class definition. 

``` csharp
public class NewComponentExample : EnhancedTriggerBoxComponent { } 
```

Below that you can declare the variables you will be using for your component like normal.

``` csharp
public GameObject exampleGameobject;
```

#### DrawInspectorGUI()

It is recommened that you override DrawInspectorGUI(). This function deals with drawing the GUI, aka what your component will look like in the inspector. If you do not override this function the base function will draw it for you with certain limitations. You will not be able to use any custom structs or enums and you won't be able to add your own tooltips.

Here's how you would draw a gameobject to a inspector (first example in the codeblock). EditorGUILayout.ObjectField is the typical object reference field you
always see in Unity. It returns the object which we will need to save as exampleGameObject so we do exampleGameObject = ObjectField.
Notice the (GameObject) before EditorGUILayout? This is because the ObjectField returns a object not a GameObject so we must 
explicitly convert it to a GameObject. For the first bit of object field we'll create a new GUIContent which will hold the field name 
(label before the field) and field tooltip (text that is displayed on hover). After that we pass in exampleGameObject again as the ObjectField 
needs the current object there so it is displayed correctly. Then we set the type which is in this case gameobject. The final 'true' allows the 
user to use gameobjects currently in the scene which we want so set it to true.

``` csharp
public override void DrawInspectorGUI()
{
        exampleGameobject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Example Game Object",
                "Example tooltip"), exampleGameobject, typeof(GameObject), true);

        exampleBool = EditorGUILayout.Toggle(new GUIContent("Example Bool",
                "Example tooltip"), exampleBool);

        exampleInt = EditorGUILayout.IntField(new GUIContent("Example Int",
                "Example tooltip"), exampleInt);

		exampleString = EditorGUILayout.TextField(new GUIContent("Example String",
				"Example tooltip"), exampleString);

		exampleEnum = (EnumName)EditorGUILayout.EnumPopup(new GUIContent("Example Enum",
				"Example tooltip"), exampleEnum);
}
```

#### ExecuteAction()

ExecuteAction() must be overriden. If this component is a condition this function is called when the trigger box is triggered (player enters it) and must return
true or false depending on if the condition has been met. If this component is a response then this function is called when all conditions
have been met and returns true or false depending on if the response has executed correctly. In the codeblock you can see a basic ExecuteAction() example
for a condition. If something has done something return true and then the responses can start executing.

``` csharp
public override bool ExecuteAction()
{
        if (exampleInt > 5)
			return true;
		else
			return false;
}
```

#### OnAwake()

OnAwake() is an optional function that you can override. This function is called either when the game first starts or when the Enhanced Trigger Box gets
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

Validation() is an optional function you can override. This function is called in edit mode after the Enhanced Trigger Box GUI gets drawn. 
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

#### Including your new component

Once you are happy with your new component you can then add it to the responses or conditions drop down lists so you can add it in the editor.
Navigate to the top of EnhancedTriggerBox.cs. You will see two enums: TriggerBoxConditions and TriggerBoxResponse. Add the name of your new class
to either enum, depending of course on whether its a condition or response. Make sure you enter the exact class name and it's helpful to keep
it in alphabetical order.

``` csharp
public enum TriggerBoxConditions
{
    SelectACondition,
    CameraCondition,
	NewComponentExample,
    PlayerPrefCondition,
}
```

Now your can add your new component as a condition or response in the editor! Because it's inherited EnhancedTriggerBoxComponent it will follow
all the same rules as the other components and functions will get called when they're supposed to. If you think your new component is useful
send it to me or add create a pull request on GitHub and I'll add it to the asset.

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

If the component parameter is set to Full Box Collider, the entire box collider must be within the cameras view. So instead of doing the above if statement on just the transform it does it on all of the box colliders points to ensure it is all in view. Minimum Box Collider is similar but only one point on the box collider needs to be visible for it to pass. Mesh Renderer uses the in-built isVisible function to work out if it is visible in a camera.

Raycast intensity allows you to customise the raycasts that get fired when checking if there's anything blocking the cameras view to the object. Ignore obstacles won't do any raycast check at all, meaning you just have to look in the direction of the object and the condition will pass, even if there's something in the way. Very low does raycast checks at a maximum of once per second against the objects position. Low does raycast checks at a maximum of once per 0.1 secs against the objects position. Med does raycast checks once per frame against the objects position. High does raycast checks once per frame against every corner of the box collider.

#### How doing the Looking Away condition type work?

It is essentially the opposite of the Looking At condition. For example, with the transform component type, if the objects position is outside of the cameras view frustum the condition will pass. Full Box Collider will pass if the whole box collider is outside of the cameras view. Minimum Box Collider will pass if any part of the box collider is outside of the cameras view. Mesh Renderer will pass if the isVisible function is false.

One thing to note is that it doesn't do any raycast checks. This means that if an object is hidden behind an obstacle it won't pass if the camera is looking in it's direction. This will be added shortly. 

#### Component fields

Camera:- This is the camera that will be used for the condition. By default this is the main camera.

Condition Type:- This is the type of condition you want. The Looking At condition only passes when the user can see a specific transform or gameobject. The Looking Away condition only passes when a transform or gameobject is out of the users camera frustum.

Condition Object:- This is the object that the condition is based upon.

Component Parameter:- This is the type of component the condition will be checked against.  Either transform (a single point in space), minimum box collider (any part of a box collider), full box collider (the entire box collider) or mesh renderer (any part of a mesh). For example with the Looking At condition and Minimum Box Collider, if any part of the box collider were to enter the camera's view, the condition would be met.

Raycast Intesity:- When using the Looking At condition type raycasts are fired to make sure nothing is blocking the cameras line of sight to the object. Here you can customise how those raycasts should be fired. Ignore obstacles fires no raycasts and mean the condition will pass even if there is an object in the way. Very low does raycast checks at a maximum of once per second against the objects position. Low does raycast checks at a maximum of once per 0.1 secs against the objects position. Med does raycast checks once per frame against the objects position. High does raycast checks once per frame against every corner of the box collider.

![Camera Condition](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/CameraCondition.png)

### Player Pref Condition

The player pref condition can be used to compare values stored in player prefs. For example, the player pref, LevelProgression, must have a value of over 5 before this condition gets met. To use this condition select the condition type, such as greater than, then enter the key of the player pref and finally the value that you want to compare that player pref with. 

#### How does it work?

It's quite simple. The value in the player pref with playerPrefKey gets compared against playerPrefValue using the condition type. If you tick refresh every frame the value in the player pref will be retrieved every time the condition is checked. If you untick refresh every frame it will retrieve it once and cache it when you first start the game.

#### Component fields

Condition Type:- The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than.

Player Pref Key:- The key (ID) of the player pref that will be compared against the above value.

Player Pref Type:- This is the type of data stored within the player pref. Options are int, float and string.

Player Pref Value:- The value that will be used to compare against the value stored in the player pref.

Refresh Every Frame:- If true, the value in the player pref will be retrieved every time the condition check happens. If false, it will only retrieve the player pref value once, when the game first starts.

![Player Pref Condition](https://alex-scott.co.uk/img/portfolio/TrigBoxSS/PlayerPrefCondition.png)

Individual Responses
---------------

Below this, all of the current responses will be listed and described in detail. Remember once the trigger box gets entered, all the conditions
get checked to see if they have been met. Once they all have been met, all the responses get executed.

### Animation Response

