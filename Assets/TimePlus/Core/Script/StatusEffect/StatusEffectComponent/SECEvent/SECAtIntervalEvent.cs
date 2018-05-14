// SECAtIntervalEvent.cs
// Author: Tedo Pranowo
// This effect contains a class defining that an action should be triggered every certain interval

using UnityEngine;

namespace TimePlus
{
    public class SECAtIntervalEvent : SECEvent
    {
        [SerializeField] private float m_interval = 1.0f;
        private float m_nextTrigger;

        public override void OnApplied()
        {
            m_nextTrigger = Time.time + m_interval;
        }

        public override bool ShouldTriggerNow()
        {
            if (m_nextTrigger < Time.time)
            {
                m_nextTrigger = m_nextTrigger + m_interval;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "Every " + m_interval + " seconds";
        }
    }
}