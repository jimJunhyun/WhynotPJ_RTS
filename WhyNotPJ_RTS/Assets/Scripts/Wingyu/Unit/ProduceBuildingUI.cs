using System.Collections;
using System.Collections.Generic;
using TMPro;
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
			button.GetComponentInChildren<TextMeshProUGUI>().text = proB.unitList[i].gameObject.name;

			int number = i;
			button.onClick.AddListener(() => proB.AddQueue(proB.unitList[number]));
		}
	}
}
