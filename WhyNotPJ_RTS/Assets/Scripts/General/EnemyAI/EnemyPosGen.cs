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
			if(EnemyBrain.instance.playerBase != null && curAccumulated >= accumulations.Count)
			{
				WholeAttack();
			}
			else
			{
				con.state = UnitState.Alert;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("유닛 하나 더함.");
			}
			Debug.Log(con.myName + " 에게 공격적인 명령.");
			//발견한 적 본진이 있으면서, 충분한 양의 유닛을 축적하였다면 축적된 유닛 전부에게 공격 명령
			//발견한 적 본진이 없다면 경계 상태로 축적함.
			//모았다가 러쉬!
		}
		else
		{
			//적이 쳐들어올만한 곳으로 방어에 유리한 유닛을 배치하려 할 것이다.
			//그걸 이제 AI가 하도록 해보자

			//자
			//일단 쳐들어올만한 곳을 물색하는거다
			//쳐들어오는 곳은 약한 곳이지
			//그럼 약한 곳을 보강하는 것이 곧 방어이다.
			//약한 곳은 그럼 어디냐
			//어디지?
			//...
		}
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
