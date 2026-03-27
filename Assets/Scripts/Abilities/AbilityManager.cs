using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    int maxSlots;
    int currentSlots = 0;
    public int availableSlots {get {return maxSlots-currentSlots;}}
    [SerializeField]
    BaseMoveAbility defaultWalk;
    BaseMoveAbility currentlyEquippedWalk;
    [SerializeField]
    BaseFallAbility defaultFall;
    BaseFallAbility currentlyEquippedFall;
    [SerializeField]
    BaseJumpAbility defaultJump;
    BaseJumpAbility currentlyEquippedJump;
    [SerializeField]
    BaseIdleAbility defaultIdle;
    [SerializeField]
    List<BaseStateAbility> moveStates;

    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    GridLayoutGroup abilityUI;

    [SerializeField]
    Player player;

    public static AbilityManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("No Ability Manager");
            }
            return _instance;
        }
    }

    private static AbilityManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        Rigidbody2D rb2D = player.GetComponent<Rigidbody2D>();

        moveStates = moveStates.Distinct().ToList();

        moveStates.Remove(defaultFall);
        moveStates.Remove(defaultWalk);
        moveStates.Remove(defaultJump);
        moveStates.Remove(defaultIdle);


        foreach(BaseStateAbility state in moveStates)
        {
            state.Init(rb2D);
            GameObject button = Instantiate(buttonPrefab, abilityUI.transform);
            button.GetComponent<AbilityButton>().Init(state);
        }
        defaultFall.Init(rb2D);
        defaultWalk.Init(rb2D);
        defaultJump.Init(rb2D);
        defaultIdle.Init(rb2D);

        Equip(defaultFall);
        Equip(defaultWalk);
        Equip(defaultJump);
        Equip(defaultIdle);
    }

    void Update()
    {
    }

    public void Equip(BaseStateAbility ability)
    {
        player.Equip(ability);
        switch(ability.state)
        {
            case MoveState.Jump:
                if (currentlyEquippedJump != null)
                {
                    currentSlots-=currentlyEquippedJump.cost;
                }
                currentlyEquippedJump = (BaseJumpAbility)ability;
                break;
            case MoveState.Walk:
                if (currentlyEquippedWalk != null)
                {
                    currentSlots-=currentlyEquippedWalk.cost;
                }
                currentlyEquippedWalk = (BaseMoveAbility)ability;
                break;
        }
        currentSlots += ability.cost;
    }

    public void Unequip(BaseStateAbility ability)
    {
        currentSlots -= ability.cost;
        switch(ability.state)
        {
            case MoveState.Jump:
            player.Equip(defaultJump);
            break;
            case MoveState.Walk:
            player.Equip(defaultWalk);
            break;
        }
    }
}
