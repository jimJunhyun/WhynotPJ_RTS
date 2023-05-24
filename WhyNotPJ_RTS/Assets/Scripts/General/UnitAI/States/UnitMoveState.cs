using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    public override void OnEnterState()
    {
        if (unitMove.IsAttack)
        {
            if (unitController.mainCamp != null)
            {
                if (true/*본진이 유효하지 않음, 즉 게임이 종료됨*/)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }
            else if (unitController.enemy != null)
            {
                if (unitController.enemy.currentState == State.Dead)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }
            else if (unitController.construction != null)
            {
                if (unitController.construction.isBroken)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }
        }

        unitAnimator.SetIsWalk(true);
    }

    public override void OnExitState()
    {
        unitAnimator.SetIsWalk(false);
    }

    public override void UpdateState()
    {
        unitMove.SetAreaSpeed(unitController.moveSpeed);

        if (unitMove.IsAttack)
        {
            if (unitController.mainCamp != null)
            {
                if (true/*본진이 유효하지 않음, 즉 게임이 종료됨*/)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }
            else if (unitController.enemy != null)
            {
                if (unitController.enemy.currentState == State.Dead)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }
            else if (unitController.construction != null)
            {
                if (unitController.construction.isBroken)
                {
                    unitController.ChangeState(State.Alert);

                    return;
                }
            }

            //공격 범위 이내에 들어오면 공격
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
