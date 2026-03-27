using UnityEngine;

public abstract class BaseFallAbility : BaseStateAbility
{
    [SerializeField]
    protected float maxMoveSpeed;
    [SerializeField]
    protected float maxAccelerationRate;
    [SerializeField]
    protected float maxFallGravity;
    [SerializeField]
    protected float maxFallSpeed;
    public override void Init(Rigidbody2D rb2D)
    {
        base.Init(rb2D);
        state = MoveState.Fall;
    }
}
