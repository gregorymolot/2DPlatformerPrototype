using UnityEngine;

public abstract class BaseIdleAbility : BaseStateAbility
{
    
    public override void Init(Rigidbody2D rb2D)
    {
        base.Init(rb2D);
        state = MoveState.Idle;
        unlocked = true;
        cost = 0;
    }
}
