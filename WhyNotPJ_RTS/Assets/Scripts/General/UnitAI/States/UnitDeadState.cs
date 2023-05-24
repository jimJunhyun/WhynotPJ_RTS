using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeadState : UnitBaseState
{
    public override void OnEnterState()
    {
        //»ç¸Á Ã³¸®
        transform.parent.gameObject.SetActive(false);
    }

    public override void OnExitState()
    {
        //Do Nothing
    }

    public override void UpdateState()
    {
        //DO Nothing
    }
}
