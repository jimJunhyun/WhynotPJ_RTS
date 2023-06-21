using System.Collections.Generic;
using UnityEngine;

public class ProducingUnit
{
	public ProducingUnit(IProducable unit, float produceTime, UnitProduceButton infoUI)
	{
		this.unit = unit;
		this.produceTime = produceTime;
		this.infoUI = infoUI;
	}

	public IProducable unit;
	public float produceTime;
	public UnitProduceButton infoUI;
}

public class Producer : MonoBehaviour
{
    public ProducingUnit item; //현재 생산중인 유닛
	public bool isProducing = false;
	public bool pSide = false;

	public List<ProducingUnit> produceList = new List<ProducingUnit>(); //생산 대기 리스트

	private float produceTime; //생산 시간
	private float progress; //생산 진행도
	public float Progress => progress;

	// 유닛을 생산 리스트에 추가
	public void AddProduct(IProducable pro, UnitProduceButton produceBtn = null)
	{
		StopProduce();

		ProducingUnit unit = produceList.Find(p => p.unit.myName == pro.myName);
		if (unit != null)
		{

		}
		else
		{
			Debug.Log("아이템 더함.");
			unit = new ProducingUnit(pro, 0, produceBtn);
		}
		item = unit;
		produceTime = item.produceTime;
		isProducing = true;
	} 

	// 우선 순위가 가장 높은 유닛을 생산 시작
	private void SetProduce()
	{
		if (produceList.Count == 0) return;
		
		item = produceList[0];
		produceList.RemoveAt(0);
		produceTime = item.produceTime;
		
		Debug.Log(item.unit.myName +" 생산 시작.");
		isProducing = true;
	}

	// 생산을 중단하고 생산하던 유닛을 리스트로 도로 넣음
	public void StopProduce()
	{
		if (item == null)
			return;

		isProducing = false;
		produceList.Add(item);
	}

	private void Update()
	{
		if (!isProducing) return;

		Processing();
	}

	private void Processing()
	{
		produceTime += Time.deltaTime;
		progress = produceTime / item.unit.produceTime;

		item.produceTime = produceTime;
		item.infoUI?.SetTimeAndSlider(item.unit.produceTime - produceTime, progress);

		if (produceTime >= item.unit.produceTime)
		{
			Produce();
		}
	}

    private void Produce()
	{
		if (item == null) return;

		item.produceTime = 0;

		//Debug.Log(item._myName + " 생산 완료.");
		MonoBehaviour obj = PoolManager.Instance.Pop(item.unit.prefab.gameObject.name);
		obj.transform.position = SetSpawnPoint();
		IProducable finProduct = obj.GetComponent<IProducable>();
		UnitController unit = obj.GetComponent<UnitController>();
		finProduct.onCompleted?.Invoke();
		unit.isPlayer = pSide;
		if (pSide)
		{
			UnitSelectManager.Instance.unitList.Add(unit);
		}
		else
		{
			EnemyPosGen.instance.myControls.Add(new UnitManageData(unit, true));
		}

		item = null;
		isProducing = false;
		produceTime = 0;
		progress = 0;
	}

	private Vector3 SetSpawnPoint()
	{
		int angle = Random.Range(0, 361);
		Vector3 pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 7f;
		pos += transform.position + new Vector3(0, 0.5f, 0);
		return pos;
	}
}