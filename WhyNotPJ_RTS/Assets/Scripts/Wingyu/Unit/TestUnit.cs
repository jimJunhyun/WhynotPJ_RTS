using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� �ӽ� ���� Ŭ����. ���� �ٲ� �� �ִ�.
/// </summary>
public class TestUnit : UnitDefault
{
	// IUnit
	public override GameObject prefab => gameObject;
	public override string myName => "�׽�Ʈ ����";

	public override float produceTime => 1f;

	public override Element element => new Element(1, 1, 1);

	public override Action onCompleted => () => Debug.Log("������ ����Ǵ�");

	public override UnitState state { get; set; } = UnitState.Wait;
	public override Vector3 objPos { get; set; }
	public override Transform target { get; set; }

	public override bool CanDragSelect => true;

	// Original
	[SerializeField] private GameObject marker;

	public override void OnSelectUnit()
	{
		marker.SetActive(true);
	}

	public override void OnDeselectUnit()
	{
		marker.SetActive(false);
	}
}