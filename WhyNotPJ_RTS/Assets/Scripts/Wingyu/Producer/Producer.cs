using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
    public IProducable item;
	public bool isProducing = false;
	public bool pSide = false;

	public Queue<IProducable> produceQueue = new Queue<IProducable>();

	private float produceTime;
	private float progress;
	public float Progress => progress;

	private void Update()
	{
		if (isProducing)
		{
			Processing();
		}
	}

	private void Processing()
	{
		produceTime += Time.deltaTime;
		progress = produceTime / item.produceTime;

		if (produceTime >= item.produceTime)
		{
			Produce();
		}
	}

	/// <summary>
	/// 생산할 유닛을 대기목록 Queue에 추가
	/// </summary>
	/// <param name="pro"></param>
	public void AddProduct(IProducable pro)
	{
		produceQueue.Enqueue(pro);

		if (!isProducing) // 현재 생산되는 유닛이 없을 시 유닛 생산 시작
			SetProduce();
	} 

	private void SetProduce()
	{
		if (produceQueue.Count == 0) return;
		
		item = produceQueue.Dequeue();
		isProducing = true;
	}

    private void Produce()
	{
		if (item == null) return;

		// 유닛을 생산
		IProducable finProduct = Instantiate(item.prefab, SetSpawnPoint(), Quaternion.identity).GetComponent<IProducable>();
		finProduct.onCompleted?.Invoke();
		UnitManager.Instance.unitList.Add(finProduct as UnitDefault);

		// 변수 초기화
		item = null;
		isProducing = false;
		produceTime = 0;
		progress = 0;

		// 다음으로 생산될 유닛을 지정
		SetProduce();
	}

	private Vector3 SetSpawnPoint()
	{
		// 유닛이 생산될 위치를 반환
		int angle = Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 2f;
		pos += transform.position + new Vector3(0, 0.5f, 0);
		return pos;
	}
}