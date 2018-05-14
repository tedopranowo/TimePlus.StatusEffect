// StatusEffect.cs
// Author: Tedo Pranowo
// This file contains Status Effect class. This class defines all the information on what the status effect
// is and how it should behave

using UnityEngine;

namespace TimePlus
{
    using Extension;

    [CreateAssetMenu(fileName = "New Status Effect", menuName = "Time Plus/Status Effect", order = 1)]
    public class StatusEffect : ScriptableObject
    {
        #region fields=============================================================================
        [SerializeField] private string m_description = "";
        [SerializeField] private Sprite m_icon = null;
        [SerializeField] private float m_duration = 0.0f;
        [SerializeField] private Effect[] m_effects = null;

        private int m_originalId = -1;
        private float m_endTime;
        #endregion

        #region properties=========================================================================
        public string description
        {
            get
            {
                return m_description;
            }
        }

        public Sprite icon
        {
            get
            {
                return m_icon;
            }
        }

        public int originalId
        {
            get
            {
                return m_originalId;
            }
        }
        #endregion

        #region methods============================================================================
        public void OnApplied(StatusEffectHandler handler)
        {
            m_endTime = Time.time + m_duration;

            foreach (Effect effect in m_effects)
                effect.OnApplied(handler.gameObject);
        }

        public void OnTick(StatusEffectHandler handler)
        {
            foreach (Effect effect in m_effects)
                effect.OnTick(handler.gameObject);

            //Remove the effect if the duration is over
            if (m_endTime < Time.time)
                handler.Remove(this);
        }

        public void OnRemoved(StatusEffectHandler handler)
        {
            foreach (Effect effect in m_effects)
                effect.OnRemoved(handler.gameObject);
        }

        public void RefreshDuration()
        {
            m_endTime = Time.time + m_duration;
        }

        public void CopyFrom(StatusEffect copySource)
        {
            m_description = copySource.m_description;
            m_icon = copySource.m_icon;
            m_duration = copySource.m_duration;
            m_originalId = copySource.m_originalId;
            if (m_originalId == -1)
                m_originalId = copySource.GetInstanceID();
            name = copySource.name;

            m_effects = new Effect[copySource.m_effects.Length];
            for (int i = 0; i < copySource.m_effects.Length; ++i)
            {
                m_effects[i] = copySource.m_effects[i].DeepCopy() as Effect;
                Debug.Assert(m_effects[i] != null, "Unexpected data type");
            }
        }
        #endregion
    }
}
