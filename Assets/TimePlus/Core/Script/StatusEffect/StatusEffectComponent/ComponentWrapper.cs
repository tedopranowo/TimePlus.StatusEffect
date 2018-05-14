// ComponentWrapper.cs
// Author: Tedo Pranowo
// This file contains ComponentWrapper, a wrapper used for classes inheritting from 
// StatusEffectComponent used to maintain a good interface in Unity inspector and serializing the 
// data

using UnityEngine;

namespace TimePlus
{
    //NOTE:
    //- Why create an almost empty class from a template?
    //  See: https://forum.unity.com/threads/custompropertydrawer-for-a-class-with-a-generic-type.172013/
    [System.Serializable]
    public class SECEventWrapper : ComponentWrapper<SECEvent>
    {
        public override string ToString()
        {
            return (m_content != null) ? m_content.ToString() : "Event";
        }
    }

    [System.Serializable]
    public class SECActionWrapper : ComponentWrapper<SECAction>
    {
        public override string ToString()
        {
            return (m_content != null) ? m_content.ToString() : "Action";
        }
    }

    [System.Serializable]
    public class ComponentWrapper<T> : ISerializationCallbackReceiver where T : StatusEffectComponent
    {
        #region fields=============================================================================
        /// <summary>
        /// The object being wrapped by this class
        /// </summary>
        protected T m_content;

        /// <summary>
        /// Handles serializing the data from m_component
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private SerializedData m_contentSerialized = null;
        #endregion

        #region properties=========================================================================
        /// <summary>
        /// The content stored by the wrapper
        /// </summary>
        public T content
        {
            set { m_content = value; }
            get { return m_content; }
        }
        #endregion

        #region ISerializationCallbackReceiver=====================================================
        public void OnBeforeSerialize()
        {
            m_contentSerialized.Reset();

            m_contentSerialized.Save(m_content);
        }

        public void OnAfterDeserialize()
        {
            if (!m_contentSerialized.IsEmpty())
            {
                m_contentSerialized.Load(out m_content);
            }
        }
        #endregion
    }
}
