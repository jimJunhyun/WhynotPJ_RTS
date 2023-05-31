using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private float curAttackCoolTime;

    public override void OnEnterState()
    {
        curAttackCoolTime = 0f;
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        curAttackCoolTime -= Time.deltaTime;

        if (Vector3.Distance(unitMove.VisualTrm.position, unitController.enemy.transform.position) > unitController.attackRange)
        {
            if (unitMove.SetTargetPosition(unitController.enemy.transform))
            {
                unitController.ChangeState(State.Move);

                return;
            }
        }

        if (curAttackCoolTime <= 0f)
        {
            unitController.enemy.CurrentStateScript.OnHit(unitController);

            curAttackCoolTime = 1f / unitController.attackSpeed;
        }
    }
}
