using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : IUnit
{
    protected UnitController unitController;
    protected UnitMove unitMove;

    public abstract void OnEnterState();

    public abstract void OnExitState();

    public abstract void UpdateState();

    public virtual void SetUp(Transform agentRoot)
    {
        unitController = agentRoot.GetComponent<UnitController>();
        unitMove = agentRoot.GetComponent<UnitMove>();
    }
}
