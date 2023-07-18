using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GroundCheck : MonoBehaviour
{
    
    public LayerMask layerMask;
    public UnityEvent<int> onLayerEnterCollision;
    public UnityEvent<int> onLayerExitCollision;

    private void Start()
    {
        if (onLayerEnterCollision.GetPersistentEventCount() == 0 || onLayerExitCollision.GetPersistentEventCount() == 0 )
        {
            Debug.LogWarning("No events subscribed to some of the events of this object!", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //returns an id for the ground type 
        //so that different sound effect of
        //particles can be triggered
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        onLayerEnterCollision?.Invoke(0);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //returns an id for the ground type 
        //so that different sound effect of
        //particles can be triggered
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        onLayerExitCollision?.Invoke(0);
    }
}
