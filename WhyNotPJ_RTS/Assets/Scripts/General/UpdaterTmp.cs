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
		Vector2 vec = Perceive.PosToIdxVector(transform.position);
        if(Mathf.Abs(prevPos.x - vec.x )> 1 || Mathf.Abs(prevPos.y - vec.y) > 1)
		{
			if (isPlayer)
			{
				PlayerEye.instance.perceived.UpdateMap(prevPos, 5, false);
				prevPos = Perceive.PosToIdxVector(transform.position);
				PlayerEye.instance.perceived.UpdateMap(prevPos, 5, true);
				Debug.Log("�÷��̾� �þ� ���ΰ�ħ��.");
			}
			else
			{
				EnemyEye.instance.perceived.UpdateMap(prevPos, 5, true);
				
			}
			
			
		}
    }

	
}
