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
			//�켱����
			con.Move(Vector3.zero);
			Debug.Log("�������� ����");
		}
		else if(set.warBias >= selected)
		{
				con.state = UnitState.Alert;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("���� �ϳ� ����.");
			
			Debug.Log(con.myName + " ���� �������� ���.");
			//�߰��� �� ������ �����鼭, ����� ���� ������ �����Ͽ��ٸ� ������ ���� ���ο��� ���� ���
			//�߰��� �� ������ ���ٸ� ��� ���·� ������.
			//��Ҵٰ� ����
		}
		else
		{
			//���� �ĵ��ø��� ������ �� ������ ������ ��ġ�Ϸ� �� ���̴�.
			//�װ� ���� AI�� �ϵ��� �غ���

			//�� ������ ������ ��ġ
			//���忡���� �ٽ� ������ �ľ��ϵ��� �ؾ� �ϰڴ�.
			//�׷����� �ϴ� ���� �νĽ�Ű�°ź��� �����ϰ�
			//�����ǾȰ��� �װſ� �°� ���ÿ� ������
		}
	}

	public void WholeAttack()
	{
		for (int i = 0; i < accumulations.Count; i++)
		{
			accumulations[i].Move(EnemyBrain.instance.playerBase);
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
			IUnit curC = myControls.Find((x) => { return x.state == UnitState.Wait; });
			if (curC != null)
			{
				SamplePos(curC);
			}
		}
	}
}
