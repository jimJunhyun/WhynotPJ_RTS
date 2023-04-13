using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    private NavMeshHit hit;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath path;

    private void Start()
    {
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
        if (navMeshAgent.velocity.sqrMagnitude >= 0.1f)
        {
            navMeshAgent.SamplePathPosition(NavMesh.AllAreas, 1, out hit);

            switch (hit.mask)
            {
                case 8:
                    navMeshAgent.speed = moveSpeed * 0.5f/*0.5f는 기획에 따라 변경 가능*/;

                    break;
                default:
                    navMeshAgent.speed = moveSpeed;

                    break;
            }
        }
    }
}
