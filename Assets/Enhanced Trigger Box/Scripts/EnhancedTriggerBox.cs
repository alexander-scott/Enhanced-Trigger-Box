using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using EnhancedTriggerbox.Component;
using System.Text;

namespace EnhancedTriggerbox
{
    [Serializable, ExecuteInEditMode]
    [HelpURL("https://alex-scott.co.uk/portfolio/enhanced-trigger-box.html")]
    public class EnhancedTriggerBox : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// Holds all the conditions the user has added to the trigger box
        /// </summary>
        [SerializeField]
        private List<EnhancedTriggerBoxComponent> conditions = new List<EnhancedTriggerBoxComponent>();

        /// <summary>
        /// Holds all the responses the user has added to the trigger box
        /// </summary>
        [SerializeField]
        private List<EnhancedTriggerBoxComponent> responses = new List<EnhancedTriggerBoxComponent>();

        /// <summary>
        /// The getter that will return all the conditions. Used when iterating through all the conditions.
        /// </summary>
        public List<EnhancedTriggerBoxComponent> listConditions
        {
            get { return conditions; }
        }

        /// <summary>
        /// The getter that will return all the responses. Used when iterating through all the responses.
        /// </summary>
        public List<EnhancedTriggerBoxComponent> listResponses
        {
            get { return responses; }
        }

        /// <summary>
        /// This bool is used to store whether the base options tab is open in the inspector so it can persist across sessions
        /// </summary>
        public bool showBaseOptions = true;

        /// <summary>
        /// If true the application will write to the console a message with the name of the trigger that was triggered
        /// </summary>
        [Tooltip("If true, the script will write to the console when certain events happen such as when the trigger box is triggered.")]
        public bool debugTriggerBox;

        /// <summary>
        /// If this is true, the script won't perform checks when the scene is run to notify you if you're missing any required references.
        /// </summary>
        [Tooltip("If this is true, you won't see any warnings in the editor when you're missing references or if there's something which could cause an error.")]
        public bool hideWarnings;

        /// <summary>
        /// If true, the entry check on the trigger box will be disabled, meaning it will go straight to the condition checking instead of waiting for something to enter the trigger box.
        /// </summary>
        [Tooltip("If true, the entry check on the trigger box will be disabled, meaning it will go straight to the condition checking instead of waiting for something to enter the trigger box.")]
        public bool disableEntryCheck;

        /// <summary>
        /// Only gameobjects with tags listed here are able to trigger the trigger box. To have more than one tag, put a comma between them. If you leave this field blank any object will be able to trigger it.
        /// </summary>
        [Tooltip("Only gameobjects with tags listed here are able to trigger the trigger box. To have more than one tag, put a comma between them. If you leave this field blank any object will be able to trigger it.")]
        public string triggerTags;

        /// <summary>
        /// The colour of the trigger box
        /// </summary>
        [Tooltip("This is the colour the trigger box and it's edges will have in the editor.")]
        public Color triggerboxColour;

        /// <summary>
        /// A set of options for when the trigger box has been trigged. Nothing does nothing. Trigger box destroys trigger box. Parent destroys parent.
        /// </summary>
        [Tooltip("This allows you to choose what happens to this gameobject after the trigger box has been triggered. Set Inactive will set this gameobject as inactive. Destroy trigger box will destroy this gameobject. Destroy parent will destroy this gameobject's parent. Do Nothing will mean the trigger box will stay active and continue to operate.")]
        public AfterTriggerOptions afterTrigger;

        /// <summary>
        /// An Enum allowing users to choose whether the trigger box follows the main camera or a selected transform
        /// </summary>
        [Tooltip("This allows you to choose if you want your trigger box to stay positioned on a moving transform or the main camera. If you pick Follow Transform a field will appear to set which transform you want the trigger box to follow. Or if you pick Follow Main Camera the trigger box will stay positioned on wherever the main camera currently is.")]
        public TriggerFollow triggerFollow;

        /// <summary>
        /// This is the time that the conditions must be met for in seconds.
        /// </summary>
        [Tooltip("This is the total time that the conditions must be met for in seconds before the responses get executed.")]
        public float conditionTime = 0f;

        /// <summary>
        /// This transform is used when trigger follow is set to transform. The trigger boxes position will be set to this transforms position every frame.
        /// </summary>
        [Tooltip("This is used when Trigger Follow is set to Follow Transform. The trigger box will stay positioned on wherever this transform is currently positioned.")]
        public Transform followTransform;

        /// <summary>
        /// This is used when a transform reference for followTransform is unavailable. An object with this named is searched for and used as the followTransform object
        /// </summary>
        [Tooltip("This is can be used if you cannot get a reference for the above Follow Transform transform. GameObject.Find() will be used to find the gameobject and transform with the name you enter.")]
        public string followTransformName;

        /// <summary>
        /// If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.
        /// </summary>
        [Tooltip("If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.")]
        public bool canWander;

        /// <summary>
        /// This is set to true when the trigger box is triggered. Once this is true we can start checking if the conditions have been met.
        /// </summary>
        private bool triggered = false;

        /// <summary>
        /// This is set to true when all the conditions have been met.
        /// </summary>
        private bool conditionMet = false;

        /// <summary>
        /// This is a timer used to make sure the time the condition has been met for is longer than conditionTime
        /// </summary>
        private float conditionTimer = 0f;

        #endregion

        #region Enums

        /// <summary>
        /// Options that are available to the user after the trigger box has been triggered.
        /// </summary>
        public enum AfterTriggerOptions
        {
            SetInactive,
            DestroyTriggerBox,
            DestroyParent,
            DoNothing,
        }

        /// <summary>
        /// The user can select if they want the trigger box to follow an object or the main camera.
        /// </summary>
        public enum TriggerFollow
        {
            Static,
            FollowMainCamera,
            FollowTransform,
        }

        #endregion

        /// <summary>
        /// Draws the inspector GUI
        /// </summary>
        public void OnInspectorGUI()
        {
            // Draw a horizontal line
            EditorGUI.indentLevel = 0;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

            // This removes items in the list which have been deleted. This can't be done in the loop and has to be done before GUI drawing starts
            // TODO: Fix undo component deletion
            conditions.RemoveAll(r => r == null);

            if (conditions.Count > 0)
            {
                // Display all the conditions
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditions[i].deleted)
                    {
                        // If the condition has been deleted we will destroy it
                        Undo.DestroyObjectImmediate(conditions[i]);
                    }
                    else
                    {
                        conditions[i].showWarnings = !hideWarnings;
                        conditions[i].OnInspectorGUI();

                        GUILayout.Space(10.0f);
                    }
                }
            }

            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel = 0;

            // Retrieve all the components that have inherited condition component
            string[] allConditions = GetComponents(true);

            // Draw the drop down list GUI item that displays all of the conditions
            int conditionIndex = EditorGUILayout.Popup("Add a new condition: ", 0, allConditions);

            if (EditorGUI.EndChangeCheck())
            {
                // If the user has selected an item in the drop down list
                if (conditionIndex != 0)
                {
                    // Determine the type of the component that needs to be added
                    Type conType = Type.GetType("EnhancedTriggerbox.Component." + allConditions[conditionIndex].Replace(" ", "").ToString());

                    // If we couldn't find the component, write to the console saying it wasn't found
                    if (conType == null)
                    {
                        Debug.Log("Unable to find the condition " + allConditions[conditionIndex].Replace(" ", "").ToString() + ". Make sure it has the EnhancedTriggerbox.Component namespace.");
                    }
                    else
                    {
                        // Create a new instance of this component and register an undo operation so that the user can undo adding this component
                        EnhancedTriggerBoxComponent obj = Undo.AddComponent(gameObject, conType) as EnhancedTriggerBoxComponent;
                        obj.hideFlags = HideFlags.HideInInspector;

                        conditions.Add(obj);
                    }

                    // Reset the drop down list
                    conditionIndex = 0;
                }
            }

            // The below code is identical to the above, just conditions has changed to responses

            EditorGUI.indentLevel = 0;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

            responses.RemoveAll(r => r == null);

            if (responses.Count > 0)
            {
                for (int i = 0; i < responses.Count; i++)
                {
                    if (responses[i].deleted)
                    {
                        Undo.DestroyObjectImmediate(responses[i]);
                    }
                    else
                    {
                        responses[i].showWarnings = !hideWarnings;
                        responses[i].OnInspectorGUI();

                        GUILayout.Space(10.0f);
                    }
                }
            }

            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel = 0;

            string[] allResponses = GetComponents(false);

            int responseIndex = EditorGUILayout.Popup("Add a new response: ", 0, allResponses);

            if (EditorGUI.EndChangeCheck())
            {
                if (responseIndex != 0)
                {
                    Type conType = Type.GetType("EnhancedTriggerbox.Component." + allResponses[responseIndex].Replace(" ", "").ToString());

                    if (conType == null)
                    {
                        Debug.Log("Unable to find the response " + allResponses[responseIndex].Replace(" ", "").ToString() + ". Make sure it has the EnhancedTriggerbox.Component namespace.");
                    }
                    else
                    {
                        EnhancedTriggerBoxComponent obj = Undo.AddComponent(gameObject, conType) as EnhancedTriggerBoxComponent;
                        obj.hideFlags = HideFlags.HideInInspector;

                        responses.Add(obj);
                    }

                    responseIndex = 0;
                }
            }
        }

        /// <summary>
        /// Called when the game is first started
        /// </summary>
        void Start()
        {
            // If a name of an object is entered then we will find that object and map it to followTransform
            if (!string.IsNullOrEmpty(followTransformName) && !disableEntryCheck)
            {
                try
                {
                    followTransform = GameObject.Find(followTransformName).transform;
                }
                catch
                {
                    Debug.Log("Unable to find game object" + followTransformName + " for Trigger Follow. Reverting to follow main camera.");
                    triggerFollow = TriggerFollow.FollowMainCamera;
                }
            }

            if (disableEntryCheck)
            {
                triggered = true;
            }

            // Do all the OnAwake functions for conditions/responses
            for (int i = 0; i < conditions.Count; i++)
            {
                conditions[i].OnAwake();
            }

            for (int i = 0; i < responses.Count; i++)
            {
                responses[i].OnAwake();
            }
        }

        /// <summary>
        /// Update loop called every frame
        /// </summary>
        void FixedUpdate()
        {
            if (!disableEntryCheck)
            {
                // This if statement updates the trigger boxes position to either stay on a transform or on the main camera
                if (triggerFollow == TriggerFollow.FollowTransform)
                {
                    transform.position = followTransform.position;
                }
                else if (triggerFollow == TriggerFollow.FollowMainCamera)
                {
                    transform.position = Camera.main.transform.position;
                }
            }

            // If the player has entered the trigger box
            if (triggered)
            {
                conditionMet = true;

                // Loop through each condition to check if it has been met
                for (int i = 0; i < conditions.Count; i++)
                {
                    conditionMet = conditions[i].ExecuteAction();

                    // If one has failed we don't need to check the rest so break out of the loop
                    if (!conditionMet)
                    {
                        break;
                    }
                }

                // If all have conditions have been met we must check that they have been met for longer than the specified conditionTime
                if (conditionMet && CheckConditionTimer())
                {
                    ConditionsMet();
                }
            }
        }

        /// <summary>
        /// This function executes all the responses and only happens after all the conditions have been met
        /// </summary>
        private void ConditionsMet()
        {
            // Execute every response
            for (int i = 0; i < responses.Count; i++)
            {
                responses[i].ExecuteAction();
            }

            // If debugTriggerBox is selected, write to the console saying the trigger box has successfully been triggered
            if (debugTriggerBox)
            {
                Debug.Log(gameObject.name + " has been triggered!");
            }

            // Depending on the selected option either set this as inactive, destroy it or destroy its parent
            switch (afterTrigger)
            {
                case AfterTriggerOptions.SetInactive:
                    gameObject.SetActive(false);
                    break;

                case AfterTriggerOptions.DestroyTriggerBox:
                    Destroy(gameObject);
                    break;

                case AfterTriggerOptions.DestroyParent:
                    Destroy(transform.parent.gameObject);
                    break;
            }
        }

        /// <summary>
        /// Called when this box intersects with another gameobject that has one of the specified triggerTags
        /// </summary>
        /// <param name="other">The collider that this object has collided with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!disableEntryCheck)
            {
                if ((triggerTags.Split(',').Contains(other.gameObject.tag)) || string.IsNullOrEmpty(triggerTags))
                {
                    triggered = true;
                }
            }
        }

        /// <summary>
        /// Called when a collider exits this collider. Only serves a purpose if the player cannot wander as it forces the condition checks to stop until the user re-enters the trigger box.
        /// </summary>
        /// <param name="other">The collider leaving this collider</param>
        private void OnTriggerExit(Collider other)
        {
            if (!canWander && !disableEntryCheck)
            {
                if ((triggerTags.Split(',').Contains(other.gameObject.tag)) || string.IsNullOrEmpty(triggerTags))
                {
                    triggered = false;
                }
            }
        }

        /// <summary>
        /// Draws a visual representation of the trigger box in the editor
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = triggerboxColour;

            if (!disableEntryCheck)
            {
                Gizmos.DrawCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                                new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z));
            }
        }

        /// <summary>
        /// This function checks to make sure the conditions have been met for a certain amount of time.
        /// </summary>
        /// <returns>Returns true or false depending on if the conditions have been met for a certain about of time.</returns>
        private bool CheckConditionTimer()
        {
            if (conditionTimer >= conditionTime)
            {
                return true;
            }
            else
            {
                conditionTimer += Time.fixedDeltaTime;
            }

            return false;
        }

        /// <summary>
        /// Uses reflection to find all instances of condition/response components and returns them as an array of strings.
        /// </summary>
        /// <param name="conditions">If true find components, false find responses.</param>
        /// <returns>Array of component names</returns>
        private string[] GetComponents(bool conditions)
        {
            string[] listOfComponents;

            if (conditions)
            {
                listOfComponents = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache)
                                    from assemblyType in domainAssembly.GetTypes()
                                    where typeof(ConditionComponent).IsAssignableFrom(assemblyType) && assemblyType.Name != "ConditionComponent"
                                    select AddSpacesToSentence(assemblyType.Name, true)).ToArray();
            }
            else
            {
                listOfComponents = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache)
                                    from assemblyType in domainAssembly.GetTypes()
                                    where typeof(ResponseComponent).IsAssignableFrom(assemblyType) && assemblyType.Name != "ResponseComponent"
                                    select AddSpacesToSentence(assemblyType.Name, true)).ToArray();
            }

            string[] newArray = new string[listOfComponents.Length + 1];
            listOfComponents.CopyTo(newArray, 1);
            newArray[0] = (conditions) ? "Select A Condition" : "Select A Response";

            return newArray;
        }

        /// <summary>
        /// Adds spaces before captial letters in a string
        /// </summary>
        /// <param name="text">The string to manipulate</param>
        /// <param name="preserveAcronyms">Should acronyms be unaffected?</param>
        /// <returns></returns>
        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        newText.Append(' ');
                    } 
                }

                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}