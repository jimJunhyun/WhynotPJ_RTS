using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProducerUI : MonoBehaviour
{
	[SerializeField]
	private ProduceableListSO produceList;
	[SerializeField]
	private UnitProduceButton prodBtnPrefab;
	[SerializeField]
	private Transform btnParent;
	[SerializeField]
	private Producer producer;

	private void Start()
	{
		foreach (UnitController unit in produceList.list)
		{
			UnitProduceButton button = PoolManager.Instance.Pop(prodBtnPrefab.gameObject.name) as UnitProduceButton;
			button.gameObject.transform.SetParent(btnParent);
			button.SetData(unit, producer);
		}
	}
}
