using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : IUnit
{
	public string myName => "����";
	public float produceTime => 1f;

	public Element element => new Element(4, 4, 0, 0);

	public Action onCompleted => ()=>{ EnemyPosGen.instance.myControls.Add(this); Debug.Log("���� ���� �Ϸ�."); };

	public bool underControl { get; set; } = false;

	public Vector3 pos { get; set;}

	public void Move(Vector3 to)
	{
		pos = to;
		Debug.Log(myName + "�� " + to + "�� ��������.");
		EnemyBrain.instance.StartCoroutine(Tester(4f));
	}

	IEnumerator Tester(float sec)
	{
		underControl = true;
		yield return new WaitForSeconds(sec);
		underControl = false;
	}
}
