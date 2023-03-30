using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    [Header("기본 스탯: 이동 속도"), SerializeField]
    private float moveSpeed;
    [Header("감속 속도"), SerializeField]
    private float decelSpeed;
    private NavMeshHit hit;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath path;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    private void Update()
    {
        if (navMeshAgent.velocity.sqrMagnitude >= 0.1f)
        {
            navMeshAgent.SamplePathPosition(NavMesh.AllAreas, 1, out hit);

            switch (hit.mask)
            {
                case 8:
                    navMeshAgent.speed = decelSpeed;

                    break;
                default:
                    navMeshAgent.speed = moveSpeed;

                    break;
            }
        }
    }

    public void SetTargetPosition(Vector3 pos)
    {
        if (navMeshAgent.CalculatePath(pos, path))
        {
            navMeshAgent.SetDestination(pos);
        }
    }
}
