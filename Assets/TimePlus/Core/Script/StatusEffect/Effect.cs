// Effect.cs
// Author: Tedo Pranowo
// This file contains Effect class. This class hold an event and an action. The event defines
// when the action should be triggered. The action defines what should be done when it is triggered

using UnityEngine;

namespace TimePlus
{
    [System.Serializable]
    public class Effect
    {
        #region fields=============================================================================
        [SerializeField] private SECEventWrapper m_eventWrapper = null;
        [SerializeField] private SECActionWrapper m_actionWrapper = null;
        #endregion

        #region properties=========================================================================
        public SECEvent secEvent
        {
            get
            {
                return (m_eventWrapper != null) ? m_eventWrapper.content : null;
            }
        }

        public SECAction secAction
        {
            get
            {
                return (m_actionWrapper != null) ? m_actionWrapper.content : null;
            }
        }
        #endregion

        #region methods============================================================================
        public void OnApplied(GameObject owner)
        {
            secEvent.OnApplied();
            secAction.OnApplied();

            //Check if the action should be applied
            if (secEvent.ShouldTriggerOnApplied())
                secAction.ApplyAction(owner);
        }

        public void OnTick(GameObject owner)
        {
            //Check if the action should be applied
            if (secEvent.ShouldTriggerNow())
                secAction.ApplyAction(owner);
        }

        public void OnRemoved(GameObject owner)
        {
            secEvent.OnRemoved();
            secAction.OnRemoved();

            //Check if the action should be applied
            if (secEvent.ShouldTriggerOnRemoved())
                secAction.ApplyAction(owner);
        }
        #endregion
    }
}
