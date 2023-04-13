using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� �ӽ� ���� Ŭ����. ���� �ٲ� �� �ִ�.
/// </summary>
public class TestUnit : UnitDefault
{
	// IUnit
	public override GameObject _prefab => gameObject;
	public override string _myName => "�׽�Ʈ ����";

	public override float _produceTime => 1f;

	public override Element _element => new Element(1, 1, 1);

	public override Action _onCompleted => () => Debug.Log("������ ����Ǵ�");

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