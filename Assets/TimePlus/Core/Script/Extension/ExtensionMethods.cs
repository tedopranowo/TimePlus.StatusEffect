// ExtensionMethods.cs
// Author: Tedo Pranowo
// This file contains extension methods for classes.
// Extension method Reference: https://unity3d.com/learn/tutorials/topics/scripting/extension-methods

using UnityEngine;
using System.Reflection;

namespace TimePlus.Extension
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// This function perform a deep copy on an object. This function shouldn't be called every frame because
        /// it is slow due to the usage of system.reflection
        /// </summary>
        /// <param name="obj"> The object to be copied </param>
        /// <returns> The copy result </returns>
        public static object DeepCopy(this object obj)
        {
            //Get the object type
            System.Type objType = obj.GetType();

            //Create a new object of the same type
            object clone = System.Activator.CreateInstance(objType);

            //Get all the object's field to be copied
            FieldInfo[] objFieldInfos = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo fieldInfo in objFieldInfos)
            {
                object value = fieldInfo.GetValue(obj);
                System.Type fieldType = fieldInfo.FieldType;

                if (fieldType.IsPrimitive || fieldType.IsEnum || fieldType == typeof(string) || fieldType.IsArray)
                {
                    fieldInfo.SetValue(clone, value);
                }
                else if (fieldType.IsClass)
                {
                    //Perform deep copy
                    fieldInfo.SetValue(clone, DeepCopy(value));
                }
                else
                {
                    Debug.LogAssertion("Unhandled deep copy case");
                }
            }
            return clone;
        }
    }
}