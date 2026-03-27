using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class BaseStateAbility : BaseAbility
{
    public MoveState state {get; protected set;}
    protected Rigidbody2D rb2D;

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void FixedUpdateState(Vector2 moveDirection);

    public abstract void UpdateState();

    public virtual void Init(Rigidbody2D rb2D)
    {
        this.rb2D = rb2D;
    }
}
