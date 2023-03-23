using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 선택 기능을 테스트하기 위한 유닛 선택 컨트롤 클래스. 추후 개선 필요.
/// </summary>
public class UnitController : MonoBehaviour
{
    private List<TestUnit> selectedUnitList;
    public List<TestUnit> unitList;

	private void Awake()
	{
		selectedUnitList = new List<TestUnit>();
	}

	public void ClickSelectUnit(TestUnit newUnit)
	{
		SelectUnit(newUnit);
	}

	public void DragSelectUnit(TestUnit newUnit)
	{
		if (!selectedUnitList.Contains(newUnit))
		{
			SelectUnit(newUnit);
		}
	}

	public void DeselectAll()
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].DeselectUnit();
		}

		selectedUnitList.Clear();
	}

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

	private void DeselectUnit(TestUnit newUnit)
	{
		newUnit.DeselectUnit();
		selectedUnitList.Remove(newUnit);
	}
}
