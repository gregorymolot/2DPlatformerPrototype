using UnityEngine;

[CreateAssetMenu(fileName = "Basic Move", menuName = "States/Moves/Basic")]
public class BasicMoveState : BaseMoveAbility
{
    public override void EnterState()
    {
        //Set gravity to 0
        GravityController.Instance.SetGravity(0f);
    }

    public override void ExitState()
    {
    }

    public override void FixedUpdateState(Vector2 moveDirection)
    {
        //Fix input when gravity is switched up
        float mag = moveDirection.magnitude;
        Vector2 desiredVelocity = moveDirection.normalized * maxMoveSpeed * mag;
        Vector2 currentVelocity = rb2D.linearVelocity;

        Vector2 speedDiff = desiredVelocity - currentVelocity;
        
        Vector2 movement = speedDiff * maxAccelerationRate;

        rb2D.AddForce(movement);

    }

    public override void UpdateState()
    {
        //Raycasts from side at bottom and skin depth, if bottom hits but top doesn't, move player up to where there is no collider
    }
}
