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
		Debug.Log("아이템 더함.");
		produceQueue.Enqueue(pro);
		if (!isProducing) // ���� ����Ǵ� ������ ���� �� ���� ���� ����
			SetProduce();
	} 

	private void SetProduce()
	{
		if (produceQueue.Count == 0) return;
		
		item = produceQueue.Dequeue();
		
		Debug.Log(item._myName +" 생산 시작.");
		isProducing = true;
	}

    private void Produce()
	{
		if (item == null) return;
		//Debug.Log(item._myName + " 생산 완료.");
		MonoBehaviour obj = PoolManager.Instance.Pop(item._prefab.gameObject.name);
		obj.transform.position = SetSpawnPoint();
		IProducable finProduct = obj.GetComponent<IProducable>();
		UnitController c = obj.GetComponent<UnitController>();
		finProduct._onCompleted?.Invoke();
		if (pSide)
		{
			UnitSelectManager.Instance.unitList.Add(finProduct as UnitDefault);
		}
		else
		{
			EnemyPosGen.instance.myControls.Add(c);
		}
		

		item = null;
		isProducing = false;
		produceTime = 0;
		progress = 0;

		SetProduce();
	}

	private Vector3 SetSpawnPoint()
	{
		int angle = Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 2f;
		pos += transform.position + new Vector3(0, 0.5f, 0);
		return pos;
	}
}