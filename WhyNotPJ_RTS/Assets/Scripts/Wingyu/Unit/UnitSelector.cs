using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 유닛 선택 컨트롤 클래스. 추후 개선 필요.
/// </summary>
public class UnitSelector : MonoBehaviour
{
	public static UnitSelector instance;

    private List<TestUnit> selectedUnitList;	// 선택된 유닛 리스트
    public List<TestUnit> unitList;				// 게임상에 존재하는 모든 유닛 리스트

	private void Awake()
	{
		instance = instance == null ? this : instance;
		selectedUnitList = new List<TestUnit>();
	}

	// 클릭을 통한 유닛 선택
	public void ClickSelectUnit(TestUnit newUnit)
	{
		SelectUnit(newUnit);
	}

	// 드래그를 통한 유닛 선택
	public void DragSelectUnit(TestUnit newUnit)
	{
		if (!selectedUnitList.Contains(newUnit))
		{
			SelectUnit(newUnit);
		}
	}

	// 유닛 모두 선택 해제
	public void DeselectAll()
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].DeselectUnit();
		}

		selectedUnitList.Clear();
	}

	// 매개변수로 받아온 유닛을 선택
	private void SelectUnit(TestUnit newUnit)
	{
		if (selectedUnitList.Contains(newUnit))
		{
			DeselectUnit(newUnit);
		}
		else
		{
			newUnit.SelectUnit();
			selectedUnitList.Add(newUnit);
		}
	}

	// 매개변수로 받아온 유닛을 선택 해제
	private void DeselectUnit(TestUnit newUnit)
	{
		newUnit.DeselectUnit();
		selectedUnitList.Remove(newUnit);
	}
}
