using EnhancedTriggerbox.Component;
using System;
using System.Linq;
using System.Text;

namespace EnhancedTriggerbox
{
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

        private static volatile ComponentList instance;
        private static object syncRoot = new Object();

        private ComponentList() { }

        public static ComponentList Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ComponentList();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Uses reflection to find all instances of condition/response components and returns them as an array of strings.
        /// </summary>
        public void GetComponents()
        {
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


