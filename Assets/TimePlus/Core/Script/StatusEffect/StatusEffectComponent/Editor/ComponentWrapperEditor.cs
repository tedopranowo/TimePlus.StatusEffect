// ComponentWrapperEditor.cs
// Author: Tedo Pranowo
// This file contains Unity Editor's code for ComponentWrapper class

using UnityEngine;
using UnityEditor;
using System;

namespace TimePlus
{
    //NOTE:
    //- Why create empty classes from a template?
    //  See: https://forum.unity.com/threads/custompropertydrawer-for-a-class-with-a-generic-type.172013/
    [CustomPropertyDrawer(typeof(SECActionWrapper))]
    public class SECActionWrapperDrawer : ComponentWrapperDrawer<SECAction, SECActionWrapperWindow> { }

    [CustomPropertyDrawer(typeof(SECEventWrapper))]
    public class SECEventWrapperDrawer : ComponentWrapperDrawer<SECEvent, SECEventWrapperWindow> { }

    public class SECEventWrapperWindow : ComponentWrapperWindow<SECEvent> { }

    public class SECActionWrapperWindow : ComponentWrapperWindow<SECAction> { }

    /// <summary>
    /// This class draws a button on the inspector which will open up a window when pressed
    /// </summary>
    /// <typeparam name="SEC"> The status effect component class type to be drawn </typeparam>
    /// <typeparam name="Window"> The window to be spawned when the button is pressed </typeparam>
    public class ComponentWrapperDrawer<SEC, Window> : PropertyDrawer 
        where SEC : StatusEffectComponent 
        where Window : ComponentWrapperWindow<SEC>
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ComponentWrapper<SEC> componentWrapper = EditorHelper.GetObjectFromSerializedProperty(property) as ComponentWrapper<SEC>;

            if (componentWrapper == null)
                return;

            if (GUI.Button(position, componentWrapper.ToString()))
            {
                //ComponentWrapperWindow<SECEvent> window = EditorWindow.CreateInstance<ComponentWrapperWindow<SECEvent>>();
                Window window = EditorWindow.GetWindow(typeof(Window)) as Window;

                //Set the window object reference
                window.componentWrapper = componentWrapper;
                window.serializedObject = property.serializedObject;
                window.Init();
                window.Show();
            }
        }
    }

    /// <summary>
    /// This class draws the content of the window which consist of 2 part:
    /// 1. An dropdown to change the class type
    /// 2. Default inspector UI for the selected class type
    /// </summary>
    /// <typeparam name="T"> The status effect component class type to be drawn </typeparam>
    public class ComponentWrapperWindow<T> : EditorWindow where T : StatusEffectComponent
    {
        #region fields=============================================================================
        private ComponentWrapper<T> m_componentWrapper;
        private SerializedObject m_serializedObject;

        private Type[] m_subtypes;
        private int m_selectedIndex;
        #endregion

        #region properties=========================================================================
        public ComponentWrapper<T> componentWrapper
        {
            set { m_componentWrapper = value; }
            get { return m_componentWrapper; }
        }

        public SerializedObject serializedObject
        {
            set { m_serializedObject = value; }
            get { return m_serializedObject; }
        }
        #endregion

        #region methods============================================================================
        /// <summary>
        /// Custom Init function to be called by ComponentWrapperDrawer<>. We don't want to use Awake() or
        /// OnEnable() because we want m_actionWrapper to be set before calling this function
        /// </summary>
        public void Init()
        {
            //Initialize all possible classes
            m_subtypes = EditorHelper.GetSubtypesOfType<T>();

            //Determine which subtype is currently selected
            T component = m_componentWrapper.content;
            Type selectedSubtype = (component != null) ? m_componentWrapper.content.GetType() : null;
            m_selectedIndex = Array.FindIndex(m_subtypes, type => type.Equals(selectedSubtype));
        }
        #endregion

        #region EditorWindow=======================================================================
        private void OnGUI()
        {
            //Convert all subtypes to string
            string[] subtypesStr = Array.ConvertAll(m_subtypes, type => type.ToString());

            //Show the subtypes in insepctor as popup
            int newIndex = EditorGUILayout.Popup("Type: ", m_selectedIndex, subtypesStr);

            //If a new subtype is selected, set it as the new content
            if (newIndex != m_selectedIndex)
            {
                m_selectedIndex = newIndex;
                m_componentWrapper.content = (T)Activator.CreateInstance(m_subtypes[m_selectedIndex]); 
            }

            //Draw the default inspector for the selected subtype
            if (m_selectedIndex != -1)
                EditorHelper.DrawCustomDefaultInspector(m_componentWrapper.content);

            //Draw a save button
            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(m_serializedObject.targetObject);
                AssetDatabase.SaveAssets();
            }
        }
        #endregion
    }

}
