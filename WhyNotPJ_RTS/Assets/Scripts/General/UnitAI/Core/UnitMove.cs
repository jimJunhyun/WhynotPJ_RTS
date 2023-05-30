using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    private NavMeshHit hit;
    private Transform visualTrm;
    public Transform VisualTrm => visualTrm;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    private NavMeshPath path;
    private bool isAttack;
    public bool IsAttack => isAttack;

    private void Start()
    {
        visualTrm = transform.Find("Visual");
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    public bool SetTargetPosition(Vector3 pos)
    {
        if (navMeshAgent.CalculatePath(pos, path))
        {
            navMeshAgent.SetDestination(pos);

            isAttack = false;

            return true;
        }

        return false;
    }

    public bool SetTargetPosition(Transform target)
    {
        if (navMeshAgent.CalculatePath(target.position, path))
        {
            navMeshAgent.SetDestination(target.position);

            isAttack = true;

            return true;
        }

        return false;
    }

    public void SetAreaSpeed(float moveSpeed)
    {
        navMeshAgent.SamplePathPosition(NavMesh.AllAreas, 1, out hit);

        switch (hit.mask)
        {
            case 8:
                navMeshAgent.speed = moveSpeed * 0.5f;
                visualTrm.localPosition = Vector3.down * 1.5f;

                break;
            default:
                navMeshAgent.speed = moveSpeed;
                visualTrm.localPosition = Vector3.zero;

                break;
        }
    }
}
