using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : IBuilding
{
	public string myName => "�ٸ����̵�";

	public float produceTime => 1.5f;

	public Element element => new Element(0, 4, 4, 0);

	public Action onCompleted => () => { Debug.Log("�ٸ����̵� �Ǽ� �Ϸ�"); };
}
