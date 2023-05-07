using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWaitState : UnitBaseState
{
    public override void OnEnterState()
    {

    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                unitMove.SetTargetPosition(hit.point);
            }
        }

        if (unitMove.NavMeshAgent.velocity.sqrMagnitude >= 0.1f)
        {
            unitController.ChangeState(State.Move);
        }
    }

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
    }
}
