// SECAction.cs
// Author: Tedo Pranowo
// This file contains the base class inherited by a status effect action

using UnityEngine;

namespace TimePlus
{
    public abstract class SECAction : StatusEffectComponent
    {
        public virtual void ApplyAction(GameObject target) { }
    }
}