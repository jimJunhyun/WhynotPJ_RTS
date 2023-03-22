using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : IBuilding
{
	public string myName => "바리케이드";

	public float produceTime => 1.5f;

	public Element element => new Element(0, 4, 4, 0);

	public Action onCompleted => () => { Debug.Log("바리케이드 건설 완료"); };
}
