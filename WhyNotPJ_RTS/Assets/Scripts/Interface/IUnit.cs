using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum State
    {
        Wait = 0,
        Alert,
        Move,
        Attack,

        Dead
    }
}

public interface IUnit
{
    public void OnEnterState();

    public void OnExitState();

    public void UpdateState();

    public void SetUp(Transform agentRoot);
}
