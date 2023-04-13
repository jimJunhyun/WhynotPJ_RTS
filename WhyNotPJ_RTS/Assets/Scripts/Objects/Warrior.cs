using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : IUnit
{
	public string myName => "����";
	public float produceTime => 1f;

	public Element element => new Element(7, 7, 7);

	public Action onCompleted => ()=>{ EnemyPosGen.instance.myControls.Add(new Warrior()); Debug.Log("���� ���� �Ϸ�."); };

	public bool underControl { get; set; } = false;

	public Vector3 objPos { get; set;}
	public UnitState state { get; set; } = UnitState.Wait;

	public Transform target { get; set;}

	public void Move(Vector3 to)
	{
		objPos = to;
		Debug.Log(myName + "�� " + to + "�� ��������.");
		EnemyBrain.instance.StartCoroutine(Tester(4f));
	}

	public void Move(Transform target)
	{
		Debug.Log(myName + "�� " + target.position + "�� ��������.");
	}

	IEnumerator Tester(float sec)
	{
		underControl = true;
		yield return new WaitForSeconds(sec);
		underControl = false;
	}
}
