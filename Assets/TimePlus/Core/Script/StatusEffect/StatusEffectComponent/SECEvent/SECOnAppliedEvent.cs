// SECOnAppliedEvent.cs
// Author: Tedo Pranowo
// This file contains a class defining that an action should be triggered when the status effect
// is applied

namespace TimePlus
{
    public class SECOnAppliedEvent : SECEvent
    {
        public override bool ShouldTriggerOnApplied()
        {
            return true;
        }

        public override string ToString()
        {
            return "When buff is applied";
        }
    }
}