using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private readonly int isWalkHash = Animator.StringToHash("isWalk");
    private readonly int isAttackHash = Animator.StringToHash("isAttack");
    private readonly int attackTriggerHash = Animator.StringToHash("attack");
    private readonly int deathTriggerHash = Animator.StringToHash("death");
    public Action onAnimationEventTrigger = null;
    public Action onAnimationEndTrigger = null;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAnimationEvent()
    {
        onAnimationEventTrigger?.Invoke();
    }

    public void OnAnimationEnd()
    {
        onAnimationEndTrigger?.Invoke();
    }

    public void SetIsWalk(bool value)
    {
        animator.SetBool(isWalkHash, value);
    }

    public void SetAttackAnimation(bool value)
    {
        animator.SetBool(isAttackHash, value);

        if (value)
        {
            animator.SetTrigger(attackTriggerHash);
        }
        else
        {
            animator.ResetTrigger(attackTriggerHash);
        }
    }

    public void SetDeath()
    {
        animator.SetTrigger(deathTriggerHash);
    }
}
