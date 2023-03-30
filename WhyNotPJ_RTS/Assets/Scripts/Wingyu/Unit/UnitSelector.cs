using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� ���� ���� ��Ʈ�� Ŭ����. ���� ���� �ʿ�.
/// </summary>
public class UnitSelector : MonoBehaviour
{
	public static UnitSelector instance;

    private List<TestUnit> selectedUnitList;	// ���õ� ���� ����Ʈ
    public List<TestUnit> unitList;				// ���ӻ� �����ϴ� ��� ���� ����Ʈ

	private void Awake()
	{
		instance = instance == null ? this : instance;
		selectedUnitList = new List<TestUnit>();
	}

	// Ŭ���� ���� ���� ����
	public void ClickSelectUnit(TestUnit newUnit)
	{
		SelectUnit(newUnit);
	}

	// �巡�׸� ���� ���� ����
	public void DragSelectUnit(TestUnit newUnit)
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
			selectedUnitList[i].DeselectUnit();
		}

		selectedUnitList.Clear();
	}

	// �Ű������� �޾ƿ� ������ ����
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

	// �Ű������� �޾ƿ� ������ ���� ����
	private void DeselectUnit(TestUnit newUnit)
	{
		newUnit.DeselectUnit();
		selectedUnitList.Remove(newUnit);
	}
}
