using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Fall", menuName = "States/Falls/Basic")]
public class BasicFallState : BaseFallAbility
{
    public override void EnterState()
    {
        //Gravity manager big gravity down
        GravityController.Instance.SetGravity(maxFallGravity);
    }

    public override void ExitState()
    {
    }

    public override void FixedUpdateState(Vector2 moveDirection)
    {
        float desiredVelocity = moveDirection.x * maxMoveSpeed;
        Vector2 currentVelocity = rb2D.linearVelocity;
        float magnitude = currentVelocity.magnitude;
        currentVelocity = Vector2.Dot(currentVelocity.normalized, rb2D.transform.right) * rb2D.transform.right * magnitude;

        Vector2 speedDiff = ((Vector2)rb2D.transform.right * desiredVelocity) - currentVelocity;
        
        Vector2 movement = speedDiff * maxAccelerationRate;

        Vector2 direction = -rb2D.transform.up;
        float directionalVelocity = Vector2.Dot(rb2D.linearVelocity,direction);

        if (directionalVelocity > maxFallSpeed)
        {
            rb2D.AddForce(movement);
            return;
        }

        rb2D.AddForce(movement + GravityController.Instance.gravity);
    }

    public override void UpdateState()
    {
        if (!GravityController.Instance.CheckGravity(maxFallGravity))
        {
            GravityController.Instance.SetGravity(maxFallGravity);
        }
    }
}
