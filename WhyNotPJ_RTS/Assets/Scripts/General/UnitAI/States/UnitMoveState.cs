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

        unitMove.NavMeshAgent.updatePosition = true;

        unitAnimator.SetIsWalk(true);

        //어디선가 Move State를 계속 부름
    }

    public override void OnExitState()
    {
        unitMove.NavMeshAgent.updatePosition = false;
        unitMove.NavMeshAgent.velocity = Vector3.zero;
        
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

                if ((unitController.enemy.transform.position - unitMove.VisualTrm.position).magnitude <= unitController.attackRange)
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
            if (unitMove.NavMeshAgent.remainingDistance <= 0f && !unitMove.NavMeshAgent.pathPending)
            {
                unitController.ChangeState(State.Wait);
            }
        }
    }
}
