using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeadState : UnitBaseState
{
    public override void OnEnterState()
    {
        unitAnimator.SetDeath();
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

    public void DeahPush()
    {
        PoolManager.Instance.Push(unitController);
        CancelInvoke();
    }
}
