using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProduceBuildingUI : MonoBehaviour
{
	[SerializeField] private Transform contentTrm;
	[SerializeField] private Button buttonPrefab;

	public void MakeButton(int count, ProduceBuilding proB)
	{
		for (int i = 0; i < count; i++)
		{
			print(i);
			Button button = Instantiate(buttonPrefab, contentTrm);
			button.onClick.AddListener(() => proB.AddQueue(proB.unitList[i - 1]));
		}
	}
}
