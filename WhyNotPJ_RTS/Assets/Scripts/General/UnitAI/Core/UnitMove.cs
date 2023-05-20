using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    private NavMeshHit hit;
    private Transform visualTrm;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    private NavMeshPath path;

    private void Start()
    {
        visualTrm = transform.Find("Visual");
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    public void SetTargetPosition(Vector3 pos)
    {
        if (navMeshAgent.CalculatePath(pos, path))
        {
            navMeshAgent.SetDestination(pos);
        }
    }

    public void SetAreaSpeed(float moveSpeed)
    {
        navMeshAgent.SamplePathPosition(NavMesh.AllAreas, 1, out hit);

        switch (hit.mask)
        {
            case 8:
                navMeshAgent.speed = moveSpeed * 0.5f/*0.5f는 기획에 따라 변경 가능*/;
                visualTrm.localPosition = Vector3.down * 1.5f;

                break;
            default:
                navMeshAgent.speed = moveSpeed;
                visualTrm.localPosition = Vector3.zero;

                break;
        }
    }
}
