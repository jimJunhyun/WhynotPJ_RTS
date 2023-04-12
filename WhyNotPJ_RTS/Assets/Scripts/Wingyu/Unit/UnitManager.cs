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

    private List<ISelectable> selectedUnitList;	// ���õ� ���� ����Ʈ
    public List<ISelectable> unitList;				// ���ӻ� �����ϴ� ��� ���� ����Ʈ

	public UnitManager()
	{
		Instance = Instance == null ? this : Instance;
		selectedUnitList = new List<ISelectable>();
		unitList = new List<ISelectable>();
		GameObject.FindObjectsOfType<UnitDefault>().ToList<ISelectable>().ForEach(unit => unitList.Add(unit));
	}

	// Ŭ���� ���� ���� ����
	public void ClickSelectUnit(ISelectable newUnit)
	{
		SelectUnit(newUnit);
	}

	// �巡�׸� ���� ���� ����
	public void DragSelectUnit(ISelectable newUnit)
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
	private void SelectUnit(ISelectable newUnit)
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
	private void DeselectUnit(ISelectable newUnit)
	{
		newUnit.OnDeselectUnit();
		selectedUnitList.Remove(newUnit);
	}
}
