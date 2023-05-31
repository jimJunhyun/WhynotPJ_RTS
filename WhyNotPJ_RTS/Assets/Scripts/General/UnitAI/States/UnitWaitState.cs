using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWaitState : UnitBaseState
{
    [SerializeField]
    private float sendSignTime = 5f;
    private float curSignTime;

    public override void OnEnterState()
    {
        unitController.mainCamp = null;
        unitController.enemy = null;
        unitController.construction = null;
        curSignTime = sendSignTime;
    }

    public override void OnExitState()
    {
        //Do Nothing
    }

    public override void UpdateState()
    {
        if (unitMove.NavMeshAgent.velocity.sqrMagnitude >= 0.1f)
        {
            unitController.ChangeState(State.Move);
        }
    }

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        curSignTime -= Time.deltaTime;

        if (curSignTime <= 0f)
        {
            //대기 신호

            curSignTime = sendSignTime;
        }
    }
}
