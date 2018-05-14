# TimePlus.StatusEffect
TimePlus.StatusEffect is a Unity tools developed to allow user to create variety of status effect fast and easy

## Instructions
[![Watch the video](https://img.youtube.com/vi/0PLvlRxStz4/0.jpg)](https://youtu.be/0PLvlRxStz4)

### Adding StatusEffect Container to GameObject
To make a GameObject able to receive StatusEffect, simply add “StatusEffectHandler” to the game object as new component

### Creating a new status effect
1. Right click anywhere on the Project window -> Select Create -> Time Plus -> StatusEffect
2. Fill the parameter as wanted in the inspector

### Creating custom event / action
1. Create a new script
2. Add “using Time Plus” on top of the script file
3. The class must inherit from “SECEvent” / “SECAction”
4. Override one or more of these functions:
   1. SECEvent
      * ShouldTriggerOnApplied(): if it return true, SECAction will be triggered when the status effect is applied to the GameObject
      * ShouldTriggerNow(): if it return true, SECAction will be triggered during Update in the same frame it returns true
      * ShouldTriggerOnRemoved(): If it return true, SECAction will be triggered when the status effect is removed from the GameObject
   2. SECAction
      * ApplyAction(): this function will be called when SECAction is triggered. Put what should happen in this function definition
   3. Others
      * OnApplied(): this function will be called when the status effect is applied to the GameObject
      * OnRemoved(): this function will be called when the status effect is removed from the GameObject

```cs
using UnityEngine;
using TimePlus;
public class SECActionDealDamage : SECAction
{
    [SerializeField] private int m_damage = 0;

    public override void ApplyAction(GameObject target)
    {
        target.GetComponent<Character>().TakeDamage(m_damage);
    }

    public override string ToString()
    {
        return "Deal " + m_damage + " damage to owner";
    }
}

```
