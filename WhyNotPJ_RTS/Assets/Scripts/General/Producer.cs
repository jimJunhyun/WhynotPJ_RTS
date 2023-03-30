using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
	public TestUnit testUnit;
    public IProducable producing;
	public bool isProducing = false;
	public bool pSide = false;

	public Queue<IProducable> produceQueue = new Queue<IProducable>();

	public List<GameObject> produceQueueGameObjects = new List<GameObject>();

	private float produceTime;

	private void Update()
	{
		if (isProducing)
		{
			produceTime += Time.deltaTime;

			if (produceTime >= producing.produceTime)
			{
				Produce();
			}
		}
	}

	/// <summary>
	/// ������ ������ ����� Queue�� �߰�
	/// </summary>
	/// <param name="pro"></param>
	public void AddProduct(IProducable pro)
	{
		produceQueue.Enqueue(pro);
		produceQueueGameObjects.Add(pro.prefab);

		if (!isProducing) // ���� ����Ǵ� ������ ���� �� ���� ���� ����
			SetProduce();
	} 

	private void SetProduce()
	{
		if (produceQueue.Count == 0) return;
		
		producing = produceQueue.Dequeue();
		isProducing = true;
	}

    private void Produce()
	{
		if (producing == null) return;

		produceQueueGameObjects.RemoveAt(0);

		// ������ ����
		IProducable product = Instantiate(producing.prefab, SetSpawnPoint(), Quaternion.identity).GetComponent<IProducable>();
		product.onCompleted?.Invoke();
		UnitSelector.instance.unitList.Add(product as TestUnit);

		producing = null;
		isProducing = false;
		produceTime = 0;

		// �������� ����� ������ ����
		SetProduce();
	}

	private Vector3 SetSpawnPoint()
	{
		// ������ ����� ��ġ�� ��ȯ
		int angle = UnityEngine.Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 2f;
		pos += transform.position;
		return pos;
	}
}