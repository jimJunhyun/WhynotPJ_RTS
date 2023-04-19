using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceBuilding : MonoBehaviour, IBuilding, ISelectable
{
	public Vector3 scale { get; set; }
	public Vector3 pos { get; set; }

	public bool CanDragSelect => false;
	public Vector3 WorldPos => transform.position;

	private Producer producer;

	public List<UnitController> unitList = new List<UnitController>();

	public ProduceBuildingUI producerUI;

	private void Awake()
	{
		producerUI.MakeButton(unitList.Count, this);
		producer = gameObject.AddComponent<Producer>();
	}

	public void AddQueue<T>(T unit) where T : IProducable
	{
		producer.AddProduct(unit);
	}

	public void OnDeselectUnit()
	{
		producerUI.ShowUI(false);
	}

	public void OnSelectUnit()
	{
		producerUI.ShowUI(true);
	}
}
