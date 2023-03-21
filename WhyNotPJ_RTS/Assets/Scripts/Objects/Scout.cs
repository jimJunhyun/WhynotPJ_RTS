using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : IUnit
{
	public string name => "정찰병";

	public float produceTime => 0.5f;

	public Element element => new Element(5, 0, 0, 5);

	public Action onCompleted => () => { Debug.Log("정찰병 생산 완료"); };
}
