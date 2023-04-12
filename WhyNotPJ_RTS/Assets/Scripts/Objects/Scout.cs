using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : IUnit
{
	public GameObject prefab => null;
	public string myName => "정찰병";

	public float produceTime => 0.5f;

	public Element element => new Element(2, 0, 0);

	public Action onCompleted => () => { EnemyPosGen.instance.myControls.Add(new Scout()); Debug.Log("정찰병 생산 완료"); };


	public Vector3 objPos {get; set;}
	public UnitState state {get;set;} = UnitState.Wait;

	public Transform target{ get;  set;}

	public IProducable Instantiate(Vector3 position = default, Quaternion rotation = default)
	{
		return null;
	}

	public void Move(Vector3 to)
	{
		objPos = to;
		Debug.Log(myName + "가 " + to + "로 움직였다.");
		EnemyBrain.instance.StartCoroutine(Tester(4f));
	}

	public void Move(Transform target)
	{
		this.target = target;
	}

	IEnumerator Tester(float sec)
	{
		state = UnitState.Moving;
		yield return new WaitForSeconds(sec);
		state = UnitState.Wait;
	}
}
