using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconTower : IBuilding
{
	public string name => "정찰 타워";

	public float produceTime => 3f;

	public Element element => new Element(0,1, 3, 6);

	public Action onCompleted => ()=>{ Debug.Log("정찰타워 건설 완료"); };
}
