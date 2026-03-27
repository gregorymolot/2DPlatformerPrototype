using UnityEngine;
using UnityEngine.UI;

public abstract class BaseAbility : ScriptableObject
{
    [SerializeField]
    public int cost;
    [SerializeField]
    public bool unlocked;
    [SerializeField]
    public Image icon;
}
