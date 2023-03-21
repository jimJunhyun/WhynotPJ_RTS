using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : IUnit
{
	public string name => "������";

	public float produceTime => 0.5f;

	public Element element => new Element(5, 0, 0, 5);

	public Action onCompleted => () => { Debug.Log("������ ���� �Ϸ�"); };
}
