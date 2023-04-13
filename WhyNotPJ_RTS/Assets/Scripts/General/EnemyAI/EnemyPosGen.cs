using Core;
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

		IUnitState currentState = con.CurrentState;
		
		if (con._element.rec >= 5f)
		{
			//�����迭 ����
			//���� ����� �̹߰� �������� �̵��ϵ��� �ϱ�..
			//�̹߰� ������ �� ���� �������� �̵��ϵ���?
			//con.unitMove.SetTargetPosition(Vector3.zero);
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
				con.ChangeState(State.Wait);
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("���� �ϳ� ����.");
			}
			Debug.Log(con._myName + "���� �������� ���.");
			//�߰��� �� ������ �ִٸ� �� ��ó�� �̵�
			//�߰��� �� ������ ���ٸ� ��� �Ǵ� ���
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
			//con.objPos = vulBuilding.pos + rand;
			//con�� �� �̻� objPos�� �����Ƿ� ���� objPos�� ����ؾ� �Ѵٸ� ������ ���� con.SetTargetPosition�� �־��ֱ�
			con.ChangeState(State.Wait);
			//����� �ƹ��� ���� �Ʊ� �ǹ��� �ִٸ� �� ��ó�� �̵�
			//����� �ƹ��� ���� �Ʊ� �ǹ��� ��ó�� �ִٸ� ��� �¼�
			//����� �ƹ��� ���� �Ʊ� �ǹ��� ���ٸ� ���� �������� ��� �¼�
			Debug.Log(con._myName + " ���� ������� ���.");
		}
	}

	public void WholeAttack()
	{
		for (int i = 0; i < accumulations.Count; i++)
		{
			SamplePos(accumulations[i]);
		}
		Debug.Log("�Ѱ���");
	}

	public void FindPlaying()
	{
		if (curAccumulated >= set.adequateSoldier)
		{
			WholeAttack();
		}
		else if (myControls.Count > 0)
		{
			/*UnitController curC = myControls.Find((x) => { return x.CurrentState == ; });
			if (curC != null)
			{
				SamplePos(curC);
			}*/
		}
	}
}
