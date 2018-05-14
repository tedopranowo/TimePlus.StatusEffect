// StatusEffectComponent.cs
// Author: Tedo Pranowo
// This file contains the base class which inherited by SECEvent and SECAction

public abstract class StatusEffectComponent
{
    public virtual void OnApplied() { }
    public virtual void OnRemoved() { }
}   