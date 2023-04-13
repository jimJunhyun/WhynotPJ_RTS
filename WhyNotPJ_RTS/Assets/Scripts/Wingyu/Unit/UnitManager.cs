using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� ���� ���� ��Ʈ�� Ŭ����. ���� ���� �ʿ�.
/// </summary>
public class UnitManager
{
	public static UnitManager Instance;

    private List<UnitDefault> selectedUnitList;		// ���õ� ���� ����Ʈ
	public List<UnitDefault> SelectedUnitList
	{
		get { return selectedUnitList; }
		set { selectedUnitList = value; }
	}
    public List<UnitDefault> unitList;				// ���ӻ� �����ϴ� ��� ���� ����Ʈ

	public UnitManager()
	{
		Instance = Instance == null ? this : Instance;
		selectedUnitList = new List<UnitDefault>();
		unitList = new List<UnitDefault>();
		GameObject.FindObjectsOfType<UnitDefault>().ToList().ForEach(unit => unitList.Add(unit));
	}

	// Ŭ���� ���� ���� ����
	public void ClickSelectUnit(UnitDefault newUnit)
	{
		SelectUnit(newUnit);
	}

	// �巡�׸� ���� ���� ����
	public void DragSelectUnit(UnitDefault newUnit)
	{
		if (!selectedUnitList.Contains(newUnit))
		{
			SelectUnit(newUnit);
		}
	}

	// ���� ��� ���� ����
	public void DeselectAll()
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].OnDeselectUnit();
		}

		selectedUnitList.Clear();
	}

	// �Ű������� �޾ƿ� ������ ����
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

	// �Ű������� �޾ƿ� ������ ���� ����
	private void DeselectUnit(UnitDefault newUnit)
	{
		newUnit.OnDeselectUnit();
		selectedUnitList.Remove(newUnit);
	}
}
