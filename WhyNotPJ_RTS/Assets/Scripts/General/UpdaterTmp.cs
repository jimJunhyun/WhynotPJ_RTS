using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterTmp : MonoBehaviour
{
	UnitController unit;
	Vector3Int prevPos;
	private void OnEnable()
	{
		unit = GetComponent<UnitController>();
		prevPos = Perceive.PosToIdxVector(transform.position);
		
	}

	private void Start()
	{
		PlayerEye.instance.perceived.AddOnUpd(prevPos, 10);
	}
	void Update()
    {
		Vector3Int vec =  Perceive.PosToIdxVector(transform.position);
        if(Mathf.Abs(prevPos.x - vec.x )> 1 || Mathf.Abs(prevPos.y - vec.y) > 1)
		{
			if (unit.isPlayer)
			{
				PlayerEye.instance.perceived.AddOffUpd(prevPos, 10);
				prevPos = vec;
				PlayerEye.instance.perceived.AddOnUpd(prevPos, 10);
				//
			}
			else
			{
				prevPos = vec;
				EnemyEye.instance.perceived.AddOnUpd(prevPos, 10);
				//Debug.Log($"적 시야 새로고침됨. {prevPos}");
			}
			
			
		}
    }

	
}
