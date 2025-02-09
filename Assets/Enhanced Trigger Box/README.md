## Enhanced Trigger Box

For full documentation and videos, visit: [Enhanced Trigger Box Documentation](https://github.com/alexander-scott/Enhanced-Trigger-Box).

Thank you for your interest in Enhanced Trigger Box! This document provides a brief overview of how to add an Enhanced Trigger Box to your scene. To learn how to create your own conditions and responses for the Enhanced Trigger Box, please refer to the full documentation.

### What is Enhanced Trigger Box?

Enhanced Trigger Box is a free tool that can be used within Unity. It allows developers to set up various responses to be executed when a player enters a specific area. You can also configure conditions that must be met before the responses are executed. These conditions might include camera conditions (e.g., ensuring the player is not looking at a specific object) or player preferences such as progress through a level. Once all conditions have been met, the responses are executed. These responses can range from spawning or destroying objects to playing animations or altering materials.

### Example Scene

You can open the example scene in the Examples directory to see Enhanced Trigger Box in action. Please note that example scene in this version of the ETB has been built with Unity 6.0 and is not guaranteed to work with drastically older or newer versions of the engine. However, the script itself will work in any engine version from Unity 5.0 onward. The examples directory is fully optional and if you have no desire to explore it (or are worried about engine incompatibles), you do not need to import that directory and instead you can just import the scripts directory.

### Adding an Enhanced Trigger Box to Your Scene

Go to the prefabs folder and drag the ETB prefab into your scene. This prefab contains the script as well as a box collider to get you started. Adjust the size and position of the ETB as needed.

Your scene now has an ETB. If a game object with the 'Player' tag enters this trigger box, a message will be sent to the console indicating it has been triggered.

At its most basic level, this is how the Enhanced Trigger Box functions. From here, you can add conditions and responses using the two drop-down lists. When the ETB is entered, all the conditions you've added will be checked to see if they have been met. Once all conditions have been met, the responses you've added will be executed. For a detailed description of each condition and response, please refer to the full documentation.

### Need Help?

If you need assistance, find any bugs, or have created a condition/response you think would be useful to others, please send an email to alexanderscott@outlook.com.
