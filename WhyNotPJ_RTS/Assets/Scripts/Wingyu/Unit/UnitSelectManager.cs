using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Units
{
}

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 유닛 선택 컨트롤 클래스. 추후 개선 필요.
/// </summary>
public class UnitSelectManager
{
	public static UnitSelectManager Instance;

    public	List<ISelectable> unitList;			// 게임상에 존재하는 모든 유닛 리스트
    private List<ISelectable> selectedList;		// 선택된 유닛 리스트
	public	List<UnitController> SelectedUnitList => ( // 선택된 모든 유닛 중에서 UnitController 타입인 유닛만 찾아서 반환
		from unit in selectedList
		where unit.GetType() == typeof(UnitController) 
		select unit as UnitController
		).ToList();

	public UnitSelectManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		selectedList = new List<ISelectable>();
		unitList = new List<ISelectable>();
		unitList = GameObject.FindObjectsOfType<UnitController>().ToList<ISelectable>();
	}

	// 클릭을 통한 유닛 선택
	public void ClickSelectUnit(ISelectable newUnit)
	{
		if (selectedList.Count > 1 || !selectedList.Contains(newUnit))
			DeselectAll();
		SelectUnit(newUnit);
	}

	// 드래그를 통한 유닛 선택
	public void DragSelectUnit(ISelectable newUnit)
	{
		if (!selectedList.Contains(newUnit))
		{
			SelectUnit(newUnit);
		}
	}

	// 유닛 모두 선택 해제
	public void DeselectAll()
	{
		for (int i = 0; i < selectedList.Count; ++i)
		{
			selectedList[i].OnDeselectUnit();
		}

		selectedList.Clear();
	}

	// 매개변수로 받아온 유닛을 선택
	private void SelectUnit(ISelectable newUnit)
	{
		if (selectedList.Contains(newUnit))
		{
			DeselectUnit(newUnit);
		}
		else
		{
			selectedList.Add(newUnit);
			newUnit.OnSelectUnit();
		}
	}

	// 매개변수로 받아온 유닛을 선택 해제
	private void DeselectUnit(ISelectable newUnit)
	{
		newUnit.OnDeselectUnit();
		selectedList.Remove(newUnit);
	}
}
