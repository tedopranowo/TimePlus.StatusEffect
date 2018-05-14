// SECOnRemovedEvent.cs
// Author: Tedo Pranowo
// This file contains a class defining that an action should be triggered when the status effect is
// removed

namespace TimePlus
{
    public class SECOnRemovedEvent : SECEvent
    {
        public override bool ShouldTriggerOnRemoved()
        {
            return true;
        }

        public override string ToString()
        {
            return "When buff is removed";
        }
    }
}