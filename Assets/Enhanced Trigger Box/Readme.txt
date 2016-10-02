For the full documentation and videos visit: https://alex-scott.co.uk/portfolio/enhanced-trigger-box.html

Hi, thanks for taking an interest in Enhanced Trigger Box. This text document will give you a brief overview of how to
add a Enhanced Trigger Box to your scene. To learn how to create your own conditions and responses for the Enhanced
Trigger Box check out the full documentation. 

Enhanced Trigger Box is a free tool that be used within Unity. It allows developers to setup various responses to 
be executed when a player walks into a certain area. You can also setup conditions that must be met before the 
responses get executed such as camera conditions where for example the player musn’t be looking at a specific object. 
Or player pref conditions such as progress through a level. Responses are executed after all conditions have been met. 
These range from spawning or destroying objects to playing animations or altering materials.

You can open up the demo or example scenes to see some of them in action. Please note that the Demo and Examples scene 
require the Unity Standard Assets (specifically the FPSController) to be imported  and will not open in anything lower than 
Unity 5.4. However the script itself will work from Unity 5.0. If you do not wish to view the demos, do not want to import the 
standard assets or are using anything under Unity 5.4 you should NOT import the demos folder.

To add a Enhanced Trigger Box to your scene go to the prefabs folder and drag the ETB prefab into your scene. This prefab
contains the script as well as a box collider to get you started. Move it around or make it bigger or smaller depending
or what you want. You now have an ETB in the scene. If you walk into this trigger box with a gameobject with the 'Player'
tag it will send a message to the console saying it has been triggered. That's essentially it at it's most basic level.
From there you can add conditions and responses using the two drop down lists. When the ETB gets entered all the conditions
you've added get checked to see if they have been met. Once they've all been met all the responses you've added get
executed. For a description of what each condition and response does check out the full documentation.

If you need help with anything, find any bugs or have created a condition/response you think would be useful to others
send me an email at alexanderscott@outlook.com.