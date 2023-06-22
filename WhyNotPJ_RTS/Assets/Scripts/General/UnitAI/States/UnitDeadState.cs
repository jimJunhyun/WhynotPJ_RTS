using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeadState : UnitBaseState
{
    public override void OnEnterState()
    {
        unitAnimator.SetDeath();
        unitMove.NavMeshAgent.ResetPath();
        if(unitController.isPlayer)
            UnitCounter.instance.DecreaseUnit();
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
