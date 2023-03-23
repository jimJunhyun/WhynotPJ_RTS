using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 임시 유닛 클래스. 추후 바뀔 수 있다.
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
