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

	//public GameObject sphere;

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
			Vector3 v = Perceive.IdxVectorToPos(FindNearestSightless(con));
			NavMeshHit hit;
			NavMesh.SamplePosition(v, out hit, 100f, NavMesh.AllAreas);
			currentState.unitMove.SetTargetPosition(hit.position);
			
			//Debug.Log($"정찰적인 조작 : {hit.position}");
		}
		else if(set.warBias >= selected)
		{
			//if(EnemyBrain.instance.playerBase != null)
			//{
			//	NavMeshHit hit;
			//	NavMesh.SamplePosition(EnemyBrain.instance.playerBase.position, out hit, 100f, NavMesh.AllAreas);
			//	currentState.unitMove.SetTargetPosition(hit.position);
			//	con.ChangeState(State.Attack);
			//	Debug.Log(con.name + "을 적 본진으로 보냄.");
			//}
			//else
			//{
				con.ChangeState(State.Alert);
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("유닛 하나 축적");
			//}
			Debug.Log(con.name + " 에게 공격적인 조작.");
		}
		else
		{
			Vector3 v = Perceive.IdxVectorToPos(FindHighestHeightIdx());
			NavMeshHit hit;
			NavMesh.SamplePosition(v, out hit, 100f, NavMesh.AllAreas);
			currentState.unitMove.SetTargetPosition(hit.position);
			con.ChangeState(State.Alert); 
			Debug.Log(con.name + " 에게 방어적인 조작 : "+hit.position);
		}
	}

	public Vector3Int FindNearestSightless(UnitController unit)
	{
		Vector3Int from = Perceive.PosToIdxVector(unit.transform.position);
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
						dest.z = floor;
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
					int units = Physics.OverlapSphere(Perceive.IdxVectorToPos(new Vector3Int(x, y)), 2.5f, 1 << 14).Length; //일단 14를 너놨는데, 나중에 레이어 충돌 생기면 바꾸고 상수로 따로 가져갈거임.
					float num = (Perceive.fullMap[y, x, floor].height/* - Perceive.averageHeight*/) * set.heightBias  - MapData.GetDist(Perceive.IdxVectorToPos(new Vector3Int(x, y)), EnemyBrain.instance.transform.position) * set.distBias;
					if (num * (set.adequateSoldier - units) > largestH) 
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
		while(accumulations.Count > 0)
		{
			((UnitBaseState)accumulations[0].CurrentState).unitMove.SetTargetPosition(EnemyBrain.instance.playerBase.position);
			myControls.Add(accumulations[0]);
			accumulations.RemoveAt(0);
		}
		Debug.Log("총공격명령");
	}

	public void FindPlaying()
	{
		if (EnemyBrain.instance.playerBase != null && accumulations.Count >= set.adequateSoldier)
		{
			WholeAttack();
		}
		else if (myControls.Count > 0)
		{
			UnitController curC = myControls.Find((x) => { return x?.CurrentState == x?.GetStateDict(State.Wait); });
			if (curC != null)
			{
				SamplePos(curC);
			}
			//유닛의 현재 상태를 인지할 수 있도록 하는 것이 무난하겠다.
		}
	}
}
