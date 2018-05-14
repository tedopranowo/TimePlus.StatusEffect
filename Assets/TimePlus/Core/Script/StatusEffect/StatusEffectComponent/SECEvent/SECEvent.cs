// SECEvent.cs
// Author: Tedo Pranowo
// This file contains the base class inherited by status effect's event component

namespace TimePlus
{
    [System.Serializable]
    public abstract class SECEvent : StatusEffectComponent
    {
        public virtual bool ShouldTriggerOnApplied()
        {
            return false;
        }

        public virtual bool ShouldTriggerNow()
        {
            return false;
        }

        public virtual bool ShouldTriggerOnRemoved()
        {
            return false;
        }
    }
}