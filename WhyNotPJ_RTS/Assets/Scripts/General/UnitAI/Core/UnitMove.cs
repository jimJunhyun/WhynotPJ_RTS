using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    private NavMeshHit hit;
    private Transform visualTrm;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    private NavMeshPath path;
    private Animator animator;

    private Vector3 lastGround;
    private float groundDis;

    private void Start()
    {
        visualTrm = transform.GetChild(1);
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        animator = visualTrm.GetComponent<Animator>();
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

                groundDis = Vector3.Distance(lastGround, visualTrm.position);

                if (groundDis <= 20)
                {
                    visualTrm.localPosition = new Vector3(0f, -(groundDis / 4), 0f);
                }

                break;
            default:
                navMeshAgent.speed = moveSpeed;
                visualTrm.localPosition = Vector3.zero;

                lastGround = transform.position;

                break;
        }

        if (navMeshAgent.velocity.sqrMagnitude >= 0.1f)
        {
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }
}
