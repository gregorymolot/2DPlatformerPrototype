using System;
using UnityEngine;
using UnityEngine.UI;

enum AbilityState
{
    Available,
    Equipped,
    Locked,
    CantAfford
}

public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    Image icon;
    BaseStateAbility ability;
    [SerializeField]
    AbilityState abilityState = AbilityState.Locked;
    [SerializeField]
    Color equippedColor;
    [SerializeField]
    Color lockedColor;
    [SerializeField]
    Color availableColor;
    [SerializeField]
    Color cantAffordColor;

    void Awake()
    {
        button.onClick.AddListener(Equip);
        CheckAvilability();
    }

    void OnEnable()
    {
        EventManager.equipEvents.OnEquip += AbilityEquipped;
        CheckAvilability();
    }
    void OnDisable()
    {
        EventManager.equipEvents.OnEquip -= AbilityEquipped;
    }

    void Update()
    {
        CheckAvilability();
    }

    void AbilityEquipped(BaseStateAbility state)
    {
        if (state == ability)
        {
            return;
        }
        if (state.state == ability.state)
        {
            abilityState = AbilityState.Available;
        }
        CheckAvilability();
    }

    void CheckAvilability()
    {
        if (ability == null)
        {
            return;
        }
        else if (!ability.unlocked)
        {
            abilityState = AbilityState.Locked;
            Debug.Log("Not for me!");
            icon.color = lockedColor;
        }
        else if (abilityState == AbilityState.Equipped)
        {
            icon.color = equippedColor;
        }
        else if (ability.cost <= AbilityManager.Instance.availableSlots && ability.unlocked && abilityState != AbilityState.Equipped)
        {
            abilityState = AbilityState.Available;
            icon.color = availableColor;
        }
        else if (ability.cost > AbilityManager.Instance.availableSlots && ability.unlocked && abilityState != AbilityState.Equipped)
        {
            abilityState = AbilityState.CantAfford;
            icon.color = cantAffordColor;
        }
    }


    public void Init(BaseStateAbility baseAbility)
    {
        ability = baseAbility;
        CheckAvilability();
    }

    void Equip()
    {
        if (abilityState == AbilityState.Equipped)
        {
            abilityState = AbilityState.Available;
            AbilityManager.Instance.Unequip(ability);
        }
        else if (abilityState == AbilityState.Available)
        {
            abilityState = AbilityState.Equipped;
            AbilityManager.Instance.Equip(ability);
        }
        //Send out equipped event
        EventManager.equipEvents.OnEquip?.Invoke(ability);
    }


}
