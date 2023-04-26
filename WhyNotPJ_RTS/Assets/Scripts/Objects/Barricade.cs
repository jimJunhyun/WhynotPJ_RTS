using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : IBuilding, IProducable
{
	public string myName = "���Ľ��ǽ�Ʈ";

	public float produceTime = 1.5f;

	public Element element = new Element(2, 9, 5);

	public Action onCompleted = () => { Debug.Log("���Ľ��ǽ�Ʈ �Ǽ� �Ϸ�"); };
    //��ó�� ����� ���� �Է��ϰų� SerializesField ������ �����Ϳ��� �Է�
    public string _myName => myName;
    public float _produceTime => produceTime;
    public Element _element => element;
    public Action _onCompleted => onCompleted;

    public List<UnitController> nearUnit { get; set; }

	public Vector3 scale { get;} = new Vector3(1,1,1);
	public Vector3 pos { get; set; }
}
