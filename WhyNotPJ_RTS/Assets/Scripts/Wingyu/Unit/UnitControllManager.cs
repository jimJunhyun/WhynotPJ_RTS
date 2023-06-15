using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���� ���� ����� �׽�Ʈ�ϱ� ���� ���� ���� ��Ʈ�� Ŭ����. ���� ���� �ʿ�.
/// </summary>
public class UnitControllManager : MonoBehaviour
{
	public static UnitControllManager Instance;

    public	List<ISelectable> unitList;			// ���ӻ� �����ϴ� ��� ���� ����Ʈ
    private List<ISelectable> selectedList;		// ���õ� ���� ����Ʈ
	public	List<UnitController> SelectedUnitList => ( // ���õ� ��� ���� �߿��� UnitController Ÿ���� ���ָ� ã�Ƽ� ��ȯ
		from unit in selectedList
		where unit.GetType() == typeof(UnitController) 
		select unit as UnitController
		).ToList();

	public bool IsSelecting => selectedList.Count > 0;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		selectedList = new List<ISelectable>();
		unitList = new List<ISelectable>();
		unitList = GameObject.FindObjectsOfType<UnitController>().ToList<ISelectable>();
	}

	// Ŭ���� ���� ���� ����
	public void ClickSelectUnit(ISelectable newUnit)
	{
		if (selectedList.Count > 1 || !selectedList.Contains(newUnit))
			DeselectAll();
		SelectUnit(newUnit);
	}

	// �巡�׸� ���� ���� ����
	public void DragSelectUnit(ISelectable newUnit)
	{
		if (!selectedList.Contains(newUnit))
		{
			SelectUnit(newUnit);
		}
	}

	// ���� ��� ���� ����
	public void DeselectAll()
	{
		for (int i = 0; i < selectedList.Count; ++i)
		{
			selectedList[i].OnDeselectUnit();
		}

		selectedList.Clear();
	}

	// �Ű������� �޾ƿ� ������ ����
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

	// �Ű������� �޾ƿ� ������ ���� ����
	private void DeselectUnit(ISelectable newUnit)
	{
		newUnit.OnDeselectUnit();
		selectedList.Remove(newUnit);
	}
}
