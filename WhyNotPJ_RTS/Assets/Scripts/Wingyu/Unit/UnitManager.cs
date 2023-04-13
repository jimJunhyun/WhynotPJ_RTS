using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 유닛 선택 컨트롤 클래스. 추후 개선 필요.
/// </summary>
public class UnitManager
{
	public static UnitManager Instance;

    private List<UnitDefault> selectedUnitList;		// 선택된 유닛 리스트
	public List<UnitDefault> SelectedUnitList
	{
		get { return selectedUnitList; }
		set { selectedUnitList = value; }
	}
    public List<UnitDefault> unitList;				// 게임상에 존재하는 모든 유닛 리스트

	public UnitManager()
	{
		Instance = Instance == null ? this : Instance;
		selectedUnitList = new List<UnitDefault>();
		unitList = new List<UnitDefault>();
		GameObject.FindObjectsOfType<UnitDefault>().ToList().ForEach(unit => unitList.Add(unit));
	}

	// 클릭을 통한 유닛 선택
	public void ClickSelectUnit(UnitDefault newUnit)
	{
		SelectUnit(newUnit);
	}

	// 드래그를 통한 유닛 선택
	public void DragSelectUnit(UnitDefault newUnit)
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
			selectedUnitList[i].OnDeselectUnit();
		}

		selectedUnitList.Clear();
	}

	// 매개변수로 받아온 유닛을 선택
	private void SelectUnit(UnitDefault newUnit)
	{
		if (selectedUnitList.Contains(newUnit))
		{
			DeselectUnit(newUnit);
		}
		else
		{
			newUnit.OnSelectUnit();
			selectedUnitList.Add(newUnit);
		}
	}

	// 매개변수로 받아온 유닛을 선택 해제
	private void DeselectUnit(UnitDefault newUnit)
	{
		newUnit.OnDeselectUnit();
		selectedUnitList.Remove(newUnit);
	}
}
