using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    private Collider[] opponents;

    public override void OnEnterState()
    {
        if (unitMove.IsAttack)
        {
            if (unitController.enemy != null)
            {
                if (unitController.enemy.currentState == State.Dead)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }
            //이동 시작 시 토목 유효성 검사
            /*else if (unitController.construction != null)
            {
                if (unitController.construction.isBroken)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }*/
        }

        unitAnimator.SetIsWalk(true);
    }

    public override void OnExitState()
    {
        unitMove.NavMeshAgent.ResetPath();
        unitAnimator.SetIsWalk(false);
    }

    public override void UpdateState()
    {
        unitMove.SetAreaSpeed(unitController.moveSpeed);

        if (unitMove.IsAttack)
        {
            if (unitController.enemy != null)
            {
                if (unitController.enemy.currentState == State.Dead)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }

                if (Vector3.Distance(unitMove.VisualTrm.position, unitController.enemy.transform.position) <= unitController.attackRange)
                {
                    unitController.ChangeState(State.Attack);

                    return;
                }
            }
            //이동 중 토목 유효성 검사
            /*else if (unitController.construction != null)
            {
                if (unitController.construction.isBroken)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }*/
        }
        else
        {
            if (unitMove.NavMeshAgent.remainingDistance <= 0f)
            {
                unitController.ChangeState(State.Wait);
            }
        }
    }
}
