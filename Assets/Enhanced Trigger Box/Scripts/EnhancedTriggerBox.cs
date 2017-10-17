using EnhancedTriggerbox.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EnhancedTriggerbox
{
    [Serializable]
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
#if UNITY_5_4_OR_NEWER
    [HelpURL("https://alex-scott.co.uk/portfolio/enhanced-trigger-box.html")]
#endif
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
        /// Holds all the exit responses the user has added to the trigger box
        /// </summary>
        [SerializeField]
        private List<EnhancedTriggerBoxComponent> exitResponses = new List<EnhancedTriggerBoxComponent>();

        /// <summary>
        /// The getter that will return all the conditions. Used when iterating through all the conditions.
        /// </summary>
        public List<EnhancedTriggerBoxComponent> Conditions
        {
            get { return conditions; }
        }

        /// <summary>
        /// The getter that will return all the responses. Used when iterating through all the responses.
        /// </summary>
        public List<EnhancedTriggerBoxComponent> Responses
        {
            get { return responses; }
        }

        /// <summary>
        /// The getter that will return all the exit responses. Used when iterating through all the exit responses.
        /// </summary>
        public List<EnhancedTriggerBoxComponent> ExitResponses
        {
            get { return exitResponses; }
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
        /// If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.
        /// </summary>
        [Tooltip("If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.")]
        public bool canWander;

        /// <summary>
        /// The colour of the trigger box
        /// </summary>
        [Tooltip("This is the colour the trigger box and it's edges will have in the editor.")]
        public Color triggerboxColour;

        /// <summary>
        /// A set of options for when the trigger box has been trigged. Nothing does nothing. Trigger box destroys trigger box. Parent destroys parent.
        /// </summary>
        [Tooltip("This allows you to choose what happens to this gameobject after the trigger box has been triggered. Set Inactive will set this gameobject as inactive. Destroy trigger box will destroy this gameobject. Destroy parent will destroy this gameobject's parent. Do Nothing will mean the trigger box will stay active and continue to operate. ExecuteExitResponses allows you to set up additional responses to be executed after the object that entered the trigger box leaves it.")]
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
        /// This is set to true when the trigger box is triggered. Once this is true we can start checking if the conditions have been met.
        /// </summary>
        private bool triggered = false;

        /// <summary>
        /// This is used when the ConditionsMet coroutine is executing to stop it accidently executing twice
        /// </summary>
        private bool responsesExecuting = false;

        /// <summary>
        /// This is a timer used to make sure the time the condition has been met for is longer than conditionTime
        /// </summary>
        private float conditionTimer = 0f;

        /// <summary>
        /// The gameobject that has collided with this trigger box. To use this data in ExecuteAction(), components must set requiresCollisionObjectData to true.
        /// </summary>
        private GameObject collidingObject;

        /// <summary>
        /// Used to cache the box collider so GetComponent doesn't have to be called multiple times a frame.
        /// </summary>
        private Collider etbCollider;

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
            ExecuteExitResponses,
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

        #region Inspector Drawing

        /// <summary>
        /// Draws the inspector GUI
        /// </summary>
        public void OnInspectorGUI()
        {
#if UNITY_EDITOR

            // Draw a horizontal line
            EditorGUI.indentLevel = 0;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

            if (conditions.Count > 0)
            {
                DisplayComponents(conditions);
            }

            DisplayAddComponent("Condition", ComponentList.Instance.conditionNames, conditions);

            EditorGUI.indentLevel = 0;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

            if (responses.Count > 0)
            {
                DisplayComponents(responses);
            }

            DisplayAddComponent("Response", ComponentList.Instance.responseNames, responses);

            if (afterTrigger == AfterTriggerOptions.ExecuteExitResponses)
            {
                EditorGUI.indentLevel = 0;
                EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

                if (exitResponses.Count > 0)
                {
                    DisplayComponents(exitResponses);
                }

                DisplayAddComponent("Exit Response", ComponentList.Instance.responseNames, exitResponses);
            }

#endif
        }

#if UNITY_EDITOR
        private void DisplayComponents(List<EnhancedTriggerBoxComponent> components)
        {
            // Display all the components
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i])
                {
                    if (components[i].deleted) // If the X button has been pressed in the top right corner of a component
                    {
                        components[i].deleted = false; // Set this to false to prevent a infinite loop when redoing
                        Undo.RecordObject(this, "Delete"); // Save the state of this object

                        EnhancedTriggerBoxComponent removeComponent = components[i];
                        components.RemoveAt(i); // Remove from the component list

                        Undo.DestroyObjectImmediate(removeComponent); // Destroy this object and record the operation
                    }
                    else
                    {
                        components[i].showWarnings = !hideWarnings;
                        components[i].OnInspectorGUI(); // Draw this component in the inspector

                        GUILayout.Space(10.0f);
                    }
                }
                else
                {
                    components.RemoveAt(i);
                }
            }
        }

        private void DisplayAddComponent(string componentType, string[] componentNames, List<EnhancedTriggerBoxComponent> componentList)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel = 0;

            int conditionIndex = 0;
            try
            {
                // Draw the drop down list GUI item that displays all of the components
                conditionIndex = EditorGUILayout.Popup("Add a new " + componentType + ": ", 0, componentNames);
            }
            catch
            {
                // I know this is very bad practice but I can't for the life of me work out what is causing
                // ArgumentException: Getting control 0's position in a group with only 0 controls when doing Repaint Aborting
                // but this does remove the error.
                return;
            } 
            
            if (EditorGUI.EndChangeCheck())
            {
                // If the user has selected an item in the drop down list
                if (conditionIndex != 0)
                {
                    // Determine the type of the component that needs to be added
                    Type conType = Type.GetType("EnhancedTriggerbox.Component." + componentNames[conditionIndex].Replace(" ", "").ToString());

                    // If we couldn't find the component, write to the console saying it wasn't found
                    if (conType == null)
                    {
                        Debug.Log("Unable to find the " + componentType + " " + componentNames[conditionIndex].Replace(" ", "").ToString() + ". Make sure it has the EnhancedTriggerbox.Component namespace.");
                    }
                    else
                    {
                        // Create a new instance of this component and register an undo operation so that the user can undo adding this component
                        EnhancedTriggerBoxComponent obj = Undo.AddComponent(gameObject, conType) as EnhancedTriggerBoxComponent;
                        obj.hideFlags = HideFlags.HideInInspector;

                        componentList.Insert(0, obj);
                    }

                    // Reset the drop down list
                    conditionIndex = 0;
                }
            }
        }
#endif

        #endregion

        /// <summary>
        /// Called when the game is first started
        /// </summary>
        void Start()
        { 
            if (disableEntryCheck) 
            {
                // If entry check is disabled, then triggered is true by default
                triggered = true;
            }
            else
            {
                // If a name of an object is entered then we will find that object and map it to followTransform
                if (!string.IsNullOrEmpty(followTransformName))
                {
                    try
                    {
                        followTransform = GameObject.Find(followTransformName).transform;
                    }
                    catch
                    {
                        Debug.Log("Unable to find game object" + followTransformName + " for Trigger Follow. Reverting to static.");
                        triggerFollow = TriggerFollow.Static;
                    }
                }
            }

            // Do all the OnAwake functions for conditions/responses
            for (int i = conditions.Count - 1; i >= 0; i--)
            {
                if (conditions[i])
                {
                    conditions[i].OnAwake();
                }
                else
                {
                    conditions.RemoveAt(i);
                }
            }

            for (int i = responses.Count - 1; i >= 0; i--)
            {
                if (responses[i])
                {
                    responses[i].OnAwake();
                }
                else
                {
                    responses.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Update loop called every frame
        /// </summary>
        void Update()
        {
            if (!disableEntryCheck)
            {
                // This switch statement updates the trigger boxes position to either stay on a transform or on the main camera
                switch (triggerFollow)
                {
                    case TriggerFollow.FollowTransform:
                        transform.position = followTransform.position;
                        break;

                    case TriggerFollow.FollowMainCamera:
                        transform.position = Camera.main.transform.position;
                        break;
                }
            }

            // If triggered is true (set when an object collides with this ETB) and the conditions haven't already been met and the responses aren't executing
            if (triggered && !responsesExecuting)
            {
                bool conditionMet = true; // Default this to true

                // Loop through each condition to check if it has been met
                for (int i = conditions.Count - 1; i >= 0; i--)
                {
                    if (conditions[i])
                    {
                        if (conditions[i].requiresCollisionObjectData)
                        {
                            conditionMet = conditions[i].ExecuteAction(collidingObject);
                        }
                        else
                        {
                            conditionMet = conditions[i].ExecuteAction();
                        }

                        // If one has failed we don't need to check the rest so break out of the loop
                        if (!conditionMet)
                        {
                            break;
                        }
                    }
                    else
                    {
                        conditions.RemoveAt(i);
                    }
                }

                // If all have conditions have been met we must check that they have been met for longer than the specified conditionTime
                if (conditionMet && CheckConditionTimer())
                {
                    StartCoroutine(ConditionsMet());
                }
            }
        }

        /// <summary>
        /// This function executes all the responses and only happens after all the conditions have been met
        /// </summary>
        private IEnumerator ConditionsMet()
        {
            responsesExecuting = true; // This is used so the coroutine doesn't get accidently triggered twice

            // If debugTriggerBox is selected, write to the console saying the trigger box has successfully been triggered
            if (debugTriggerBox)
            {
                Debug.Log(gameObject.name + " has been triggered!");
            }

            // There is the possibility of the exit responses still being executed. This ends them all
            if (afterTrigger == AfterTriggerOptions.ExecuteExitResponses)
            {
                for (int i = exitResponses.Count - 1; i >= 0; i--)
                {
                    exitResponses[i].EndComponentCoroutine();
                }
            }

            float waitTime = 0f;

            // Execute every response
            for (int i = responses.Count - 1; i >= 0; i--)
            {
                if (responses[i])
                {
                    if (responses[i].requiresCollisionObjectData)
                    {
                        responses[i].ExecuteAction(collidingObject);
                    }
                    else
                    {
                        responses[i].ExecuteAction();
                    }

                    // Find the longest response duration
                    if (responses[i].duration > waitTime)
                    {
                        waitTime = responses[i].duration;
                    }
                }
                else
                {
                    responses.RemoveAt(i);
                }
            }

            if (afterTrigger == AfterTriggerOptions.DestroyParent ||
                afterTrigger == AfterTriggerOptions.DestroyTriggerBox ||
                afterTrigger == AfterTriggerOptions.SetInactive)
            {
                // If one of the responses is using has a duration attribute then we'll need to wait for that amount of time before disabling/destroying the trigger box as that will cancel whatever the component is doing
                yield return new WaitForSeconds(waitTime);
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

                case AfterTriggerOptions.DoNothing: // If do nothing is set, simply reset the variables and all the components
                    conditionTimer = 0f;
                    responsesExecuting = false;

                    if (!disableEntryCheck)
                        triggered = false;

                    for (int i = 0; i < conditions.Count; i++)
                    {
                        conditions[i].ResetComponent();
                    }
                    for (int i = 0; i < responses.Count; i++)
                    {
                        responses[i].ResetComponent(); 
                    }
                    break;

                case AfterTriggerOptions.ExecuteExitResponses:
                    // Do nothing. Wait for the triggered object to leave the trigger box
                    break;
            }
        }

        private void OnExitResponses()
        {
            for (int i = responses.Count - 1; i >= 0; i--)
            {
                // Make sure all coroutines (or things that take time to execute) have been stopped
                responses[i].EndComponentCoroutine();
            }

            // Reset the components
            for (int i = responses.Count - 1; i >= 0; i--)
            {
                responses[i].ResetComponent(); 
            }
            for (int i = conditions.Count - 1; i >= 0; i--)
            {
                conditions[i].ResetComponent();
            }

            // Execute all the exit responses the same way as the normal responses
            for (int i = exitResponses.Count - 1; i >= 0; i--) 
            {
                if (exitResponses[i].requiresCollisionObjectData)
                {
                    exitResponses[i].ExecuteAction(collidingObject);
                }
                else
                {
                    exitResponses[i].ExecuteAction();
                }
            }

            responsesExecuting = false; // Essentially reset the trigger box
            triggered = false;
            conditionTimer = 0f;
        }

        /// <summary>
        /// Called when this box intersects with another gameobject that has one of the specified triggerTags
        /// </summary>
        /// <param name="other">The collider that this object has collided with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!disableEntryCheck)
            {
                if (string.IsNullOrEmpty(triggerTags) || (triggerTags.Split(',').Contains(other.gameObject.tag))) // If it has a valid tag
                {
                    triggered = true;
                    collidingObject = other.gameObject;
                }
            }
        }

        /// <summary>
        /// Called when a collider exits this collider
        /// </summary>
        /// <param name="other">The collider leaving this collider</param>
        private void OnTriggerExit(Collider other)
        {
            if (collidingObject == other.gameObject) // Only look for an exit from the same object that entered
            {
                if (afterTrigger == AfterTriggerOptions.ExecuteExitResponses) // If we have been told to do something on exit
                {
                    if (responsesExecuting) // If all the responses are executing/finished executing
                    {
                        OnExitResponses();
                    }
                }
                else // Else we need simply set triggered to false because the object has left the trigger box
                {
                    if (!canWander) // Unless the object is allowed to wander then leave it as true
                    {
                        triggered = false;
                    }
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
                if (etbCollider == null)
                {
                    etbCollider = GetComponent<Collider>(); // Cache the collider
                }

                Gizmos.DrawCube(new Vector3(etbCollider.bounds.center.x, etbCollider.bounds.center.y, etbCollider.bounds.center.z),
                                new Vector3(etbCollider.bounds.size.x, etbCollider.bounds.size.y, etbCollider.bounds.size.z));
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
                conditionTimer += Time.deltaTime;
            }

            return false;
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