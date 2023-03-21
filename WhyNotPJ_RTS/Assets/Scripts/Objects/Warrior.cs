using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : IUnit
{
	public string name => "전사";
	public float produceTime => 1f;

	public Element element => new Element(4, 4, 0, 0);

	public Action onCompleted => ()=>{ Debug.Log("전사 생산 완료."); };

	
}
