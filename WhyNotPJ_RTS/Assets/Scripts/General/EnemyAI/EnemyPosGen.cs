using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;



/// <summary>
/// 전략이란 무엇이며, 언제 사용하는가?
/// 또한 전략을 사용하기 위해서는 필요한 것이 무엇인가?
/// 
/// 적을 섬멸하는 전략
///		우세 : 적과의 전면전
///		동등 : 농성
///		열세 : 유인하여 각개격파
///	적을 우회하는 전략
///		우세 : 망치와 모루
///		동등 : 먹여치기&환격
///		열세 : 은밀기동
///	적을 속이는 전략
///		우세 : 복병으로 유인
///		동등 : 양동 전략
///		열세 : 가치부전 (의도적인 유닛 희생 = 닥돌 => 시간 벌기 / 또는 무작위 조작) (손해최소화-우세 전략과 유사하여 플레이어를 속임.)
/// 손해를 최소화하는 전략
///		우세 : 물귀신
///		동등 : 도주
///		열세 : 먼 곳으로 유인
/// 
/// 전략의 범위는 위와같다.
/// 그렇다면 이들의 특징을 살펴보자.
/// 섬멸, 우회, 기만책은 각각 공, 수, 기동 성향에 대응된다.
/// 또한 불리한 전투의 경우 손해 최소화 전략을 사용할 수 있다.
/// 
/// 전략은 전투를 인식하는 데에서 사용을 시작할 수 있다.
/// 
/// 전투
/// 예상 충돌 지점을 마련하고 거기서부터 일정 거리 안에 있는 애들이 참여인원이 되겠다.
/// 
/// </summary>

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

		UnitBaseState currentState = (con.CurrentStateScript as UnitBaseState);

		if (con.element.rec >= 5f)
		{
			Vector3 v = Perceive.IdxVectorToPos(FindNearestSightless(con));
			NavMeshHit hit;
			NavMesh.SamplePosition(v, out hit, 100f, NavMesh.AllAreas);
			currentState.unitMove.SetTargetPosition(hit.position);
			
			//Debug.Log($"정찰적인 조작 : {hit.position}");
		}
		else if(set.warBias >= selected)
		{
			if(EnemyEye.instance.perceived.founds.Count > 0)
			{
				if(EnemyBrain.instance.predictedFights != null)
				{

				}
				currentState.unitMove.SetTargetPosition(FindNearestUnit(currentState.transform.position, EnemyEye.instance.perceived.founds));
			}
			else
			{
				con.ChangeState(State.Alert);
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("유닛 하나 축적");
			}
			
			
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
		Vector3Int dest = Perceive.PosToIdxVector(EnemyBrain.instance.transform.position);
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

	Transform FindNearestUnit(Vector3 fromPos, List<UnitController> units)
	{
		float dist = float.MaxValue;
		Transform found = null;
		for (int i = 0; i < units.Count; i++)
		{
			float tempDist;
			if(dist > (tempDist = (fromPos - units[i].transform.position).sqrMagnitude))
			{
				found = units[i].transform;
				dist = tempDist;
			}
		}
		return found;
	}

	public void WholeAttack()
	{
		while(accumulations.Count > 0)
		{
			((UnitBaseState)accumulations[0].CurrentStateScript).unitMove.SetTargetPosition(EnemyBrain.instance.playerBase.position);
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
			UnitController curC = myControls.Find((x) => { return x?.CurrentStateScript == x?.GetStateDict(State.Wait); });
			if (curC != null)
			{
				SamplePos(curC);
			}
			//유닛의 현재 상태를 인지할 수 있도록 하는 것이 무난하겠다.
		}
	}
}
