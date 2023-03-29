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
			//�����迭 ����
			//���� ����� �̹߰� �������� �̵��ϵ��� �ϱ�..
			//�̹߰� ������ �� ���� �������� �̵��ϵ���?
			con.Move(Vector3.zero);
			Debug.Log("�������� ����");
		}
		else if(set.warBias >= selected)
		{
			if(EnemyBrain.instance.playerBase != null)
			{
				con.Move(EnemyBrain.instance.playerBase);
				Debug.Log(con.myName+"���� �������� ����");
			}
			else
			{
				con.state = UnitState.Wait;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("���� �ϳ� ����.");
			}
			Debug.Log(con.myName + " ���� �������� ���.");
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
			con.objPos = vulBuilding.pos + rand;
			con.state = UnitState.Alert;
			//����� �ƹ��� ���� �Ʊ� �ǹ��� �ִٸ� �� ��ó�� �̵�
			//����� �ƹ��� ���� �Ʊ� �ǹ��� ��ó�� �ִٸ� ��� �¼�
			//����� �ƹ��� ���� �Ʊ� �ǹ��� ���ٸ� ���� �������� ��� �¼�
			Debug.Log(con.myName + " ���� ������� ���.");
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
			IUnit curC = myControls.Find((x) => { return x.state == UnitState.Wait; });
			if (curC != null)
			{
				SamplePos(curC);
			}
		}
	}
}
