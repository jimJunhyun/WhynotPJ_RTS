using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
	[SerializeField]
	private Transform spawnPosition;
    public IProducable item;
	public bool isProducing = false;
	public bool pSide = false;

	public Queue<IProducable> produceQueue = new Queue<IProducable>();

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
		progress = produceTime / item._produceTime;

		if (produceTime >= item._produceTime)
		{
			Produce();
		}
	}

	public void AddProduct(IProducable pro)
	{
		produceQueue.Enqueue(pro);

		if (!isProducing)
			SetProduce();
	} 

	private void SetProduce()
	{
		if (produceQueue.Count == 0) return;
		
		item = produceQueue.Dequeue();
		isProducing = true;
	}

    private IProducable Produce()
	{
		if (item == null) return null;

		UnitController obj = PoolManager.Instance.Pop(item._prefab.gameObject.name) as UnitController;
		obj._pSide = pSide;
		obj.transform.position = SetSpawnPoint();
		IProducable finProduct = obj.GetComponent<IProducable>();
		finProduct._onCompleted?.Invoke();
		UnitSelectManager.Instance.unitList.Add(obj);

		item = null;
		isProducing = false;
		produceTime = 0;
		progress = 0;

		SetProduce();

		return finProduct;
	}

	private Vector3 SetSpawnPoint()
	{
		int angle = Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 2f;
		pos += transform.position;
		return pos;
	}
}