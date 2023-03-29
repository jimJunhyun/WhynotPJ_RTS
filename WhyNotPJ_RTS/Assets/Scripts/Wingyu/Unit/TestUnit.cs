using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� �ӽ� ���� Ŭ����. ���� �ٲ� �� �ִ�.
/// </summary>
public class TestUnit : MonoBehaviour
{
    [SerializeField] private GameObject marker;
    private NavMeshAgent navMeshAgent;

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	public void SelectUnit()
	{
		marker.SetActive(true);
	}

	public void DeselectUnit()
	{
		marker.SetActive(false);
	}

	public void MoveTo(Vector3 end)
	{
		navMeshAgent.SetDestination(end);
	}
}
