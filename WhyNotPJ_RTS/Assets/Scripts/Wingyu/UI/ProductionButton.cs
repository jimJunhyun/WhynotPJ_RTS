using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionButton : PoolableMono
{
    private UnitController unit;

    private TextMeshProUGUI textmesh;
	private Button button;

	private Producer producer;

	private void Init()
	{
		textmesh = transform.Find("Text").GetComponent<TextMeshProUGUI>();
		button = GetComponent<Button>();
		transform.localScale = Vector3.one;
	}

	public void SetData(UnitController nextUnit, Producer producer)
	{
		Init();

		unit = nextUnit;
		textmesh.text = unit.name;
		this.producer = producer;
		button.onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		producer.AddProduct(unit);
	}
}
