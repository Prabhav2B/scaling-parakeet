using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectEnums 
{
    public enum JumpParameters
    {
        Height,
        Gravity,
        Time
    }

    public enum FroggyState
    {
        Idle,
        Jump,
        ReachTarget
    }
}
