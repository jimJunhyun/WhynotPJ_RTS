using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : IUnit
{
	public string myName => "정찰병";

	public float produceTime => 0.5f;

	public Element element => new Element(5, 0, 0, 5);

	public Action onCompleted => () => { EnemyPosGen.instance.myControls.Add(this); Debug.Log("정찰병 생산 완료"); };

	public bool underControl { get; set; }

	public Vector3 pos {get; set;}

	public void Move(Vector3 to)
	{
		pos = to;
		Debug.Log(myName + "가 " + to + "로 움직였다.");
		EnemyBrain.instance.StartCoroutine(Tester(4f));
	}

	IEnumerator Tester(float sec)
	{
		underControl = true;
		yield return new WaitForSeconds(sec);
		underControl = false;
	}
}
