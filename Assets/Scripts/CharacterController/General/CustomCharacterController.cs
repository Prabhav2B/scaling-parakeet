using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CustomCharacterController : MonoBehaviour
{

    [Header("Select Variable Parameter")] 
    [SerializeField] private ProjectEnums.JumpParameters jumpParameters;
    
    [Header("Jump Parameter")] 
    [Range(0, 10)]
    [SerializeField] private float gravityScale = 1f;
    [Range(0f, 20f)]
    [SerializeField]
    protected float jumpHeight = 10f;
    [Range(0.1f, 10f)]
    [SerializeField] private float timeToReachJumpPeak = 1f;
    
    [Header("OptionalSettings")] 
    [SerializeField] private bool squashAndStretch = false;
    [Space(5)]
    [SerializeField] private bool altFallGravity = false;
    [Range(0, 10)]
    [SerializeField] private float altFallGravityScale = 1.2f;
    

    [Header("Jump Events")]
    public UnityEvent<float> omJumpApexReached;
    public UnityEvent<int> onJumpInitiated;
    public UnityEvent<int> onLanded;
    public UnityEvent onFlip;

    protected Rigidbody2D Rb;
    private Vector2 _localGravity;
    private Vector2 _localAltFallGravity;
    protected float LocalGravityY;
    protected bool Grounded;
    protected float JumpVelocity;

    public bool IsGrounded => Grounded;

    private const float BaseGravity = 9.8f;

    private void Awake()
    {
        if(squashAndStretch)
            return;
        
        var squashAndStretchComp = GetComponentInChildren<SquashAndStretch>();
        if (squashAndStretchComp != null)
        {
            squashAndStretchComp.DisableSelf();
        }
            
    }

    // Start is called before the first frame update
    void Start()
    {
        Grounded = true;
        Rb = GetComponent<Rigidbody2D>();
        Rb.gravityScale = 0f;
        LocalGravityY = gravityScale * BaseGravity;

        //this function can alter : jumpHeight, timeToReachJumpPeak and _localGravityY
        JumpParameterCalculation();
        
        _localGravity = new Vector2(0f, -LocalGravityY);
        _localAltFallGravity = new Vector2(0f, -altFallGravityScale * BaseGravity);

    }

    private void JumpParameterCalculation()
    {
        switch (jumpParameters)
        {
            case ProjectEnums.JumpParameters.Height:
            {
                jumpHeight = (LocalGravityY * Mathf.Pow(timeToReachJumpPeak, 2f)) / 2f;
            }
                break;
            case ProjectEnums.JumpParameters.Gravity:
            {
                LocalGravityY = 2 * jumpHeight / Mathf.Pow(timeToReachJumpPeak, 2f);
            }
                break;
            case ProjectEnums.JumpParameters.Time:
            {
                timeToReachJumpPeak = Mathf.Sqrt((2 * jumpHeight)/LocalGravityY);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FixedUpdate()
    {
        if (altFallGravity)
        {
            if (Rb.velocity.y < 0.01f)
            {
                //Apply Alt Gravity
                Rb.AddForce(_localAltFallGravity, ForceMode2D.Force);
                return;
            }   
        }
        
        //ApplyGravity
        Rb.AddForce(_localGravity, ForceMode2D.Force);
    }

    protected virtual void Jump(Vector2 targetNormalVector = default)
    {
        
        if (!Grounded) return;
        Grounded = false;

        onJumpInitiated?.Invoke(0);

        //reset y-velocity for consistency
        Rb.velocity = new Vector2(Rb.velocity.x, 0f);

        //formula to reach height <jumpHeight>
        //under gravity <maxGravityAcceleration> 
        // v0=sqrt(2gY)
        JumpVelocity = Mathf.Sqrt(2f * LocalGravityY * jumpHeight);
        Rb.velocity = new Vector2(Rb.velocity.x, JumpVelocity);
    }

    public void PlayerHitGround(int id)
    {
        Grounded = true;
        onLanded?.Invoke(0);
    }
    
    [Obsolete("Not used any more, using a velocity based implementation", true)]
    private void JumpPhysics()
    {
        
        if (!Grounded) return;
        Grounded = false;
        
        //reset y-velocity for consistency
        Rb.velocity = new Vector2(Rb.velocity.x, 0f);

        var jumpVec = Vector2.up * 10f;
        Rb.AddForce(jumpVec, ForceMode2D.Impulse);
    }
}
