// StatusEffectHandler.cs
// Author: Tedo Pranowo
// This file contains StatusEffectHandler class. This class should be attached to any object that
// may receive status effect

using System.Collections.Generic;
using UnityEngine;

namespace TimePlus
{
    public class StatusEffectHandler : MonoBehaviour
    {
        #region fields=================================================================================
        /// <summary>
        /// Store all the status effects owned by this handler
        /// NOTE:
        /// - Why not use HashSet?
        ///     We don't actually store the original data to the container. Instead, we are storing
        ///     the copy of the original data. Using HashSet won't allow us to obtain the data stored
        ///     in the container using the original data
        /// </summary>
        private Dictionary<int, StatusEffect> m_statusEffects = new Dictionary<int, StatusEffect>();

        /// <summary>
        /// Store all the status effects reference that's going to be deleted
        /// </summary>
        private List<StatusEffect> m_toBeRemovedStatusEffects = new List<StatusEffect>();
        #endregion

        #region properties=============================================================================
        public StatusEffect[] statusEffects
        {
            get
            {
                StatusEffect[] statusEffectList = new StatusEffect[m_statusEffects.Count];
                m_statusEffects.Values.CopyTo(statusEffectList, 0);
                return statusEffectList;
            }
        }

        #endregion

        #region methods================================================================================
        /// <summary>
        /// Apply a status effect to the handler
        /// </summary>
        /// <param name="statusEffect"> The status effect to be applied </param>
        public void Apply(StatusEffect statusEffect)
        {
            int originalId = statusEffect.GetInstanceID();

            //If the player has that status effect already, refresh the duration
            if (m_statusEffects.ContainsKey(originalId))
            {
                //Refresh the duration of the status effect currently applied on the player
                m_statusEffects[originalId].RefreshDuration();
            }
            else
            {
                //Create a copy of the status effect
                StatusEffect newStatusEffect = ScriptableObject.CreateInstance<StatusEffect>();
                newStatusEffect.CopyFrom(statusEffect);

                //Apply the status effect copy to the player
                m_statusEffects[originalId] = newStatusEffect;
                newStatusEffect.OnApplied(this);
            }
        }

        /// <summary>
        /// Remove a status effect from handler
        /// </summary>
        /// <param name="statusEffect"> The status effect to be removed </param>
        public void Remove(StatusEffect statusEffect)
        {
            statusEffect.OnRemoved(this);

            m_toBeRemovedStatusEffects.Add(statusEffect);
        }
        #endregion

        #region MonoBehaviours=========================================================================
        void Update()
        {
            //Apply Tick for every status effects
            foreach (KeyValuePair<int, StatusEffect> kvp in m_statusEffects)
            {
                kvp.Value.OnTick(this);
            }

            //Remove all status effects that are marked as to be removed
            foreach (StatusEffect statusEffect in m_toBeRemovedStatusEffects)
            {
                m_statusEffects.Remove(statusEffect.originalId);
                Destroy(statusEffect);
            }
            m_toBeRemovedStatusEffects.Clear();
        }
        #endregion
    }
}