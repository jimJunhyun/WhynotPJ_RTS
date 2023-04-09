using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterTmp : MonoBehaviour
{
	public bool isPlayer; //양 측중 한 측을 찾아서 발견해 전달해줄 예정. 아직 IUnit 수정사항이 미완이라 보류중.
	Vector2Int prevPos;
	private void Awake()
	{
		prevPos = Perceive.PosToIdxVector(transform.position);
	}
	void Update()
    {
		Vector2 vec = Perceive.PosToIdxVector(transform.position);
        if(Mathf.Abs(prevPos.x - vec.x )> 1 || Mathf.Abs(prevPos.y - vec.y) > 1)
		{
			if (isPlayer)
			{
				PlayerEye.instance.perceived.UpdateMap(prevPos, 5, false);
				prevPos = Perceive.PosToIdxVector(transform.position);
				PlayerEye.instance.perceived.UpdateMap(prevPos, 5, true);
				Debug.Log("플레이어 시야 새로고침됨.");
			}
			else
			{
				EnemyEye.instance.perceived.UpdateMap(prevPos, 5, true);
				
			}
			
			
		}
    }

	
}
