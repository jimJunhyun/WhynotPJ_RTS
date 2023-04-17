using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterTmp : MonoBehaviour
{
	public bool isPlayer; //�� ���� �� ���� ã�Ƽ� �߰��� �������� ����. ���� IUnit ���������� �̿��̶� ������.
	Vector2Int prevPos;
	private void Awake()
	{
		prevPos = Perceive.PosToIdxVector(transform.position);
	}
	void Update()
    {
		Vector2Int vec =  Perceive.PosToIdxVector(transform.position);
        if(Mathf.Abs(prevPos.x - vec.x )> 1 || Mathf.Abs(prevPos.y - vec.y) > 1)
		{
			if (isPlayer)
			{
				PlayerEye.instance.perceived.UpdateMap(prevPos, 15, false);
				prevPos = vec;
				PlayerEye.instance.perceived.UpdateMapRecur(prevPos, 15, true);
				Debug.Log($"�÷��̾� �þ� ���ΰ�ħ��. {prevPos}");
			}
			else
			{
				prevPos = vec;
				EnemyEye.instance.perceived.UpdateMapRecur(prevPos, 15, true);
			}
			
			
		}
    }

	
}
