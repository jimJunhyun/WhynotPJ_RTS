using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeadState : UnitBaseState
{
    public override void OnEnterState()
    {
        unitAnimator.SetDeath();
        unitMove.NavMeshAgent.ResetPath();
        Invoke("DeadPush", 4f);
    }

    public override void OnExitState()
    {
        //Do Nothing
    }

    public override void UpdateState()
    {
        //DO Nothing
    }

    public void DeadPush()
    {
        PoolManager.Instance.Push(unitController);
    }
}
