using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : IBuilding, IProducable
{
	public GameObject prefab => null;
	public string myName => "바리케이드";

	public float produceTime => 1.5f;

	public Element element => new Element(0, 9, 7);

	public Action onCompleted => () => { Debug.Log("바리케이드 건설 완료"); };

	public List<IUnit> nearUnit { get; set; }

	public Vector3 scale { get;} = new Vector3(1,1,1);
	public Vector3 pos { get; set; }
}
