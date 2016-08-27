using EnhancedTriggerbox.Component;
using System;
using System.Linq;

namespace EnhancedTriggerbox
{
    /// <summary>
    /// Singleton that holds an array of condition names and condition responses
    /// </summary>
    public class ComponentList
    {
        /// <summary>
        /// Cached names of all the conditions. Used in the add condition drop down list.
        /// </summary>
        public string[] conditionNames;

        /// <summary>
        /// Cached names of all the respones. Used in the add response drop down list.
        /// </summary>
        public string[] responseNames;

        /// <summary>
        /// The component list instance that holds the arrays
        /// </summary>
        private static ComponentList instance;

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static ComponentList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentList();
                }

                return instance;
            }
        }

        /// <summary>
        /// The constructor that uses reflection to find all instances of condition/response components and returns them as an array of strings.
        /// </summary>
        private ComponentList()
        {
            // Get all the names of the assemblies which inherit ConditionComponent
            string[] listOfComponents = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache)
                                         from assemblyType in domainAssembly.GetTypes()
                                         where typeof(ConditionComponent).IsAssignableFrom(assemblyType) && assemblyType.Name != "ConditionComponent"
                                         select EnhancedTriggerBox.AddSpacesToSentence(assemblyType.Name, true)).ToArray();

            // Add the Select A Condition/Response items to the list so they're displayed at the top. Is this the best way to do this?
            string[] newArray = new string[listOfComponents.Length + 1];
            listOfComponents.CopyTo(newArray, 1);
            newArray[0] = "Select A Condition";
            conditionNames = newArray;

            listOfComponents = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GlobalAssemblyCache)
                                from assemblyType in domainAssembly.GetTypes()
                                where typeof(ResponseComponent).IsAssignableFrom(assemblyType) && assemblyType.Name != "ResponseComponent"
                                select EnhancedTriggerBox.AddSpacesToSentence(assemblyType.Name, true)).ToArray();

            newArray = new string[listOfComponents.Length + 1];
            listOfComponents.CopyTo(newArray, 1);
            newArray[0] = "Select A Response";
            responseNames = newArray;
        }
    }
}


