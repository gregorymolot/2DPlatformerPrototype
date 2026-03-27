using UnityEngine;

public abstract class BaseJumpAbility : BaseStateAbility
{
    [SerializeField]
    protected float jumpPower;
    [SerializeField]
    protected float timeToMaxJumpGravity;
    [SerializeField]
    protected float maxJumpGravity;
    [SerializeField]
    protected float maxMoveSpeedAir;
    [SerializeField]
    protected float maxAccelerationRateAir;
    public override void Init(Rigidbody2D rb2D)
    {
        base.Init(rb2D);
        state = MoveState.Jump;
    }
}
