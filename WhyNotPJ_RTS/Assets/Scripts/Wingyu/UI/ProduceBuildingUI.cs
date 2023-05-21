using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProduceBuildingUI : MonoBehaviour
{
	[SerializeField]
	private ProduceableListSO produceList;
	[SerializeField]
	private ProductionButton prodBtnPrefab;
	[SerializeField]
	private Transform btnParent;
	[SerializeField]
	private Producer producer;

	private void Start()
	{
		foreach (UnitController unit in produceList.list)
		{
			ProductionButton button = PoolManager.Instance.Pop(prodBtnPrefab.gameObject.name) as ProductionButton;
			button.gameObject.transform.SetParent(btnParent);
			button.SetData(unit, producer);
		}
	}
}
