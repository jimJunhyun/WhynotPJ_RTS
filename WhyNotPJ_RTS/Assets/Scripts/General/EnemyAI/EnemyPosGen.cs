using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPosGen : MonoBehaviour
{
	public static EnemyPosGen instance;

	public EnemySettings set;
	public Transform basePos;
	public float randomAmount;

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
			//정찰계열 유닛
			//가장 가까운 미발견 지형으로 이동하도록 하기..
			//미발견 지형이 더 넓은 방향으로 이동하도록?
			con.Move(Vector3.zero);
			Debug.Log("정찰적인 조작");
		}
		else if(set.warBias >= selected)
		{
			if(EnemyBrain.instance.playerBase != null)
			{
				con.Move(EnemyBrain.instance.playerBase);
				Debug.Log(con.myName+"에게 공격적인 조작");
			}
			else
			{
				con.state = UnitState.Wait;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("유닛 하나 더함.");
			}
			Debug.Log(con.myName + " 에게 공격적인 명령.");
			//발견한 적 본진이 있다면 그 근처로 이동
			//발견한 적 본진이 없다면 대기 또는 경계
		}
		else
		{
			IBuilding vulBuilding = buildings.Find(x=>x.nearUnit.Count == 0);
			if(vulBuilding == null)
			{
				vulBuilding = EnemyBrain.instance.GetComponent<Base>();
			}
			vulBuilding.nearUnit.Add(con); 
			Vector3 rand = randomAmount * Random.insideUnitSphere;
			rand.y = 0;
			con.objPos = vulBuilding.pos + rand;
			con.state = UnitState.Alert;
			//사람이 아무도 없는 아군 건물이 있다면 그 근처로 이동
			//사람이 아무도 없는 아군 건물이 근처에 있다면 경계 태세
			//사람이 아무도 없는 아군 건물이 없다면 본진 주위에서 경계 태세
			Debug.Log(con.myName + " 에게 방어적인 명령.");
		}
	}

	public void WholeAttack()
	{
		for (int i = 0; i < accumulations.Count; i++)
		{
			SamplePos(accumulations[i]);
		}
		Debug.Log("총공격");
	}

	public void FindPlaying()
	{
		if (curAccumulated >= set.adequateSoldier)
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
