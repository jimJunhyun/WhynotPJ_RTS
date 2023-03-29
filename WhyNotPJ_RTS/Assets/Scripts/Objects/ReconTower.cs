using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconTower : IBuilding, IProducable
{
	public string myName => "���� Ÿ��";

	public float produceTime => 3f;

	public Element element => new Element(0,1, 8);

	public Action onCompleted => ()=>{ Debug.Log("����Ÿ�� �Ǽ� �Ϸ�"); };

	public List<IUnit> nearUnit { get; set; }

	public Vector3 scale{ get;} = new Vector3(1, 1, 1);
	public Vector3 pos { get; set; }
}
