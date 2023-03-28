using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//임시. 나중에는 new Barricade() 처럼 가져올듯
//본진은 생산 불가하니 이렇게 함.
public class Base :  MonoBehaviour, IBuilding
{
    public List<IUnit> nearUnit{ get;set;} =  new List<IUnit>();

    public Vector3 scale { get;set; } = new Vector3(3,3,3);
    public Vector3 pos { get => transform.position; set => transform.position = value;}

	private void Awake()
	{
		transform.localScale = scale;
	}


}
