using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    public override void OnEnterState()
    {
        unitAnimator.SetIsWalk(true);
    }

    public override void OnExitState()
    {
        unitAnimator.SetIsWalk(false);
    }

    public override void UpdateState()
    {
        unitMove.SetAreaSpeed(unitController.moveSpeed);

        if (unitMove.NavMeshAgent.remainingDistance <= 0f)
        {
            unitController.ChangeState(State.Wait);
        }
    }
}
