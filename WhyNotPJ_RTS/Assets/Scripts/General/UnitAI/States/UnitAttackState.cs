using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private bool isAttack;
    private float curAttackCoolTime;

    public override void OnEnterState()
    {
        unitAnimator.onAnimationEventTrigger += OnAttackHandle;
        unitAnimator.onAnimationEndTrigger += AttackEndHandle;
    }

    public override void OnExitState()
    {
        unitAnimator.onAnimationEventTrigger -= OnAttackHandle;
        unitAnimator.onAnimationEndTrigger -= AttackEndHandle;
    }

    public override void UpdateState()
    {
        if (isAttack)
        {
            return;
        }

        else if (unitController.enemy?.currentState == State.Dead)
        {
            unitController.ChangeState(State.Alert);

            return;
        }

        curAttackCoolTime -= Time.deltaTime;

        if (unitController.enemy != null && (unitController.enemy.transform.position - unitMove.VisualTrm.position).magnitude > unitController.attackRange)
        {
            if (unitMove.SetTargetPosition(unitController.enemy.transform))
            {
                unitController.ChangeState(State.Move);

                return;
            }
        }

        if (curAttackCoolTime <= 0f)
        {
            isAttack = true;

            unitAnimator.SetAttackAnimation(isAttack);
        }
    }

    public void OnAttackHandle()
    {
        unitController.enemy?.CurrentStateScript.OnHit(unitController);
        unitController.mainCamp?.OnHit(unitController);
    }

    public void AttackEndHandle()
    {
        curAttackCoolTime = 1f / unitController.attackSpeed;
        isAttack = false;

        unitAnimator.SetAttackAnimation(isAttack);
    }
}
