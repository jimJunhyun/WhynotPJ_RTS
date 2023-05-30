using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		if(!ListComparison<UnitController>(foundUnits, perceived.founds))
		{
			foundUnits = perceived.founds;
			EnemyBrain.instance.ReactTo(foundUnits);
		}
	}


	bool ListComparison<T>(List<T> left, List<T> right)
	{
		IEnumerable<T> diff1 = left.Except(right);
		IEnumerable<T> diff2 = right.Except(left);

		return !diff1.Any() && !diff2.Any();
	}
}
