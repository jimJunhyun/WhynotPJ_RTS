using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    public override void OnEnterState()
    {

    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        unitMove.SetAreaSpeed(unitController.moveSpeed);
    }

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
    }
}
