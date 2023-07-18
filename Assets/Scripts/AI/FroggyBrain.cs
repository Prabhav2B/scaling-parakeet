using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FroggyController))]
public class FroggyBrain : MonoBehaviour
{
    [Range(0f, 100f)] 
    [SerializeField] private float waitBetweenJump = 1f;

    private float _timeElapsed;
    private bool _jumpCompleted;

    private FroggyController _froggyController;
    
    private void Start()
    {
        _froggyController = GetComponent<FroggyController>();
        if(_froggyController == null)
            Debug.LogError("Missing Reference to Froggy Controller :dead:!", this);
        
        _timeElapsed = 0f;
        _jumpCompleted = true;
    }

    private void Update()
    {
        if(_jumpCompleted)
            _timeElapsed += Time.deltaTime;

        if (!(_timeElapsed > waitBetweenJump)) return;
        if (!TryJump()) return;
        _timeElapsed = 0f;
        _jumpCompleted = false;
    }

    private bool TryJump()
    {
        if (!_froggyController.IsGrounded) return false;
        
        _froggyController.ReceiveJumpCommand();
        return true;

    }

    public void ResetGroundedFlag(int id)
    {
        _jumpCompleted = true;
    }
}
