using Core;
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

	public List<UnitController> myControls = new List<UnitController>();
	public List<UnitController> accumulations = new List<UnitController>();
	public List<IBuilding> buildings = new List<IBuilding>();

	private void Awake()
	{
		instance = this;
	}

	public void SamplePos(UnitController con)
	{
		float selected = Random.Range(0, set.passiveBias + set.warBias);

		UnitBaseState currentState = (con.CurrentState as UnitBaseState);


		if (con._element.rec >= 5f)
		{
			currentState.unitMove.SetTargetPosition(Perceive.IdxVectorToPos(FindNearestSightless(con)));
			//con.unitMove.SetTargetPosition(Vector3.zero);
			//con.Move(Vector3.zero);
			Debug.Log("�������� ����");
		}
		else if(set.warBias >= selected)
		{
			if(EnemyBrain.instance.playerBase != null)
			{
				//con.unitMove.SetTargetPosition(EnemyBrain.instance.playerBase.position);
				Debug.Log(con._myName + "���� �������� ����");
			}
			else
			{
				con.ChangeState(State.Alert);
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("���� �ϳ� ����.");
			}
			Debug.Log(con.name + " ���� �������� ����.");
		}
		else
		{
			Vector3 v = Perceive.IdxVectorToPos(FindHighestHeightIdx());

			con.ChangeState(State.Alert); 
			Debug.Log(con.name + " ���� ������� ����");
		}
	}

	public Vector3Int FindNearestSightless(UnitController unit)
	{
		Vector3Int from = new Vector3Int(100, 100);
		Vector3Int dest = Vector3Int.zero;
		float smallestD = float.MaxValue;
		//from = Perceive.PosToIdxVector(unit.transform);
		for (int y = 0; y < Perceive.MAPY; ++y)
		{
			for (int x = 0; x < Perceive.MAPX; ++x)
			{
				int floor = 0;
				if(Perceive.fullMap[y, x, 1].Id != 0)
				{
					floor = 1;
				}
				if (EnemyEye.instance.perceived.map[y, x, floor] <= 0)
				{
					float dist = MapData.GetDist(Perceive.IdxVectorToPos(from), Perceive.IdxVectorToPos(new Vector3Int(x, y)));
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

	public Vector3Int FindHighestHeightIdx()
	{
		float largestH = float.MinValue;
		Vector3Int v = Perceive.PosToIdxVector(EnemyBrain.instance.transform.position);
		for (int y = 0; y < Perceive.MAPY; y++)
		{
			for (int x = 0; x < Perceive.MAPX; x++)
			{
				int floor = 0;
				if (Perceive.fullMap[y, x, 1].Id != 0)
				{
					floor = 1;
				}
				if (EnemyEye.instance.perceived.map[y, x, floor] > 0)
				{
					float num = Perceive.fullMap[y, x, floor].height * set.heightBias  - MapData.GetDist(Perceive.IdxVectorToPos(new Vector3Int(x, y)), EnemyBrain.instance.transform.position) * set.distBias;
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
			((UnitBaseState)accumulations[i].CurrentState).unitMove.SetTargetPosition(EnemyBrain.instance.playerBase.position);
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
			//UnitController curC = myControls.Find((x) => { return x.CurrentState == State.Wait; });
			//if (curC != null)
			//{
			//	SamplePos(curC);
			//}
			//유닛의 현재 상태를 인지할 수 있도록 하는 것이 무난하겠다.
		}
	}
}
