using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAlertState : UnitBaseState
{
    private Collider[] opponents;
    private float closestDis;

    public override void OnEnterState()
    {
        unitController.InitTarget();
        unitMove.NavMeshAgent.ResetPath();

        //��� �� ���� ����
        /*if (Physics.SphereCast(unitMove.VisualTrm.position, unitController.detectRange, Vector3.down, out RaycastHit hitInfo, 0f, unitController.whatIsMainCamp))
        {
            
            if (unitMove.SetTargetPosition(hitInfo.transform))
            {
                unitController.mainCamp = hitInfo.transform.GetComponent<MainCamp>();
                unitController.enemy = null;
                unitController.construction = null;

                unitController.ChangeState(State.Move);

                return;
            }
        }*/

        opponents = Physics.OverlapSphere(unitMove.VisualTrm.position, unitController.detectRange, unitController.whatIsUnit);
        closestDis = unitController.detectRange;

        foreach (Collider op in opponents)
        {
            Transform opTrm = op.transform;
            UnitController opCtrl;
			if (opCtrl = opTrm.GetComponent<UnitController>())
			{
                if(opCtrl.currentState != State.Dead && opCtrl.isPlayer != unitController.isPlayer && opCtrl.isSeen())
				{
                    float distance = (opTrm.position - transform.position).sqrMagnitude;

                    if (distance < closestDis)
                    {
                        closestDis = distance;

                        unitController.InitTarget(opCtrl);
                    }
                }
			}
            
        }

        if (unitController.enemy != null)
        {
            if (unitMove.SetTargetPosition(unitController.enemy.transform))
            {
                unitController.ChangeState(State.Move);

                return;
            }
        }

        //��� �� ��� ����
        /*opponents = Physics.OverlapSphere(unitMove.VisualTrm.position, unitController.detectRange, unitController.whatIsConstruction);
        closestDis = unitController.detectRange;

        foreach (Collider op in opponents)
        {
            Transform opTrm = op.transform;
            float distance = (opTrm.position - transform.position).sqrMagnitude;

            if (distance < closestDis)
            {
                closestDis = distance;
                unitController.mainCamp = null;
                unitController.enemy = null;
                unitController.construction = opTrm.GetComponent<GroundBreak>();
            }
        }

        if (unitController.construction != null)
        {
            if (unitMove.SetTargetPosition(unitController.construction.transform))
            {
                unitController.ChangeState(State.Move);

                return;
            }
        }*/ 

        unitController.ChangeState(State.Wait);
    }

    public override void OnExitState()
    {
        //Do Nothing
    }

    public override void UpdateState()
    {
        //Do Nothing
    }
}
