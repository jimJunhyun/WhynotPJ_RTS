using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitProduceButton : PoolableMono
{
    private UnitController unit;
	private Buildables buildable;

	[SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timeText;
	[SerializeField] private Image slider;
	private Button button;

	private Producer producer;
	private ConstructBuild construct;

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Init()
	{
		transform.localScale = Vector3.one;
		timeText.gameObject.SetActive(false);
	}

	public void SetData(UnitController nextUnit, Producer producer)
	{
		Init();

		unit = nextUnit;
		nameText.text = unit.name;
		this.producer = producer;
		button.onClick.AddListener(ProduceUnit);
		image.sprite = nextUnit.image;
	}

	public void SetData(Buildables buildable, ConstructBuild construct)
	{
		Init();

		this.buildable = buildable;
		nameText.text = buildable.ToString();
		this.construct = construct;
		button.onClick.AddListener(ConstructBuilding);
	}

	public void SetTimeAndSlider(float time, float percent)
	{
		slider.fillAmount = percent;

		if (time <= 0)
		{
			timeText.gameObject.SetActive(false);
			return;
		}

		timeText.gameObject.SetActive(true);
		timeText.text = $"{(int)time}s";
	}

	public void ProduceUnit()
	{
		producer.AddProduct(unit, this);
	}

	public void ConstructBuilding()
	{
		construct.StartCoroutine(construct.BuildInp(buildable));
	}
}
