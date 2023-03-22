using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : IUnit
{
	public string myName => "������";

	public float produceTime => 0.5f;

	public Element element => new Element(5, 0, 0, 5);

	public Action onCompleted => () => { EnemyPosGen.instance.myControls.Add(this); Debug.Log("������ ���� �Ϸ�"); };

	public bool underControl { get; set; }

	public Vector3 pos {get; set;}

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
