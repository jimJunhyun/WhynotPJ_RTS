using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPosGen : MonoBehaviour
{
	public static EnemyPosGen instance;

	public EnemySettings set;
	public Transform basePos;
	public float calcApt;
	public float calcRad;

	public GameObject sphere;

	int curAccumulated = 0;

	public List<IUnit> myControls = new List<IUnit>();
	public List<IUnit> accumulations = new List<IUnit>();
	public List<IBuilding> buildings = new List<IBuilding>();

	private void Awake()
	{
		instance = this;
	}

	public void SamplePos(IUnit con)
	{
		float selected = Random.Range(0, set.passiveBias + set.warBias);
		
		if (con.element.rec >= 5f)
		{
			con.Move(Perceive.IdxVectorToPos(FindNearestSightless(con)));
			//�����迭 ����
			//���� ����� �̹߰� �������� �̵��ϵ��� �ϱ�..
			//�̹߰� ������ �� ���� �������� �̵��ϵ���?
			//�켱����
			//con.Move(Vector3.zero);
			Debug.Log("�������� ����");
		}
		else if(set.warBias >= selected)
		{
				con.state = UnitState.Alert;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("���� �ϳ� ����.");
			
			Debug.Log(con.myName + " ���� �������� ���.");
			//�߰��� �� ������ �����鼭, ����� ���� ������ �����Ͽ��ٸ� ������ ���� ���ο��� ���� ���
			//�߰��� �� ������ ���ٸ� ��� ���·� ������.
			//��Ҵٰ� ����
		}
		else
		{
			Vector3 v = Perceive.IdxVectorToPos(FindHighestHeightIdx());
			con.Move(v); 

			con.state = UnitState.Alert;//�Ϸ�� ���� ���� ����. (�Ϸ�� �ൿ�� �����ϱ�, �Ǵ� ������ �ڷ�ƾ���� �����Ͽ� ��ȯ���� ��ٸ��� �ϱ�??)
			Debug.Log(con.myName + " ���� ������� ���");

			//������ ����� ������ ���� ��.
			//�Ÿ����� �ݺ��, ���̿��� ���
			
		}
	}

	public Vector2Int FindNearestSightless(IUnit unit) //���� ������ �ʿ���. ���ֿ� ���Ѱŵ� �˻� ���ǿ� ���Ѱŵ�
	{
		Vector2Int from = new Vector2Int(100, 100);
		Vector2Int dest = Vector2Int.zero;
		float smallestD = float.MaxValue;
		//from = Perceive.PosToIdxVector(unit.transform);
		for (int y = 0; y < Perceive.MAPY; ++y)
		{
			for (int x = 0; x < Perceive.MAPX; ++x)
			{
				if(!EnemyEye.instance.perceived.map[y, x].visiblity)
				{
					float dist = MapData.GetDist(Perceive.IdxVectorToPos(from), Perceive.IdxVectorToPos(new Vector2Int(x, y)));
					Debug.Log($"{x}, {y} : {dist}");
					if (smallestD > dist)
					{
						dest.x = x;
						dest.y = y;
						smallestD = dist;
					}
				}
			}
		}
		return dest;
	}

	public Vector2Int FindHighestHeightIdx() //�����鼭 ������� ��ȣ��.
	{
		float largestH = float.MinValue;
		Vector2Int v = Perceive.PosToIdxVector(EnemyBrain.instance.transform.position);
		for (int y = 0; y < Perceive.MAPY; y++)
		{
			for (int x = 0; x < Perceive.MAPX; x++)
			{
				if(EnemyEye.instance.perceived.map[y, x].visiblity) 
				{
					float num = EnemyEye.instance.perceived.map[y, x].height * set.heightBias  - MapData.GetDist(Perceive.IdxVectorToPos(new Vector2Int(x, y)), EnemyBrain.instance.transform.position) * set.distBias;
					if (num > largestH) 
					{
						v.x = x;
						v.y = y;
						largestH = num;
					}
					
				}
			}
		}
		//Debug.Log(pos);
		//Debug.Log($"{Perceive.IdxVectorToPos(pos)}");
		//EditorApplication.isPaused = true;
		return v;
	}

	public void WholeAttack()
	{
		for (int i = 0; i < accumulations.Count; i++)
		{
			accumulations[i].Move(EnemyBrain.instance.playerBase);
		}
		Debug.Log("�Ѱ���");
	}

	public void FindPlaying()
	{
		if (EnemyBrain.instance.playerBase != null && curAccumulated >= accumulations.Count)
		{
			WholeAttack();
		}
		else if (myControls.Count > 0)
		{
			IUnit curC = myControls.Find((x) => { return x.state == UnitState.Wait; });
			if (curC != null)
			{
				SamplePos(curC);
			}
		}
	}
}
