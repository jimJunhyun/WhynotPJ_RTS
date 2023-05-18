using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBaseState : MonoBehaviour, IUnitState
{
    [HideInInspector]
    public UnitController unitController;
    [HideInInspector]
    public UnitMove unitMove;
    [HideInInspector]
    public UnitAnimator unitAnimator;

    public abstract void OnEnterState();

    public abstract void OnExitState();

    public abstract void UpdateState();

    public virtual void SetUp(Transform agentRoot)
    {
        unitController = agentRoot.GetComponent<UnitController>();
        unitMove = agentRoot.GetComponent<UnitMove>();
        unitAnimator = agentRoot.GetComponent<UnitAnimator>();
    }
}
