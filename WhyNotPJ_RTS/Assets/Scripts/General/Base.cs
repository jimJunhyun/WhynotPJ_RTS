using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ӽ�.
//������ ���� �Ұ��ϴ� �̷��� ��.
public class Base :  MonoBehaviour, IBuilding
{
    public List<UnitController> nearUnit{ get;set;} =  new List<UnitController>();

    public Vector3 scale { get;set; } = new Vector3(3,3,3);
    public Vector3 pos { get => transform.position; set => transform.position = value;}

	private void Awake()
	{
		transform.localScale = scale;
	}


}
