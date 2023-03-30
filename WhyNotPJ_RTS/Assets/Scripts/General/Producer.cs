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
	/// 생산할 유닛을 대기목록 Queue에 추가
	/// </summary>
	/// <param name="pro"></param>
	public void AddProduct(IProducable pro)
	{
		produceQueue.Enqueue(pro);
		produceQueueGameObjects.Add(pro.prefab);

		if (!isProducing) // 현재 생산되는 유닛이 없을 시 유닛 생산 시작
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

		// 유닛을 생산
		IProducable product = Instantiate(producing.prefab, SetSpawnPoint(), Quaternion.identity).GetComponent<IProducable>();
		product.onCompleted?.Invoke();
		UnitSelector.instance.unitList.Add(product as TestUnit);

		producing = null;
		isProducing = false;
		produceTime = 0;

		// 다음으로 생산될 유닛을 지정
		SetProduce();
	}

	private Vector3 SetSpawnPoint()
	{
		// 유닛이 생산될 위치를 반환
		int angle = UnityEngine.Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 2f;
		pos += transform.position;
		return pos;
	}
}