using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 임시 유닛 클래스. 추후 바뀔 수 있다.
/// </summary>
public class TestUnit : UnitDefault
{
	// IUnit
	public override GameObject prefab => gameObject;
	public override string myName => "테스트 유닛";

	public override float produceTime => 1f;

	public override Element element => new Element(1, 1, 1);

	public override Action onCompleted => () => Debug.Log("유닛이 생산되다");

	public override bool isPlayer => true;//아군 적군 구분

	public override float healthPoint => 0f;//HP 설정

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