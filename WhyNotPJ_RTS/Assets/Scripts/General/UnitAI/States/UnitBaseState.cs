using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

    public virtual void OnHit(UnitController attacker)
    {
        unitController.healthPoint -= attacker.attackPower * (1f - (unitController.defensePower - attacker.defensePenetration) / (unitController.defensePower - attacker.defensePenetration + 100f));

        if (unitController.healthPoint <= 0f)
        {
            unitController.healthPoint = 0f;

            unitController.ChangeState(State.Dead);
        }
        else
        {
            unitController.ChangeState(State.Alert);
        }
    }
}
