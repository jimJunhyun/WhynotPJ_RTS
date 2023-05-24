using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private readonly int isWalkHash = Animator.StringToHash("isWalk");
    private Animator animator;

    private void Awake()
    {
        animator = transform.Find("Visual").GetComponent<Animator>();
    }

    public void SetIsWalk(bool value)
    {
        animator.SetBool(isWalkHash, value);
    }
}
