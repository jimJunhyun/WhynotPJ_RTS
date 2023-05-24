using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Process
{
	public Process(IProducable item, float produceTime, Slider progressSlider)
	{
		this.item = item;
		this.produceTime = produceTime;
		this.progressSlider = progressSlider;
	}

	public IProducable item;
	public float produceTime;
	public Slider progressSlider;
}

public class Producer : MonoBehaviour
{
	[SerializeField]
	private Transform spawnPosition;
    public IProducable item;
	public Process curItem;
	public bool isProducing = false;
	public bool pSide = false;

	public Queue<IProducable> produceQueue = new Queue<IProducable>();
	public Stack<Process> processStack = new Stack<Process>();

	private float produceTime;
	private float progress;
	public float Progress => progress;

	private void Update()
	{
		if (!isProducing) return;

		Processing();
	}

	private void Processing()
	{
		produceTime += Time.deltaTime;
		progress = produceTime / curItem.item._produceTime;
		
		if (curItem.progressSlider)
			curItem.progressSlider.value = progress;
		curItem.produceTime = produceTime;

		if (produceTime >= curItem.item._produceTime)
		{
			curItem.progressSlider?.gameObject.SetActive(false);
			MakeProduce();
		}

	}

    private IProducable MakeProduce()
	{
		if (curItem == null) return null;

		UnitController item = PoolManager.Instance.Pop(curItem.item._prefab.gameObject.name) as UnitController;
		item._pSide = pSide;
		item.transform.position = SetSpawnPoint();
		IProducable finProduct = item.GetComponent<IProducable>();
		finProduct._onCompleted?.Invoke();
		UnitSelectManager.Instance.unitList.Add(item);

		processStack.Pop();
		Init();
		SetProduce();

		return finProduct;
	}

	private void Init()
	{
		curItem = null;
		isProducing = false;
		produceTime = 0;
		progress = 0;
	}

	public void AddProduce(IProducable pro, Slider slider = null)
	{
		if (slider.gameObject.activeInHierarchy) return;
		slider?.gameObject.SetActive(true);

		Init();
		Process process = new Process(pro, 0, slider);
		processStack.Push(process);

		if (!isProducing)
			SetProduce();
	} 

	private void SetProduce()
	{
		if (processStack.Count == 0) return;

		curItem = processStack.Peek();
		produceTime = curItem.produceTime;
		
		isProducing = true;
	}

	// 스폰 위치 공식
	private Vector3 SetSpawnPoint()
	{
		int angle = Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 2f;
		pos += transform.position;
		return pos;
	}
}