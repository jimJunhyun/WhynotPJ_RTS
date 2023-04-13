using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 임시 유닛 클래스. 추후 바뀔 수 있다.
/// </summary>
public class TestUnit : UnitDefault
{
	// IUnit
	public override GameObject _prefab => gameObject;
	public override string _myName => "테스트 유닛";

	public override float _produceTime => 1f;

	public override Element _element => new Element(1, 1, 1);

	public override Action _onCompleted => () => Debug.Log("유닛이 생산되다");

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