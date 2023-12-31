using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetTarget : MonoBehaviour
{

    [SerializeField] private GameObject target;
    private Camera _mainCam;

    private static int _targetCount;
    private GameObject _targetInstance;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    public void CreateTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_targetCount > 0) return;
        
        var worldPos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldPos.Scale(new Vector3(1f, 1f, 0f));
        _targetInstance = Instantiate(target, worldPos, quaternion.identity, transform);

        _targetCount++;
    }
    
    public void DeleteTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_targetCount == 0) return;
        
        Destroy(_targetInstance);
        _targetCount--;
    }
    
}
