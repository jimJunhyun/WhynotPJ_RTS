using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : IUnit
{
	public string myName => "전사";
	public float produceTime => 1f;

	public Element element => new Element(4, 4, 0, 0);

	public Action onCompleted => ()=>{ EnemyPosGen.instance.myControls.Add(this); Debug.Log("전사 생산 완료."); };

	public bool underControl { get; set; } = false;

	public Vector3 pos { get; set;}

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
