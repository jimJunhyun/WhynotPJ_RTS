using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class UnitManageData //지휘에 필요한 정보들
{
	public UnitController con;
	public bool isReceivingCommand;

	public UnitManageData(UnitController cont, bool isReceiving)
	{
		con = cont;
		isReceivingCommand = isReceiving;
	}
}

public class EnemyPosGen : MonoBehaviour
{
	public static EnemyPosGen instance;

	public Transform basePos;
	public float calcApt;
	public float calcRad;

	//public GameObject sphere;

	public List<UnitManageData> myControls = new List<UnitManageData>();
	public List<UnitManageData> accumulations = new List<UnitManageData>();
	public List<IBuilding> buildings = new List<IBuilding>();

	

	public void SamplePos(UnitManageData con)
	{

		UnitBaseState currentState = (con.con.CurrentStateScript as UnitBaseState);

		if (con.con.element.rec >= 5f)
		{
			Vector3 v = Perceive.IdxVectorToPos(FindNearestSightless(con.con));
			NavMeshHit hit;
			NavMesh.SamplePosition(v, out hit, 100f, NavMesh.AllAreas);
			if (currentState.unitMove.SetTargetPosition(hit.position)) 
				con.con.ChangeState(State.Move);
			
		}
		else
		{
			con.isReceivingCommand = false;
			con.con.ChangeState(State.Wait);
			accumulations.Add(con);
			myControls.Remove(con);
			Debug.Log("유닛 하나 축적 : " + con.con.myName);
		}


		//생각해보니 전투를 인식하고 전술을 쓰는건 여기가 아님.
		//여기는 그냥 추가적인 유닛에 대한 조작일 뿐이고, 실상 전투는 별개로 처리가  들어가야 할 것임.
		//일단 없는 셈 치고 합치는걸로
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
					Debug.Log("2ㅊ으");
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
			if (((UnitBaseState)accumulations[0].con.CurrentStateScript).unitMove.SetTargetPosition(EnemyBrain.instance.playerBase))
			{
				accumulations[0].con.ChangeState(State.Move);
			}
			else{
				Debug.Log("이동 실패");
				accumulations[0].con.ChangeState(State.Wait);
			}
			myControls.Add(accumulations[0]);
			accumulations.RemoveAt(0);
		}
		Debug.Log("총공격명령");
	}

	public void FindPlaying()
	{
		if (EnemyEye.instance.foundBase && accumulations.Count >= EnemyBrain.instance.set.adequateSoldier)
		{
			WholeAttack(); //반 임시
		}
		else if (myControls.Count > 0)
		{
			UnitManageData curC = myControls.Find((x) => x.con.currentState == State.Wait && x?.isReceivingCommand == true );
			if (curC != null)
			{
				//Debug.Log($"{curC.con.myName} found, {curC.con.currentState} && {curC.isReceivingCommand}");
				SamplePos(curC);
			}
			
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		EnemyDiffSet.instance.AddUpdateActs(FindPlaying);
	}
}
