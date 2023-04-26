using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : IBuilding, IProducable
{
	public string myName = "히파스피스트";

	public float produceTime = 1.5f;

	public Element element = new Element(2, 9, 5);

	public Action onCompleted = () => { Debug.Log("히파스피스트 건설 완료"); };
    //위처럼 상수로 직접 입력하거나 SerializesField 등으로 에디터에서 입력
    public string _myName => myName;
    public float _produceTime => produceTime;
    public Element _element => element;
    public Action _onCompleted => onCompleted;

    public List<UnitController> nearUnit { get; set; }

	public Vector3 scale { get;} = new Vector3(1,1,1);
	public Vector3 pos { get; set; }
}
