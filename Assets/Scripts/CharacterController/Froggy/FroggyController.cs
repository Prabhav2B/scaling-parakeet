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

    //private int _facingLeft;
    
    protected override void Jump(Vector2 targetNormalVector = default)
    {
        if (!Grounded) return;
        Grounded = false;

        onJumpInitiated?.Invoke(0);
        
        if(_direction * targetNormalVector.x < 0)
        {
            _direction *= -1;
            onFlip?.Invoke();
        }

        //reset y-velocity for consistency
        Rb.velocity = new Vector2(Rb.velocity.x, 0f);

        //NOTE: new x velocity is being appended to existing x velocity
        _jumpXVelocity = Rb.velocity.x + forwardJumpForce * targetNormalVector.x;

        // if(targetNormalVector.y > 0f)
        //     targetNormalVector.Scale(new Vector2(1f, 0f));
        
        //formula to reach height <jumpHeight>
        //under gravity <maxGravityAcceleration> 
        // v0=sqrt(2gY)
        JumpVelocity = Mathf.Sqrt(2f * LocalGravityY * jumpHeight * targetNormalVector.y);
        Rb.velocity = new Vector2(_jumpXVelocity, JumpVelocity);
    }
    
    protected void RandomJump()
    {
        if (!Grounded) return;
        Grounded = false;

        onJumpInitiated?.Invoke(0);

        //reset y-velocity for consistency
        Rb.velocity = new Vector2(Rb.velocity.x, 0f);

        //NOTE: new x velocity is being appended to existing x velocity
        _jumpXVelocity = Rb.velocity.x + forwardJumpForce * _direction;

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

    public void ReceiveJumpCommand(Vector2 direction = default)
    {
        if(direction == default)
            RandomJump();
        else
            Jump(direction);
    }
}
