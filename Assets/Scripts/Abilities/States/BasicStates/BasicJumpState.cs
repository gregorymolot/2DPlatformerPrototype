using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Jump", menuName = "States/Jumps/Basic")]
public class BasicJumpState : BaseJumpAbility
{
    public override void EnterState()
    {
        //Use gravity manager to increase gravity by amount for certain amount of time
        GravityController.Instance.SetGravity(0, maxJumpGravity, timeToMaxJumpGravity);
        
        Vector2 dotMag = Vector2.Dot(rb2D.linearVelocity, rb2D.transform.up) * rb2D.transform.up;
        rb2D.linearVelocity -= dotMag;
        rb2D.AddForce(rb2D.transform.up * jumpPower, ForceMode2D.Impulse);
    }

    public override void ExitState()
    {
    }

    public override void FixedUpdateState(Vector2 moveDirection)
    {
        float desiredVelocity = moveDirection.x * maxMoveSpeedAir;
        Vector2 currentVelocity = rb2D.linearVelocity;
        float magnitude = currentVelocity.magnitude;
        currentVelocity = Vector2.Dot(currentVelocity.normalized, rb2D.transform.right) * rb2D.transform.right * magnitude;

        Vector2 speedDiff = ((Vector2)rb2D.transform.right * desiredVelocity) - currentVelocity;
        
        Vector2 movement = speedDiff * maxAccelerationRateAir;

        rb2D.AddForce(movement + GravityController.Instance.gravity);

    }

    public override void UpdateState()
    {
    }
}
