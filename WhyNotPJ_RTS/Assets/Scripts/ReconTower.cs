using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconTower : IBuilding
{
	public string name => "���� Ÿ��";

	public float produceTime => 3f;

	public Element element => new Element(0,1, 3, 6);

	public Action onCompleted => ()=>{ Debug.Log("����Ÿ�� �Ǽ� �Ϸ�"); };
}
