// EditorHelper.cs
// Author: Tedo Pranowo
// This file contains useful functions often helpful when dealing with UnityEditor

using System.Collections;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace TimePlus
{
    public static class EditorHelper
    {
        public static object GetObjectFromSerializedProperty(SerializedProperty property)
        {
            //Get the serialized object
            object curObject = property.serializedObject.targetObject;

            //Get the path
            string path = property.propertyPath;

            //Convert path to code path
            path = path.Replace(".Array.data", "");

            //Split by '.'
            string[] elements = path.Split('.');

            //Iterate through each element
            foreach (string element in elements)
            {
                string elementName = element;
                int index = -1;

                //If an open bracket is found, separate the element name from the index
                int openBracketIndex = element.IndexOf("[");
                if (openBracketIndex != -1)
                {
                    elementName = element.Substring(0, openBracketIndex);
                    index = System.Convert.ToInt32(element.Substring(openBracketIndex).Replace("[", "").Replace("]", ""));
                }

                //Get the field with specified element name
                var field = curObject.GetType().GetField(elementName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                curObject = field.GetValue(curObject);

                //If the curObject is null, return null
                if (curObject == null)
                    return null;

                //Get the indexed element if it exist
                if (index != -1)
                {
                    IEnumerable enumerable = curObject as IEnumerable;
                    IEnumerator enumerator = enumerable.GetEnumerator();

                    for (int i = 0; i <= index; ++i)
                    {
                        if (!enumerator.MoveNext())
                            return null;
                    }

                    curObject = enumerator.Current;
                }
            }

            return curObject;

        }

        public static void DrawCustomDefaultInspector(object obj)
        {
            //Convert the concrete type
            System.Type csEventType = obj.GetType();
            FieldInfo[] csEventFieldInfo = csEventType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo fieldInfo in csEventFieldInfo)
            {
                string name = ParseFieldName(fieldInfo.Name);
                object value = fieldInfo.GetValue(obj);

                object[] myAttributes = fieldInfo.GetCustomAttributes(typeof(UnityEngine.SerializeField), true);

                //If the object is either public or has [SerializeField], draw it
                if (myAttributes.Length > 0 || fieldInfo.Attributes == FieldAttributes.Public)
                {
                    //Draw the inspector
                    if (value is int)
                    {
                        fieldInfo.SetValue(obj, EditorGUILayout.IntField(name, (int)value));
                    }
                    else if (value is float)
                    {
                        fieldInfo.SetValue(obj, EditorGUILayout.FloatField(name, (float)value));
                    }
                    else if (value.GetType().IsEnum)
                    {
                        fieldInfo.SetValue(obj, EditorGUILayout.EnumPopup(name, value as System.Enum));
                    }
                }
            }
        }

        public static string ParseFieldName(string name)
        {
            string parsedName = name;

            //Remove the m_
            if (name.StartsWith("m_"))
                parsedName = name.Substring(2);

            return name;
        }

        /// <summary>
        /// Return all class of type T or inherit from type T and is not an abstract class
        /// </summary>
        /// <param name="ignoreAbstract"> If this is true, abstract classes will be included in the result</param>
        public static System.Type[] GetSubtypesOfType<T>() where T : class
        {
            System.Type[] types = Assembly.GetAssembly(typeof(T)).GetTypes();
            if (types.Length == 0)
                return null;

            return Assembly.GetAssembly(typeof(T)).GetTypes().Where(curType => !curType.IsAbstract && curType.IsSubclassOf(typeof(T))).ToArray();
        }
    }
}