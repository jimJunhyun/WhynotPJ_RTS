using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;



/// <summary>
/// 전술은 전투를 승리로 이끌기 위해서 사용하는 일체의 수단이다.
/// 
/// 적을 섬멸하는 전술 (중요도 : 아군 유닛의 손실 억제 <= 전략에 걸리는 시간 감소)
///		우세 : 전면전 + 적극적 추격
///		동등 : 망치와 모루 (보고도 대응이 어려움)
///		열세 : 유인하여 각개격파
///	적을 붙잡는 전술 (중요도 : 아군 유닛의 손실 억제 >= 전략에 걸리는 시간 감소)
///		우세 : 방어
///		동등 : 먹여치기&환격
///		열세 : 게릴라
///	적을 속이는 전술 (전투 자체를 파악하기 어렵게 함.)
///		우세 : 복병으로 유인
///		동등 : 양동 공격 (통찰하면 대응할 수 있음.)
///		열세 : 가치부전 (의도적인 유닛 희생 = 닥돌 => 시간 벌기 / 또는 무작위 전술) (다른 전술과 유사하여 플레이어를 속임.)
/// 
/// 전술의 범위는 위와같다.
/// 그렇다면 이들의 특징을 살펴보자.
/// 섬멸, 저지, 기만책은 각각 공격, 수비, 기동 성향에 대응된다.
/// 
/// 전술은 전투를 인식하는 데에서 사용을 시작할 수 있다.
/// 
/// 전투
/// 예상 충돌 지점을 마련하고 거기서부터 일정 거리 안에 있는 애들이 참여인원이 되겠다.
/// 
/// 만들기 쉬운 순서로 만들어보자
/// 
/// 
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
			
		}
		else
		{
			con.ChangeState(State.Wait);
			accumulations.Add(con);
			myControls.Remove(con);
			Debug.Log("유닛 하나 축적");
		}


		//생각해보니 전투를 인식하고 전술을 쓰는건 여기가 아님.
		//여기는 그냥 추가적인 유닛에 대한 조작일 뿐이고, 실상 전투는 별개로 처리가  들어가야 할 것임.
		//일단 없는 셈 치고 합치는걸로
		//if (EnemyEye.instance.perceived.founds.Count > 0 && accumulations.Count >= set.adequateSoldier)
		//{
		//	if (EnemyBrain.instance.predictedFights != null)
		//	{
		//		EnemyBrain.instance.predictedFights.engagedAIUnits = accumulations;
		//		switch (EnemyBrain.instance.predictedFights.ResultEstimate())
		//		{
		//			case Result.Draw:
		//				{
		//					if (set.violenceBias > set.defendBias)
		//					{
		//						if (set.violenceBias > set.reconBias)
		//						{
		//							//섬멸-동등
		//						}
		//						else
		//						{
		//							//기만-동등
		//						}
		//					}
		//					else
		//					{
		//						if (set.defendBias > set.reconBias)
		//						{
		//							//저지-동등
		//						}
		//						else
		//						{
		//							//기만-동등
		//						}
		//					}
		//				}
		//				break;
		//			case Result.EnemyWin:
		//				{
		//					if (set.violenceBias > set.defendBias)
		//					{
		//						if (set.violenceBias > set.reconBias)
		//						{
		//							//섬멸-우세
		//						}
		//						else
		//						{
		//							//기만-우세
		//						}
		//					}
		//					else
		//					{
		//						if (set.defendBias > set.reconBias)
		//						{
		//							//저지-우세
		//						}
		//						else
		//						{
		//							//기만-우세
		//						}
		//					}
		//				}
		//				break;
		//			case Result.PlayerWin:
		//				{
		//					if (set.violenceBias > set.defendBias)
		//					{
		//						if (set.violenceBias > set.reconBias)
		//						{
		//							//섬멸-열세
		//						}
		//						else
		//						{
		//							//기만-열세
		//						}
		//					}
		//					else
		//					{
		//						if (set.defendBias > set.reconBias)
		//						{
		//							//저지-열세
		//						}
		//						else
		//						{
		//							//기만-열세
		//						}
		//					}
		//				}
		//				break;
		//		}
		//	}
		//	currentState.unitMove.SetTargetPosition(FindNearestUnit(currentState.transform.position, EnemyEye.instance.perceived.founds));
		//}
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

	//public Vector3Int FindHighestHeightIdx()
	//{
	//	float largestH = float.MinValue;
	//	Vector3Int v = Perceive.PosToIdxVector(EnemyBrain.instance.transform.position);
	//	for (int y = 0; y < Perceive.MAPY; y++)
	//	{
	//		for (int x = 0; x < Perceive.MAPX; x++)
	//		{
	//			int floor = 0;
	//			if (Perceive.fullMap[y, x, 1].Id != 0)
	//			{
	//				floor = 1;
	//			}
	//			if (EnemyEye.instance.perceived.map[y, x, floor] > 0)
	//			{
	//				int units = Physics.OverlapSphere(Perceive.IdxVectorToPos(new Vector3Int(x, y)), 2.5f, 1 << 14).Length; //일단 14를 너놨는데, 나중에 레이어 충돌 생기면 바꾸고 상수로 따로 가져갈거임.
	//				float num = (Perceive.fullMap[y, x, floor].height/* - Perceive.averageHeight*/) * set.heightBias  - MapData.GetDist(Perceive.IdxVectorToPos(new Vector3Int(x, y)), EnemyBrain.instance.transform.position) * set.distBias;
	//				if (num * (set.adequateSoldier - units) > largestH) 
	//				{
	//					v.x = x;
	//					v.y = y;
	//					largestH = num;
	//				}
					
	//			}
	//		}
	//	}
	//	//Debug.Log(pos);
	//	//Debug.Log($"{Perceive.IdxVectorToPos(pos)}");
	//	//EditorApplication.isPaused = true;
	//	return v;
	//}

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
			if (((UnitBaseState)accumulations[0].CurrentStateScript).unitMove.SetTargetPosition(EnemyBrain.instance.playerBase))
			{
				accumulations[0].ChangeState(State.Move);
			}
			else{
				Debug.Log("이동 실패");
				accumulations[0].ChangeState(State.Wait);
			}
			myControls.Add(accumulations[0]);
			accumulations.RemoveAt(0);
		}
		Debug.Log("총공격명령");
	}

	public void FindPlaying()
	{
		if (EnemyBrain.instance.playerBase != null && accumulations.Count >= set.adequateSoldier)
		{
			WholeAttack(); //반 임시
		}
		else if (myControls.Count > 0)
		{
			UnitController curC = myControls.Find((x) => { return x?.currentState == State.Wait; });
			if (curC != null)
			{
				SamplePos(curC);
			}
			
		}
	}
}
