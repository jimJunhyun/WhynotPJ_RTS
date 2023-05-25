using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyEye : MonoBehaviour
{
	public static EnemyEye instance;
    
	public Perceive perceived = new Perceive();

	List<UnitController> foundUnits = new List<UnitController>();

	private void Awake()
	{
		instance = this;

		perceived.ResetMap(false);

		
		perceived.AddOnUpd(Perceive.PosToIdxVector(transform.position), 10);
	}
	
	private void LateUpdate()
	{
		perceived.UpdateMap();
		
	}
}
