using System;
using JetBrains.Annotations;
using UnityEngine;

public static class EventManager
{
    public static readonly EquipEvents equipEvents = new EquipEvents();
    public class EquipEvents
    {
        public Action<BaseStateAbility> OnEquip;
    } 
}
