using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� �ӽ� ���� Ŭ����. ���� �ٲ� �� �ִ�.
/// </summary>
public class TestUnit : MonoBehaviour, IUnit
{
	// IUnit
	public GameObject prefab => gameObject;
	public string myName => "�׽�Ʈ ����";

	public float produceTime => 1f;

	public Element element => new Element(1, 1, 1);

	public Action onCompleted => () => Debug.Log("������ �����");

	public UnitState state { get; set; } = UnitState.Wait;
	public Vector3 objPos { get; set; }
	public Transform target { get; set; }


	// Original
	[SerializeField] private GameObject marker;

	public void SelectUnit()
	{
		marker.SetActive(true);
	}

	public void DeselectUnit()
	{
		marker.SetActive(false);
	}
}