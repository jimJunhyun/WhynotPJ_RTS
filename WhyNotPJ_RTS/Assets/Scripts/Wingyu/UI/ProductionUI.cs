using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionUI : MonoBehaviour
{
	[SerializeField]
	private ProduceableListSO producableUnitList;
	[SerializeField]
	private UnitProduceButton prodBtnPrefab;
	[SerializeField]
	private Transform unitParent;
	[SerializeField]
	private Transform engineeringParent;
	[SerializeField]
	private Producer producer;
	[SerializeField]
	private ConstructBuild constructer;

	private void Start()
	{
		foreach (UnitController unit in producableUnitList.list)
		{
			UnitProduceButton button = PoolManager.Instance.Pop(prodBtnPrefab.gameObject.name) as UnitProduceButton;
			button.gameObject.transform.SetParent(unitParent);
			button.SetData(unit, producer);
		}

		for (int i = 0; i < System.Enum.GetValues(typeof(Buildables)).Length; i++)
		{
			UnitProduceButton button = PoolManager.Instance.Pop(prodBtnPrefab.gameObject.name) as UnitProduceButton;
			button.gameObject.transform.SetParent(engineeringParent);
			button.SetData((Buildables)i, constructer);
		}
	}
}
