using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EnhancedTriggerbox.Component
{
    /* Remember this component won't be included in the drop down lists until it inherits either ConditionComponent or ResponseComponent.
     * In this case you will need to replace EnhancedTriggerBoxComponent below with either of those two.
     */
    [AddComponentMenu("")]
    public class NewComponentExample 
        : EnhancedTriggerBoxComponent // REPLACE THIS!!!
    {
        /* 
         * Here you can declare your variables or structs or anything you want, just like a normal script.
         */
        public GameObject exampleGameobject;

        public bool exampleBool;

        public int exampleInt;

        /*
         * You should override DrawInspectorGUI() and add write the code that draws your variables in the inspector as shown below.
         */
        public override void DrawInspectorGUI()
        {
#if UNITY_EDITOR

            /*  Here's how you would draw a gameobject to a inspector. EditorGUILayout.ObjectField is the typical object reference field you
             *  always see in Unity. It returns the object which we will need to save as exampleGameObject so we do exampleGameObject = ObjectField.
             *  Notice the (GameObject) before EditorGUILayout? This is because the ObjectField returns a object not a GameObject so we must 
             *  explicitly convert it to a GameObject. 
             *   
             *  For the first bit of object field we'll create a new GUIContent which will hold the field name (label before the field)
             *  and field tooltip (text that is displayed on hover). Example Game Object is this fields name and Example tooltip is this fields
             *  tooltip. After that we pass in exampleGameObject again as the ObjectField needs the current object stored in there so it is
             *  displayed correctly. Then we set the type which is in this case gameobject. The final 'true' allows the user to use gameobjects
             *  currently in the scene which we want so set it to true.
             */
            exampleGameobject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Example Game Object",
                    "Example tooltip"), exampleGameobject, typeof(GameObject), true);

            /*
             * Now we will draw exampleBool to the inspector. Same again, exampleBool = EditorGUILayout.Toggle(). This time we don't need to write
             * (bool) before it because it already returns a bool. We also don't need to specify the type as this field only allows bools.
             */
            exampleBool = EditorGUILayout.Toggle(new GUIContent("Example Bool",
                    "Example tooltip"), exampleBool);

            exampleInt = EditorGUILayout.IntField(new GUIContent("Example Int",
                    "Example tooltip"), exampleInt);

            /*
             * Other common GUI fields:
             * String - EditorGUILayout.TextField()
             * Floats - EditorGUILayout.FloatField()
             * Enums - (EnumName)EditorGUILayout.EnumPopup()
             */

#endif
        }

        /*
         * If this component is a condition this function is called when the trigger box is triggered (player enters it) and must returns true or
         * false depending on if the condition has been met. If this component is a response then this function is called when all conditions
         * have been met can and returns true or false depending on if the response has executed correctly. This function must be overriden by 
         * each inheirited component.
         */
        public override bool ExecuteAction()
        {
            /* 
             * For conditions you should return true once your condition has been met and false when it hasn't. For example:
             * if (exampleInt > 5)
             *   return true;
             * else
             *   return false;
             *   
             *   For responses you should return true after your response has been executed. For example:
             *   exampleInt = 0;
             *   return true;
             */

            /* Remove this line */
            return base.ExecuteAction();
        }

        /*
         * This function is optional and will be called when the game starts. You can place whatever you want in here and it will be executed when the
         * game starts or when the Enhanced Trigger Box gets initalised. The most common uses will be for caching components or objects or getting
         * updated values.
         */
        public override void OnAwake()
        {

        }

        /*
         * This function is optional and is called in edit mode after the Enhanced Trigger Box GUI gets drawn. You should use it for displaying warnings
         * about your component such as missing references or invalid values. 
         */
        public override void Validation()
        {
            /* This checks if exampleGameObject is null. If it is null we should warn the user about it because it might cause errors. */
            if (!exampleGameobject)
            {
                /* You should use ShowWarningMessage to display your warnings as this will take into account if the user has disabled
                 * warnings or not.
                 */
                ShowWarningMessage("WARNING: You haven't assigned an object to exampleGameobject.");
            }
        }
    }
}
