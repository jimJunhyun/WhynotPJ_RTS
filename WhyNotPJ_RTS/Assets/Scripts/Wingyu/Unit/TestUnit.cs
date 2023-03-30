using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 임시 유닛 클래스. 추후 바뀔 수 있다.
/// </summary>
public class TestUnit : MonoBehaviour, IUnit
{
	// IUnit
	public GameObject prefab => gameObject;
	public string myName => "테스트 유닛";

	public float produceTime => 1f;

	public Element element => new Element(1, 1, 1);

	public Action onCompleted => () => Debug.Log("유닛이 생산됬어여");

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