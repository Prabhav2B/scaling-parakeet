using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroggyController : CustomCharacterController
{
    [Header("Froggy Parameters")]
    [SerializeField][Range(0.1f ,10f)]
    private float forwardJumpForce = 5f;

    private float _jumpXVelocity;
    private float _direction = 1f;
    
    protected override void Jump()
    {
        if (!Grounded) return;
        Grounded = false;

        onJumpInitiated?.Invoke(0);

        //reset y-velocity for consistency
        Rb.velocity = new Vector2(Rb.velocity.x, 0f);

        _jumpXVelocity = forwardJumpForce * _direction;

        //formula to reach height <jumpHeight>
        //under gravity <maxGravityAcceleration> 
        // v0=sqrt(2gY)
        JumpVelocity = Mathf.Sqrt(2f * LocalGravityY * jumpHeight);
        Rb.velocity = new Vector2(_jumpXVelocity, JumpVelocity);
    }

    public void RandomizeJumpDirection()
    {
        //Random Flipping, definitely needs to be changed and placed somewhere else
        if (Random.value > 0.5f)
        {
            _direction *= -1;
            onFlip?.Invoke();
        }
    }

    public void ReceiveJumpCommand()
    {
        Jump();
    }
}
