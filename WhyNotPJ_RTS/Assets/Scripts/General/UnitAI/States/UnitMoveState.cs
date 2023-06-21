using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    private bool isPending;
    private float requireTime, trafficCounter = 0f;

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
        isPending = false;

        unitAnimator.SetIsWalk(true);

        //전투 중에 어디선가 Move State를 계속 부름(그래서 순간이동하는 것으로 추정)
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

        if (!isPending && !unitMove.NavMeshAgent.pathPending)
        {
            isPending = true;
            requireTime = unitMove.NavMeshAgent.remainingDistance / unitController.moveSpeed;
            trafficCounter = unitMove.NavMeshAgent.stoppingDistance = 0f;
        }

        if (unitMove.IsAttack)
        {
            if (unitController.mainCamp != null)
            {
                if (Vector3.Distance(unitMove.VisualTrm.position, unitController.mainCamp.transform.position) <= unitController.attackRange)
                {
                    unitController.ChangeState(State.Attack);

                    return;
                }

                return;
            }
            else if (unitController.enemy != null)
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
            if (unitMove.NavMeshAgent.remainingDistance <= 0.5f && !unitMove.NavMeshAgent.pathPending)
            {
                unitController.ChangeState(State.Wait);
            }
            
            if (isPending)
            {
                if (trafficCounter < requireTime)
                {
                    trafficCounter += Time.deltaTime;
                }
                else
                {
                    unitMove.NavMeshAgent.stoppingDistance += Time.deltaTime;
                }
            }
        }
    }
}
