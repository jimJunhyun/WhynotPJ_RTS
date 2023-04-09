using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPosGen : MonoBehaviour
{
	public static EnemyPosGen instance;

	public EnemySettings set;
	public Transform basePos;
	public float calcApt;
	public float calcRad;

	int curAccumulated = 0;

	public List<IUnit> myControls = new List<IUnit>();
	public List<IUnit> accumulations = new List<IUnit>();
	public List<IBuilding> buildings = new List<IBuilding>();

	Vector2Int pos;

	private void Awake()
	{
		instance = this;
	}

	public void SamplePos(IUnit con)
	{
		float selected = Random.Range(0, set.passiveBias + set.warBias);
		
		if (con.element.rec >= 5f)
		{
			//정찰계열 유닛
			//가장 가까운 미발견 지형으로 이동하도록 하기..
			//미발견 지형이 더 넓은 방향으로 이동하도록?
			//우선보류
			con.Move(Vector3.zero);
			Debug.Log("정찰적인 조작");
		}
		else if(set.warBias >= selected)
		{
				con.state = UnitState.Alert;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("유닛 하나 더함.");
			
			Debug.Log(con.myName + " 에게 공격적인 명령.");
			//발견한 적 본진이 있으면서, 충분한 양의 유닛을 축적하였다면 축적된 유닛 전부에게 공격 명령
			//발견한 적 본진이 없다면 경계 상태로 축적함.
			//모았다가 러쉬
		}
		else
		{
			Vector3 v = Perceive.IdxVectorToPos(FindHighestHeightIdx());
			con.Move(v); 

			con.state = UnitState.Alert;//완료시 상태 경계로 변경. (완료시 행동을 연결하기, 또는 선택을 코루틴으로 변경하여 반환까지 기다리게 하기??)
			Debug.Log(con.myName + " 에게 방어적인 명령");

			//본진과 충분히 가까우며 높은 곳.
			//거리에는 반비례, 높이에는 비례
			
		}
	}

	public Vector2Int FindHighestHeightIdx() //높으면서 가까움을 선호함.
	{
		float largestH = float.MinValue;
		/*Vector2Int v*/pos = Perceive.PosToIdxVector(EnemyBrain.instance.transform.position);
		for (int y = 0; y < Perceive.MAPY; y++)
		{
			for (int x = 0; x < Perceive.MAPX; x++)
			{
				if(EnemyEye.instance.perceived.map[y, x].visiblity) 
				{
					float num = EnemyEye.instance.perceived.map[y, x].height * (set.heightBias / (set.distBias + 1)) - MapData.GetDist(Perceive.IdxVectorToPos(new Vector2Int(y, x)), EnemyBrain.instance.transform.position) * (set.distBias / (set.heightBias + 1));
					//높이가 연산에 안들어가는듯?
					Debug.Log($"{x} ,  {y} : {num}");
					if (num > largestH) 
					{
						pos.x = y;
						pos.y = x;
						largestH = num;
					}
					
				}
			}
		}
		return pos;
	}

	public void WholeAttack()
	{
		for (int i = 0; i < accumulations.Count; i++)
		{
			accumulations[i].Move(EnemyBrain.instance.playerBase);
		}
		Debug.Log("총공격");
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

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(Perceive.IdxVectorToPos(pos), 1);
	}
}
