using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMono
{
	public MonoBehaviour mono;
	public Button button;
}

public class ProduceBuildingUI : MonoBehaviour
{
	[SerializeField] private Transform contentTrm;
	[SerializeField] private Button buttonPrefab;

	private GameObject uiParent;

	private ProduceBuilding produceBuilding;

	private List<ButtonMono> buttonMonos = new List<ButtonMono>();

	private void Awake()
	{
		uiParent = GameObject.Find("UnitProducer");
	}

	private void Start()
	{
		uiParent.SetActive(false);
	}

	public void SetButton(int count, ProduceBuilding proB)
	{
		produceBuilding = proB;

		for (int i = 0; i < count; i++)
		{
			ButtonMono bm = new ButtonMono();
			bm.mono = PoolManager.Instance.Pop(buttonPrefab.gameObject.name);
			bm.button = bm.mono.GetComponent<Button>();
			bm.button.transform.SetParent(contentTrm);
			bm.button.GetComponentInChildren<TextMeshProUGUI>().text = produceBuilding.unitList[i]._myName;

			int number = i;
			bm.button.onClick.AddListener(() => produceBuilding.AddQueue(produceBuilding.unitList[number]));
			buttonMonos.Add(bm);
		}
	}

	public void UnsetButton()
	{
		if (buttonMonos.Count == 0) return; 

		for (int i = 0; i < buttonMonos.Count; i++)
		{
			buttonMonos[i].button.onClick.RemoveListener(() => produceBuilding.AddQueue(produceBuilding.unitList[i]));
			PoolManager.Instance.Push(buttonMonos[i].mono);
		}

		produceBuilding = null;
	}

	public void ShowUI(bool show)
	{
		uiParent.SetActive(show);
	}
}
