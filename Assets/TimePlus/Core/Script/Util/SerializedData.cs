// SerializedData.cs
// Author: Tedo Pranowo
// This file contains SerializedData class. This class is used to serialize a class as a string.
// The main usage of this class is to keep track of the class type stored inside base class
// variables

using UnityEngine;
using System.Reflection;

namespace TimePlus
{
    [System.Serializable]
    public class SerializedData
    {
        [SerializeField] private string m_serializedData;

        public void AddInt(int data)
        {
            m_serializedData += data.ToString() + '|';
        }

        public void AddFloat(float data)
        {
            m_serializedData += data.ToString() + '|';
        }

        public void AddString(string data)
        {
            m_serializedData += data + '|';
        }

        public void Save(object obj)
        {
            if (obj == null)
                return;

            //Get the obj type
            System.Type objType = obj.GetType();

            //If it is a class, store the data type of that class
            if (objType.IsClass)
                m_serializedData += objType.ToString() + '|';

            //Get all the object's field to be copied
            FieldInfo[] objFieldInfos = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo fieldInfo in objFieldInfos)
            {
                object value = fieldInfo.GetValue(obj);
                System.Type fieldType = fieldInfo.FieldType;

                if (value is int || value is float || value is string)
                {
                    m_serializedData += value.ToString() + '|';
                }
                else if (fieldType.IsEnum)
                {
                    m_serializedData += ((int)value).ToString() + '|';
                }
                else if (fieldType.IsClass)
                {
                    Save(value);
                }
            }
        }

        public void Load<T>(out T obj)
        {
            string[] data = GetData();
            Load(out obj, ref data, 0);
        }

        private void Load<T>(out T obj, ref string[] data, int index)
        {
            //Get the obj type
            System.Type objType = System.Type.GetType(data[index]);
            ++index;

            //Don't do anything if the data type isn't correct
            if (!(objType.IsSubclassOf(typeof(T))))
            {
                obj = default(T);
                return;
            }

            //Create a new object of the type in the data
            obj = (T)System.Activator.CreateInstance(objType);

            //Get all the object's field to be filled
            FieldInfo[] objFieldInfos = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo fieldInfo in objFieldInfos)
            {
                System.Type fieldType = fieldInfo.FieldType;

                if (fieldType == typeof(string))
                {
                    fieldInfo.SetValue(obj, data[index]);
                    ++index;
                }
                else if (fieldType == typeof(float))
                {
                    fieldInfo.SetValue(obj, float.Parse(data[index]));
                    ++index;
                }
                else if (fieldType == typeof(int))
                {
                    fieldInfo.SetValue(obj, int.Parse(data[index]));
                    ++index;
                }
                else if (fieldType.IsEnum)
                {
                    fieldInfo.SetValue(obj, int.Parse(data[index]));
                    ++index;
                }
                else if (fieldType.IsClass)
                {
                    object fieldObj;
                    Load(out fieldObj, ref data, index);
                    fieldInfo.SetValue(obj, fieldObj);
                }
                else
                {
                    Debug.LogAssertion("Unhandled serialization data type");
                }
            }
        }

        public string[] GetData()
        {
            return m_serializedData.Split('|');
        }

        public void Reset()
        {
            m_serializedData = "";
        }

        public bool IsEmpty()
        {
            return m_serializedData.Length == 0 || m_serializedData == null;
        }
    }

}
