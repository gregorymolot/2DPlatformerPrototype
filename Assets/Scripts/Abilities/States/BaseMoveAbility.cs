using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseMoveAbility : BaseStateAbility
{
    [SerializeField]
    protected float maxMoveSpeed;
    [SerializeField]
    protected float maxAccelerationRate;
    public override void Init(Rigidbody2D rb2D)
    {
        base.Init(rb2D);
        state = MoveState.Walk;
    }
}
