using UnityEngine;

public abstract class BaseDashAbility : BaseStateAbility
{
    
    [SerializeField]
    protected float dashSpeed;
    public override void Init(Rigidbody2D rb2D)
    {
        base.Init(rb2D);
        state = MoveState.Dash;
    }
}
