using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : IUnit
{
	public string name => "����";
	public float produceTime => 1f;

	public Element element => new Element(4, 4, 0, 0);

	public Action onCompleted => ()=>{ Debug.Log("���� ���� �Ϸ�."); };

	
}
