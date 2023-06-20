using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    private NavMeshHit hit;
    private Transform visualTrm;
    public Transform VisualTrm => visualTrm;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    private bool isAttack;
    public bool IsAttack => isAttack;

    private void Awake()
    {
        isAttack = true;
        visualTrm = transform.Find("Visual");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public bool SetTargetPosition(Vector3 pos)
    {
        if (navMeshAgent.CalculatePath(pos, new NavMeshPath()))
        {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(pos);

            isAttack = false;

            return true;
        }

        return false;
    }

    public bool SetTargetPosition(Transform target)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(target.position, out hit, 10f, NavMesh.AllAreas);
        if (navMeshAgent.CalculatePath(hit.position, new NavMeshPath()))
        {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(hit.position);

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
