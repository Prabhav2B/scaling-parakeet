using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(FroggyController))]
public class FroggyBrain : MonoBehaviour
{
    [Range(0f, 100f)] 
    [SerializeField] private float waitBetweenJump = 1f;

    private float _timeElapsed;
    private bool _jumpCompleted;

    private FroggyController _froggyController;

    private delegate void StateChange(ProjectEnums.FroggyState state);
    private StateChange onStateChange;
    
    
    private void Start()
    {
        _froggyController = GetComponent<FroggyController>();
        if(_froggyController == null)
            Debug.LogError("Missing Reference to Froggy Controller :dead:!", this);
        
        _timeElapsed = 0f;
        _jumpCompleted = true;
    }

    private void OnEnable()
    {
        onStateChange += HandleStateChange;
    }

   
    private void OnDisable()
    {
        onStateChange -= HandleStateChange;
    }
    
    private void HandleStateChange(ProjectEnums.FroggyState state)
    {
        Debug.Log(state);
        switch (state)
        {
            
            case ProjectEnums.FroggyState.Idle:
                StartCoroutine(Idleing());
                break;
            case ProjectEnums.FroggyState.Jump:
                if ((_timeElapsed > waitBetweenJump))
                {
                    if (TryJump())
                    {
                        _jumpCompleted = false;
                        break;
                    }
                }
                EvaluateState();
                break;
            case ProjectEnums.FroggyState.ReachTarget:
                TryReachTarget();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void TryReachTarget()
    {
        var targetPosition = FindObjectOfType<TargetBehavior>().transform.position;
        var vectorToTarget = (targetPosition - transform.position);
        var normalVectorToTarget = vectorToTarget.normalized;
        
        _froggyController.ReceiveJumpCommand(normalVectorToTarget);
    }

    private void Update()
    {
        if(_jumpCompleted)
            _timeElapsed += Time.deltaTime;
    }

    private void EvaluateState()
    {
        if (FindObjectOfType<TargetBehavior>() != null)
        {
            onStateChange?.Invoke(ProjectEnums.FroggyState.ReachTarget);
            return;
        }

        if (_jumpCompleted)
        {
            onStateChange?.Invoke(Random.value > 0.5f ? ProjectEnums.FroggyState.Jump : ProjectEnums.FroggyState.Idle);
            return;
        }

        onStateChange?.Invoke(ProjectEnums.FroggyState.Idle);

    }

    private bool TryJump()
    {
        if (!_froggyController.IsGrounded) return false;
        
        _froggyController.RandomizeJumpDirection();
        _froggyController.ReceiveJumpCommand();
        return true;

    }

    public void ResetGroundedFlag(int id)
    {
        _jumpCompleted = true;
        _timeElapsed = 0f;
        EvaluateState();
    }

    private IEnumerator Idleing()
    {
        //wait for random time
        Debug.Log("Started Idleing");
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        
        Debug.Log("Ended Idleing");
        EvaluateState();
    }
}


