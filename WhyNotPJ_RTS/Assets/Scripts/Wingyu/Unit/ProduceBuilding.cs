using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceBuilding : MonoBehaviour, IBuilding, ISelectable
{
	public List<IUnit> nearUnit { get; set; }
	public Vector3 scale { get; set; }
	public Vector3 pos { get; set; }

	public bool CanDragSelect => false;
	public Vector3 WorldPos => transform.position;

	[SerializeField] private Producer producer;

	public List<UnitDefault> unitList = new List<UnitDefault>();

	public ProduceBuildingUI proUI;

	private void Awake()
	{
		proUI.MakeButton(unitList.Count, this);
	}

	public void AddQueue<T>(T unit) where T : IProducable
	{
		producer.AddProduct(unit);
	}

	public void OnDeselectUnit()
	{
		
	}

	public void OnSelectUnit()
	{
		print("Selected");
	}
}
