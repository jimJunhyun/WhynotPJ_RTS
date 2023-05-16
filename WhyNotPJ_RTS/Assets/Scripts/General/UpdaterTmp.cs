using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterTmp : MonoBehaviour
{
	public bool isPlayer; //양 측중 한 측을 찾아서 발견해 전달해줄 예정. 아직 IUnit 수정사항이 미완이라 보류중.
	Vector3Int prevPos;
	private void Awake()
	{
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
			if (isPlayer)
			{
				PlayerEye.instance.perceived.AddOffUpd(prevPos, 10);
				prevPos = vec;
				PlayerEye.instance.perceived.AddOnUpd(prevPos, 10);
				//Debug.Log($"플레이어 시야 새로고침됨. {prevPos}");
			}
			else
			{
				prevPos = vec;
				EnemyEye.instance.perceived.AddOnUpd(prevPos, 10);
			}
			
			
		}
    }

	
}
