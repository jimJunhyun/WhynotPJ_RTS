using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAlertState : UnitBaseState
{
    private Transform playerTrm;

    public override void OnEnterState()
    {

    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        //RaycastHit detect;
    }

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);

        playerTrm = agentRoot;
    }
}
